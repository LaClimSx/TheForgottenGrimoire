using UnityEngine;
using UnityEngine.InputSystem;

public class DummyManager : MonoBehaviour
{
    [SerializeField] private InputActionReference rightTriggerReference;
    [SerializeField] private InputActionReference leftButtonXPressed;
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject elecball;
    [SerializeField] private GameObject flameThrower;
    [SerializeField] private GameObject staff;

    private GameObject toCast;
    private GameObject toLaunch;
    private GameObject castedSpell;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Transform leftHand;
    private bool inCast = false;

    private int spellSwitch = 0;

    void Update()
    {
        if (leftButtonXPressed.action.triggered)
        {
            spellSwitch = ++spellSwitch % 3;
            switch (spellSwitch)
            {
                case 0:
                    toCast = fireball;
                    break;
                case 1:
                    toCast = elecball;
                    break;
                case 2:
                    toCast = flameThrower;
                    break;
                default:
                    break;
            }
            print($"To cast: {toCast.name}");
        }
        if (staff.GetComponent<StaffGrabableScript>().InHand)
        {
            if (rightTriggerReference.action.ReadValue<float>() > 0.5f && toCast == flameThrower && !inCast)
            {
                print("flamme throw should be trigger");
                castedSpell = Instantiate(toCast, leftHand);
                inCast = true;
            }
            else if (inCast && rightTriggerReference.action.ReadValue<float>() <= 0.5f)
            {
                print("flamme throw should be stopped");
                inCast = false;
                Destroy(castedSpell);
                castedSpell = null;
            }
            else if (rightTriggerReference.action.triggered && toCast != null && castedSpell == null)
            {
                print("cast projectile");
                castProjectileSpell();
            }
            else if (castedSpell != null && rightTriggerReference.action.triggered)
            {
                print("launch projectile");
                launchCastedProjectileSpell();
                castedSpell = null;
            } 

        } else if (castedSpell != null)
        {
            Destroy(castedSpell);
            castedSpell = null;
        }        
    }

    private void launchCastedProjectileSpell()
    {
        Destroy(castedSpell);
        GameObject projectile = Instantiate(toLaunch, projectileSpawnPoint.localToWorldMatrix.MultiplyPoint3x4(projectileSpawnPoint.localPosition), Quaternion.identity);
        projectile.GetComponent<ProjectileSpell>().launch(projectileSpawnPoint.forward);
    }

    private void castProjectileSpell()
    {
        castedSpell = Instantiate(toCast, projectileSpawnPoint.localToWorldMatrix.MultiplyPoint3x4(projectileSpawnPoint.localPosition), projectileSpawnPoint.localRotation, projectileSpawnPoint);
        toLaunch = toCast;
    }
}
