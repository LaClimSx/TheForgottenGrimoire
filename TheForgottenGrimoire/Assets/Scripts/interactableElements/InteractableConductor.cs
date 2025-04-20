using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractorElec))]
[RequireComponent(typeof(Collider))]
public class InteractableConductor : InteractableElement
{
    //[SerializeField] private float _conductance;
    [SerializeField] private float _blinkInterval;

    private InteractorElec conductor;
    private Renderer renderer;
    private Color baseColor;
    private Color blinkColor = Color.cyan;
    private List<InteractorElec> interactors;
    private List<InteractorElec> srcInteractors;
    private List<InteractableConductor> listeningConductors;

    private float nextUpdate;

    private void Awake()
    {
        Type = InteractableType.Conductor;
        conductor = GetComponent<InteractorElec>();
        renderer = GetComponent<Renderer>();
        baseColor = renderer.material.color;
        conductor.enabled = false;
        interactors = new List<InteractorElec>();
        srcInteractors = new List<InteractorElec>();
        listeningConductors = new List<InteractableConductor>();
    }

    private void Start()
    {
        collidingConductors();
    }

    private void Update()
    {
        if (interactors.Count > 0 && Time.time >= nextUpdate)
        {
            blink();
            nextUpdate = Time.time + _blinkInterval;
        }
    }

    private void collidingConductors()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            InteractableConductor interactable = collider.gameObject.GetComponent<InteractableConductor>(); 
            if (interactable != null && !interactable.Equals(this)) listeningConductors.Add(interactable);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        InteractorElec interactor = collision.gameObject.GetComponent<InteractorElec>();
        if (interactor != null && interactor.Type == InteractorElement.InteractorType.Elec)
        {
            interactors.Add(interactor);
            bool isSrc = collision.gameObject.GetComponent<InteractableConductor>() != null;
            if (isSrc) srcInteractors.Add(interactor);
            conductor.Power += interactor.Power;
            conductor.enabled = true;
            notifyListeners(interactor, isSrc, true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        InteractorElec interactor = collision.gameObject.GetComponent<InteractorElec>();
        if (interactor != null && interactor.Type == InteractorElement.InteractorType.Elec)
        {
            if (srcInteractors.Contains(interactor))
            {
                srcInteractors.Remove(interactor);
                conductor.Power -= interactor.Power;
                if (srcInteractors.Count == 0) conductor.enabled = false;
                notifyListeners(interactor, true, false);
            }
            else
            {
                interactors.Remove(interactor);
                conductor.Power -= interactor.Power;
                if (conductor.Power <= 0) conductor.enabled = false;
                notifyListeners(interactor, false, false);
            }
        }
    }

    private void notifyListeners(InteractorElec interactor, bool interactorIsSrc, bool isPropaging)
    {
        listeningConductors.ForEach(i =>
        {
            if (isPropaging) i.receiveElec(interactor, interactorIsSrc); 
            else i.noMoreElec(interactor, interactorIsSrc);
        });
    }

    public void receiveElec(InteractorElec interactor, bool interactorIsSrc)
    {
        if (!interactors.Contains(interactor)) 
        {
            conductor.Power += interactor.Power;
            conductor.enabled = true;
            interactors.Add(interactor);
            if (interactorIsSrc) srcInteractors.Add(interactor);
            notifyListeners(interactor, interactorIsSrc, true);
        }
    }

    public void noMoreElec(InteractorElec interactor, bool interactorIsSrc)
    {
        if (interactors.Contains(interactor))
        {
            if (interactorIsSrc)
            {
                srcInteractors.Remove(interactor);
                interactors.Remove(interactor);
                if (srcInteractors.Count == 0) conductor.enabled = false;
                else conductor.Power -= interactor.Power;
                notifyListeners(interactor, interactorIsSrc, false);
            } else
            {
                interactors.Remove(interactor);
                conductor.Power -= interactor.Power;
                notifyListeners(interactor, interactorIsSrc, false);
            }
        }
    }

    private void blink()
    {
        if (renderer.material.color == baseColor) renderer.material.color += blinkColor;
        else renderer.material.color -= blinkColor;
    }
}
