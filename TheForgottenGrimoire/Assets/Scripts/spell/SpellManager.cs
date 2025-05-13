using Spells;
using static Spells.SpellType;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Casters;
using System.Collections;

namespace Spells
{
    public enum SpellState
    {
        Pending,
        Drawing,
        Casting
    }
    public enum SpellType
    {
        Telekinesis, // Far grab
        Blinkstep, // Far teleport
        Hub, // Teleport to hub
        Fireball,
        FlameJet, //Flame thrower
        Emberlight, //Light that stays with you
        ChargeShot, //Far zap
        ArcHands, //Palpatine
        Earthball, //Climbable rock projectile
        Cube, //Earth companion cube
        HandJet, //Iron Man
        WindColumn, //Wind push
        NoSpell
    }
    public enum SpellShape
    {
        Lightning,
        Spiral,
        Square,
        Infinity,
        Triangle,
        NoShape
    }
}
public class SpellManager : MonoBehaviour
{
    [SerializeField] private InputActionReference rightTriggerReference;
    [SerializeField] private GameObject staff;
    [SerializeField] private Transform leftHand;
    [SerializeField] private int maxLiveCube = 4;
    [SerializeField] private Transform projectileSpawnPoint;

    //Teleportation
    [SerializeField] private const float smallDistanceTP = 5f;
    [SerializeField] private const float largeDistanceTP = 10f;
    [SerializeField] private XRRayInteractor xrRayInteractor;

    //Grab
    [SerializeField] private const float smallDistanceGrab = 5f;
    [SerializeField] private const float largeDistanceGrab = 15f;
    [SerializeField] private CurveInteractionCaster curveInteractionCasterLeft;
    [SerializeField] private CurveInteractionCaster curveInteractionCasterRight;
    [SerializeField] private float spaceSpellDuration = 7f;

    //Spells
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject elecball;
    [SerializeField] private GameObject flameThrower;
    [SerializeField] private GameObject cubeCompanion;

    private SpellState _spellState = SpellState.Pending;
    private GameObject toCast;
    private GameObject castedSpell;
    private List<GameObject> liveCubes = new List<GameObject>();
    
    private bool triggerDown = false;

    private Dictionary<SpellType, bool> unlockedSpells = new Dictionary<SpellType, bool>();

    public SpellState SpellState
    {
        get => _spellState;
        set
        {
            _spellState = value;
            if (_spellState == SpellState.Casting && _currentSpellType != SpellType.NoSpell)
            {
                CastSpell(_currentSpellType);
            }
        }
    }

    private SpellType _currentSpellType = SpellType.NoSpell;

    public SpellType CurrentSpellType
    {
        get => _currentSpellType;
        set => _currentSpellType = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitDict(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_spellState == SpellState.Casting)
        {
            if (toCast.GetComponent<ProjectileSpell>() != null && castedSpell == null && !triggerDown)
            {
                print("Instantiating " + toCast.name);
                castedSpell = Instantiate(toCast, projectileSpawnPoint.localToWorldMatrix.MultiplyPoint3x4(projectileSpawnPoint.localPosition), projectileSpawnPoint.localRotation, projectileSpawnPoint);
            }
            else if (rightTriggerReference.action.triggered && toCast.GetComponent<ProjectileSpell>() != null && castedSpell != null && !triggerDown)
            {
                print("Launching balls");
                Destroy(castedSpell);
                GameObject projectile = Instantiate(toCast, projectileSpawnPoint.localToWorldMatrix.MultiplyPoint3x4(projectileSpawnPoint.localPosition), Quaternion.identity);
                projectile.GetComponent<ProjectileSpell>().launch(projectileSpawnPoint.forward);
                castedSpell = null;
                _spellState = SpellState.Pending;
            }
            else if (rightTriggerReference.action.triggered && toCast == cubeCompanion)
            {
                if (liveCubes.Count >= maxLiveCube)
                {
                    GameObject tokill = liveCubes[0];
                    liveCubes.RemoveAt(0);
                    Destroy(tokill);
                }
                liveCubes.Add(Instantiate(toCast, projectileSpawnPoint.localToWorldMatrix.MultiplyPoint3x4(projectileSpawnPoint.localPosition), Quaternion.identity));
                _spellState = SpellState.Pending;
            }
            else if (rightTriggerReference.action.ReadValue<float>() > 0.5 && !triggerDown)
            {
                triggerDown = true;
                if (toCast == flameThrower)
                {
                    print("Instantiating " + toCast.name);
                    castedSpell = Instantiate(toCast, leftHand);
                }
            }
            else if (triggerDown && rightTriggerReference.action.ReadValue<float>() <= 0.5)
            {
                triggerDown = false;
                Destroy(castedSpell);
                castedSpell = null;
                _spellState = SpellState.Pending;
            }
        } 
        else
        {
            Destroy(castedSpell);
            castedSpell = null;
            _spellState = SpellState.Pending;
        }
        //if (!staff.GetComponent<StaffGrabableScript>().InHand)
        //{
        //    Destroy(castedSpell);
        //    castedSpell = null;
        //    _spellState = SpellState.Pending;
        //}
    }

