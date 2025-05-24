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

    public bool hasEnergy()
    {
        return Power > 0;
    }

    private void Update()
    {
        if (Power > 0 && Time.time >= nextUpdate)
        {
            //print("electrified");
            if (_blinkInterval != 0) blink();
            nextUpdate = Time.time + _blinkInterval;
        }

        if (Power <= 0)
        {
            renderer.material.color = baseColor;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        InteractorElec interactor = collision.gameObject.CompareTag("palpatine") ? collision.gameObject.GetComponent<Palpatine>().interactor() : collision.gameObject.GetComponent<InteractorElec>();
        InteractableConductor conductor = collision.gameObject.GetComponent<InteractableConductor>();
        //print(collision.collider.name + " (is collider " + conductor == null + ") collided with " + name);
        if (interactor != null)
        {
            _conductorManager.srcCollidedWithConductor(interactor, this);
        }

        if (conductor != null)
        {
            //print("conductor conductor collision detected");
            _conductorManager.ConductorCollidingWithConductor(conductor, this);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        InteractorElec interactor = collision.gameObject.GetComponent<InteractorElec>();
        InteractableConductor conductor = collision.gameObject.GetComponent<InteractableConductor>();

        if (interactor != null)
        {
            //print($"[DEBUG] source {interactor.name} exiting collision with {name}");
            _conductorManager.srcLeavingConductor(interactor, this);
        }

        if (conductor != null)
        {
           // print($"[DEBUG] conductor {conductor.name} exiting collision with {name}");
            _conductorManager.conductorLeavingConductor(conductor, this);   
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