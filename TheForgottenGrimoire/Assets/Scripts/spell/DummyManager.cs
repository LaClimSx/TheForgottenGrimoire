using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DummyManager : MonoBehaviour
{
    [SerializeField] private InputActionReference rightTriggerReference;
    [SerializeField] private InputActionReference leftButtonXPressed;
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject elecball;
    [SerializeField] private GameObject flameThrower;
    [SerializeField] private GameObject cubeCompanion;
    [SerializeField] private GameObject staff;

    private GameObject toCast;
    private GameObject toLaunch;
    private GameObject castedSpell;
    private List<GameObject> liveCubes = new List<GameObject>();
    [SerializeField] private int maxLiveCube = 4;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Transform leftHand;
    private bool inCast = false;

    private int spellSwitch = 3;

    void Update()
    {
        if (leftButtonXPressed.action.triggered)
        {
            spellSwitch = ++spellSwitch % 4;
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
                case 3:
                    toCast = cubeCompanion;
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
                castedSpell = Instantiate(toCast, leftHand);
                inCast = true;
            }
            else if (toCast == cubeCompanion && rightTriggerReference.action.triggered && castedSpell == null)
            {
                if (liveCubes.Count >= maxLiveCube)
                {
                    GameObject tokill = liveCubes[0];
                    liveCubes.RemoveAt(0);
                    Destroy(tokill);
                }
                liveCubes.Add(Instantiate(toCast, projectileSpawnPoint.localToWorldMatrix.MultiplyPoint3x4(projectileSpawnPoint.localPosition), Quaternion.identity));
            }
            else if (inCast && rightTriggerReference.action.ReadValue<float>() <= 0.5f)
            {
                inCast = false;
                Destroy(castedSpell);
                castedSpell = null;
            }
            else if (rightTriggerReference.action.triggered && toCast != null && castedSpell == null)
            {
                castProjectileSpell();
            }
            else if (castedSpell != null && rightTriggerReference.action.triggered)
            {
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

    public void removeCompanionCube(GameObject cube)
    {
        if (liveCubes.Contains(cube)) liveCubes.Remove(cube);
        print("live cubes " + liveCubes);
    }
}