    void  CastSpell(SpellType spellType)
    {
        //TODO: stuff
        print("casting :" + spellType);
        switch (spellType)
        {
            case Telekinesis:
                SetGrab(largeDistanceGrab);
                StartCoroutine(ResetGrab(smallDistanceGrab, spaceSpellDuration));
                _spellState = SpellState.Pending;
                break;
            case Blinkstep:
                SetTP(largeDistanceTP);
                StartCoroutine(ResetTP(smallDistanceTP, spaceSpellDuration));
                _spellState = SpellState.Pending;
                break;
            case Hub:
                _spellState = SpellState.Pending;
                break;
            case Fireball:
                toCast = fireball;
                break;
            case FlameJet:
                toCast = flameThrower;
                break;
            case Emberlight:
                _spellState = SpellState.Pending;
                break;
            case ChargeShot:
                toCast = elecball;
                break;
            case ArcHands:
                _spellState = SpellState.Pending;
                break;
            case Earthball:
                _spellState = SpellState.Pending;
                break;
            case Cube:
                toCast = cubeCompanion;
                break;
            case HandJet:
                _spellState = SpellState.Pending;
                break;
            case WindColumn:
                _spellState = SpellState.Pending;
                break;
            default:
                _spellState = SpellState.Pending;
                break;
        }
    }

    public void RemoveCompanionCube(GameObject cube)
    {
        if (liveCubes.Contains(cube)) liveCubes.Remove(cube);
        print("live cubes " + liveCubes);
    }

    private void SetTP(float distance)
    {
        if (xrRayInteractor)
        {
            xrRayInteractor.velocity = distance;
            print("Setting teleport distance to " + distance);
        }
        else
        {
            Debug.LogError("XRRayInteractor is not assigned.");
        }
    }

    private IEnumerator ResetTP(float distance = smallDistanceTP, float delay = 7f)
    {
        yield return new WaitForSeconds(delay);
        SetTP(distance);
    }


    private void SetGrab(float distance)
    {
        if (curveInteractionCasterLeft && curveInteractionCasterRight)
        {
            curveInteractionCasterLeft.castDistance = distance;
            curveInteractionCasterRight.castDistance = distance;
            print("Setting grab distance to " + distance);
        }
        else
        {
            Debug.LogError("CurveInteractionCasters are not assigned.");
        }
    }

    private IEnumerator ResetGrab(float distance = smallDistanceGrab, float delay = 7f)
    {
        yield return new WaitForSeconds(delay);
        SetGrab(distance);
    }

    private void InitDict(bool devCheat = false) //Remove the devCheat argument for release
    {
        foreach (SpellType spelltype in System.Enum.GetValues(typeof(SpellType)))
        {
            if (spelltype == SpellType.NoSpell)
            {
                continue;
            }
            unlockedSpells.Add(spelltype, devCheat);
        }
    }

    public void UnlockSpell(SpellType spellType)
    {
        if (unlockedSpells.ContainsKey(spellType))
        {
            unlockedSpells[spellType] = true;
        }
        else
        {
            Debug.LogError("Spell type not found in dictionary: " + spellType);
        }
    }

    public bool IsSpellUnlocked(SpellType spellType)
    {
        if (unlockedSpells.ContainsKey(spellType))
        {
            return unlockedSpells[spellType];
        }
        else
        {
            Debug.LogError("Spell type not found in dictionary: " + spellType);
            return false;
        }
    }
}
