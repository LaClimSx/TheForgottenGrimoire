using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableChargeable : InteractableElement
{
    [SerializeField] private float _maxCharge = 50f;

    [SerializeField] private float _blinkInterval;

    private Renderer renderer;
    private Color baseColor;
    private Color blinkColor = Color.cyan;
    private float nextUpdate;
    private bool charging;
    private float nextCharge;

    private float charge = 0f;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        baseColor = renderer.material.color;
    }

    private void Update()
    {
        if (charging && Time.time >= nextCharge)
        {
            incrementCharge();
            print($"[DEBUG] charge level: {getChargeLevel()}, is charging: {charging}");
            nextCharge = Time.time + 1;
        }

        if (charging && Time.time >= nextUpdate)
        {
            blink();
            nextUpdate = Time.time + _blinkInterval;
        }

        if (!charging && renderer.material.color == blinkColor)
        {
            print("[DEBUG] Bob");
            renderer.material.color = Color.green;
        }
    }

    private void incrementCharge()
    {
        charge = charge + Power;
        if (charge > _maxCharge)
        {
            charge = _maxCharge;
            charging = false;
        }
    }

    public float getChargeLevel()
    {
        return charge / _maxCharge;
    } 

    private void OnCollisionEnter(Collision collision)
    {
        InteractorElec interactor = collision.gameObject.CompareTag("palpatine") ? collision.gameObject.GetComponent<Palpatine>().interactor() : collision.gameObject.GetComponent<InteractorElec>();        
        if (interactor != null)
        {
            if (!charging) charging = true;
            Power += interactor.Power;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        InteractorElec interactor = collision.gameObject.GetComponent<InteractorElec>();
        if (interactor != null)
        {
            Power -= interactor.Power;
            if (Power <= 0) charging = false;
        }
    }

    private void blink()
    {
        if (renderer.material.color == baseColor) renderer.material.color += blinkColor;
        else renderer.material.color -= blinkColor;
    }
}
