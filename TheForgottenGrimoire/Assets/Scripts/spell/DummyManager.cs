using UnityEngine;
using UnityEngine.InputSystem;

public class DummyManager : MonoBehaviour
{
    [SerializeField] private InputActionReference rightTriggerReference;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject elecball;

    private GameObject toCast;
    private GameObject castedSpell;

    private bool aimimg;

    private int spellSwitch = 0;
    
    void Update()
    {
        if (rightTriggerReference.action.triggered && !aimimg)
        {
            if (spellSwitch % 2 == 0) toCast = fireball;
            else toCast = elecball;
            castProjectileSpell();
            aimimg = true;
            spellSwitch++;
        } else if (aimimg)
        {
            aimimg = !castedSpell.GetComponent<ProjectileSpell>().Launched;
        }
    }

    private void castProjectileSpell()
    {
        castedSpell = Instantiate(toCast, rightHandTransform.localPosition, rightHandTransform.localRotation);
    }
}
