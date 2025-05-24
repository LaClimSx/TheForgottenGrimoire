using UnityEngine;

public class Batterie : MonoBehaviour
{
    [SerializeField] private GameObject trap;
    [SerializeField] private GameObject energyLevel;
    [SerializeField] private Transform chargeDisplay;
    private InteractableChargeable batterie;
    private InteractorElec sendElec;
    private float batterieCharge = 0f;
    private bool trapActivated = false;

    private void Awake()
    {
        batterie = energyLevel.GetComponent<InteractableChargeable>();
        sendElec = energyLevel.GetComponent<InteractorElec>();
    }

    private void Update()
    {
        if (batterieCharge < 1)
        {
            Vector3 scale = chargeDisplay.localScale;
            batterieCharge = batterie.getChargeLevel();
            scale.Set(scale.x, scale.y, batterieCharge);
            chargeDisplay.localScale = scale;
        }      
        else if (!trapActivated)
        {
            trapActivated = true;
            trap.SetActive(false);
            if (sendElec != null) sendElec.enabled = true;
        }
    }
}
