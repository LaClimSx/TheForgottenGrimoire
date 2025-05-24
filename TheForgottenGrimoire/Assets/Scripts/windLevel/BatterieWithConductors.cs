using UnityEngine;
using System.Collections.Generic;

public class BatterieWithConductors : MonoBehaviour
{
    [SerializeField] private List<InteractableConductor> conductors = new List<InteractableConductor>();
    [SerializeField] private int conductorRequired;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform chargeDisplay;
    [SerializeField] private float maxCharge;
    [SerializeField] private float updateInterval;
    
    private float batterieCharge = 0f;
    private float nextUpdate = 0f;
    private bool targetActivated;    

    private void Update()
    {
        if (Time.time > nextUpdate)
        {
            nextUpdate = Time.time + updateInterval;
            if (batterieCharge < maxCharge && areConductorPowered())
            {
                batterieCharge++;
            }
        }

        if (batterieCharge < maxCharge)
        {
            if (targetActivated)
            {
                targetActivated = false;
                target.SetActive(true);
            }
            Vector3 scale = chargeDisplay.localScale;            
            scale.Set(scale.x, scale.y, batterieCharge/maxCharge);
            chargeDisplay.localScale = scale;
        }
        else if (!targetActivated)
        {
            targetActivated = true;
            target.SetActive(!target.activeSelf);
        }
    }

    private bool areConductorPowered()
    {
        int poweredCount = 0;
        conductors.ForEach(c =>
        {
            if (c.Power > 0) poweredCount++;
        });

        return poweredCount >= conductorRequired; 
    }
}
