using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableConductor : InteractableElement
{
    [SerializeField] private float _blinkInterval;
    private float nextUpdate;
    private Renderer renderer;
    private Color baseColor;
    private Color blinkColor = Color.cyan;

    private ConductorManager _conductorManager;

    private void Awake()
    {
        Type = InteractableType.Conductor;
        renderer = GetComponent<Renderer>();
        baseColor = renderer.material.color;
        _conductorManager = GameObject.FindGameObjectWithTag("conductorManager").GetComponent<ConductorManager>();
    }

    private void Update()
    {
        if (Power > 0 && Time.time >= nextUpdate)
        {
            blink();
            nextUpdate = Time.time + _blinkInterval;
        }
        if (Power <= 0 && renderer.material.color == blinkColor)
        {
            renderer.material.color = Color.green;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        InteractorElec interactor = collision.gameObject.GetComponent<InteractorElec>();
        InteractableConductor conductor = collision.gameObject.GetComponent<InteractableConductor>();
        
        if (interactor != null)
        {
            _conductorManager.srcCollidedWithConductor(interactor, this);
        }

        if (conductor != null)
        {
            _conductorManager.ConductorCollidingWithConductor(conductor, this);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        InteractorElec interactor = collision.gameObject.GetComponent<InteractorElec>();
        InteractableConductor conductor = collision.gameObject.GetComponent<InteractableConductor>();

        if (interactor != null)
        {
            _conductorManager.srcLeavingConductor(interactor, this);
        }

        if (conductor != null)
        {
            print("[DEBUG] EXIT COND TRIGGER");
            _conductorManager.conductorLeavingCondictor(conductor, this);   
        }
    }

    public (List<InteractableConductor>, List<InteractorElec>) getNeighbors()
    {
        List<InteractableConductor> neighborsCond = new List<InteractableConductor>();
        List<InteractorElec> neighborsElec = new List<InteractorElec>();
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            InteractableConductor conductor = collider.gameObject.GetComponent<InteractableConductor>();
            InteractorElec elec = collider.gameObject.GetComponent<InteractorElec>();
            if (conductor != null && !conductor.Equals(this)) neighborsCond.Add(conductor);
            if (elec != null && !elec.Equals(this)) neighborsElec.Add(elec);
        }
        return (neighborsCond, neighborsElec);
    }

    private void blink()
    {
        if (renderer.material.color == baseColor) renderer.material.color += blinkColor;
        else renderer.material.color -= blinkColor;
    }
}

//[RequireComponent(typeof(InteractorElec))]
//[RequireComponent(typeof(Collider))]
//public class InteractableConductor : InteractableElement
//{
//    //[SerializeField] private float _conductance;
//    [SerializeField] private float _blinkInterval;

//    private InteractorElec conductor;
//    private Renderer renderer;
//    private Color baseColor;
//    private Color blinkColor = Color.cyan;
//    private List<InteractorElec> interactors;

//    private float nextUpdate;

//    private void Awake()
//    {
//        Type = InteractableType.Conductor;
//        conductor = GetComponent<InteractorElec>();
//        renderer = GetComponent<Renderer>();
//        baseColor = renderer.material.color;
//        conductor.enabled = false;
//        interactors = new List<InteractorElec>();
//    }

//    private void Update()
//    {
//        if (interactors.Count > 0 && Time.time >= nextUpdate)
//        {
//            blink();
//            nextUpdate = Time.time + _blinkInterval;
//        }
//        if (interactors.Count <= 0 && renderer.material.color == blinkColor)
//        {
//            renderer.material.color = Color.green;   
//        }
//    }

//    public void ElecFromAlreadyCollidingSrc(InteractorElec interactor)
//    {
//        if (!interactors.Contains(interactor)) addNewElecSrc(interactor);
//    }

//    /*private void alreadyCollidingConductors()
//    {
//        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
//        foreach (Collider collider in colliders)
//        {
//            InteractableConductor interactable = collider.gameObject.GetComponent<InteractableConductor>(); 
//            if (interactable != null && !interactable.Equals(this)) 
//        }
//    }*/

//    private void OnCollisionEnter(Collision collision)
//    {
//        addNewElecSrc(collision.gameObject.GetComponent<InteractorElec>());   
//    }

//    private void addNewElecSrc(InteractorElec interactor)
//    {
//        if (interactor != null && interactor.Type == InteractorElement.InteractorType.Elec)
//        {
//            interactors.Add(interactor);
//            conductor.Power += interactor.Power;
//            if (!conductor.enabled)
//            {
//                conductor.enabled = true;
//                conductor.GetCurrentFrom(interactor);
//            }
//        }
//    }

//    public void removeElecFromStillCollidingSource(InteractorElec interactor, float power)
//    {
//        if (interactors.Contains(interactor)) removeElecSrc(interactor, power);
//    }

//    private void OnCollisionExit(Collision collision)
//    {
//        print($"[DEBUG] {name} detected an exiting collision with {collision.gameObject.name}");
//        InteractorElec interactor = collision.gameObject.GetComponent<InteractorElec>();
//        if (interactor is not null) removeElecSrc(interactor, interactor.Power);

//    }

//    private void removeElecSrc(InteractorElec interactor, float power)
//    {
//        if (interactor != null && interactor.Type == InteractorElement.InteractorType.Elec)
//        {
//            print($"[DEBUG] Removing elec source {interactor.name} to {name}");
//            interactors.Remove(interactor);
//            conductor.Power -= power;
//            if (conductor.Power <= 0)
//            {
//                conductor.RemoveCurrentFrom(interactor, power);
//                conductor.enabled = false;
//            }
//        }
//    }

//    private void blink()
//    {
//        if (renderer.material.color == baseColor) renderer.material.color += blinkColor;
//        else renderer.material.color -= blinkColor;
//    }
//}
