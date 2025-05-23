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
        CastSuccessfull,
        Animation,
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
    [SerializeField] private Transform rightHand;
    [SerializeField] private int maxLiveCube = 4;
    [SerializeField] private Transform projectileSpawnPoint;

    //Teleportation
    [SerializeField] private Transform hubTransform;
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
    [SerializeField] private GameObject earthball;
    [SerializeField] private GameObject flameThrower;
    [SerializeField] private GameObject cubeCompanion;
    [SerializeField] private GameObject handJets;
    [SerializeField] private GameObject palpatine;
    [SerializeField] private GameObject windColumn;

    //Cast successfull animation
    [SerializeField] private GameObject fireSuccess;
    [SerializeField] private GameObject elecSuccess;
    [SerializeField] private GameObject earthSuccess;
    [SerializeField] private GameObject windSuccess;
    [SerializeField] private GameObject spaceSuccess;
    [SerializeField] private float minAnimationDuration;
    private GameObject currentAnimation;
    private float actualCastTime;

    private SpellState _spellState = SpellState.Pending;
    private GameObject toCast;
    private GameObject castedSpell;
    private List<GameObject> liveCubes = new List<GameObject>();
    
    private bool spellInUse = false;

    private Dictionary<SpellType, bool> unlockedSpells = new Dictionary<SpellType, bool>();

    public SpellState SpellState
    {
        get => _spellState;
        set
        {
            //if (_spellState == SpellState.Animation) print($"Switching from animation to {value}");
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
        InitDict(true); // true to unlock every spells at the beginning -- debug purposes
    }

    private Vector3 getProjectilSpawnPointInWorldCoord()
    {
        return projectileSpawnPoint.localToWorldMatrix.MultiplyPoint3x4(projectileSpawnPoint.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (actualCastTime < Time.time && SpellState == SpellState.Animation)
        {
            SpellState = SpellState.Casting;
        }
        else if (SpellState == SpellState.CastSuccessfull)
        {
            actualCastTime = Time.time + minAnimationDuration;
            SpellState = SpellState.Animation;            
            switch (_currentSpellType)
            {                
                case SpellType.ArcHands:
                    currentAnimation = Instantiate(elecSuccess, leftHand);
                    break;
                case SpellType.Blinkstep:
                //    currentAnimation = Instantiate(spaceSuccess, rightHand);
                    break;
                case SpellType.ChargeShot:
                    currentAnimation = Instantiate(elecSuccess, getProjectilSpawnPointInWorldCoord(), Quaternion.identity, projectileSpawnPoint);
                    break;
                case SpellType.Cube:
                    currentAnimation = Instantiate(earthSuccess, getProjectilSpawnPointInWorldCoord(), Quaternion.identity, projectileSpawnPoint);
                    break;
                case SpellType.Earthball:
                    currentAnimation = Instantiate(earthSuccess, getProjectilSpawnPointInWorldCoord(), Quaternion.identity, projectileSpawnPoint);
                    break;
                case SpellType.Emberlight:
                    currentAnimation = Instantiate(fireSuccess, getProjectilSpawnPointInWorldCoord(), Quaternion.identity, projectileSpawnPoint);
                    break;
                case SpellType.Fireball:
                    currentAnimation = Instantiate(fireSuccess, getProjectilSpawnPointInWorldCoord(), Quaternion.identity, projectileSpawnPoint);
                    break;
                case SpellType.FlameJet:
                    currentAnimation = Instantiate(fireSuccess, leftHand);
                    break;
                case SpellType.HandJet:
                    currentAnimation = Instantiate(windSuccess, leftHand);
                    break;
                case SpellType.Hub:
                //    currentAnimation = Instantiate(spaceSuccess, GameObject.FindGameObjectWithTag("Player").transform);
                    break;
                case SpellType.Telekinesis:
                    //currentAnimation = Instantiate(spaceSuccess, rightHand);
                    break;
                case SpellType.WindColumn:
                    currentAnimation = Instantiate(windSuccess, getProjectilSpawnPointInWorldCoord(), Quaternion.identity, projectileSpawnPoint);
                    break;
                case SpellType.NoSpell:                    
                    break;
            }
        }
        else if (SpellState == SpellState.Casting)
        {
            if (toCast.GetComponent<ProjectileSpell>() != null && castedSpell == null && !spellInUse)
            {
                print("Instantiating " + toCast.name);
                Destroy(currentAnimation);
                castedSpell = Instantiate(toCast, getProjectilSpawnPointInWorldCoord(), projectileSpawnPoint.rotation, projectileSpawnPoint);
            }
            else if (rightTriggerReference.action.triggered && toCast.GetComponent<ProjectileSpell>() != null && castedSpell != null && !spellInUse)
            {
                print("Launching balls");
                Destroy(castedSpell);
                GameObject projectile = Instantiate(toCast, getProjectilSpawnPointInWorldCoord(), Quaternion.identity);
                projectile.GetComponent<ProjectileSpell>().launch(projectileSpawnPoint.forward);
                castedSpell = null;
                SpellState = SpellState.Pending;
            }
            else if (rightTriggerReference.action.triggered && toCast == cubeCompanion)
            {
                if (liveCubes.Count >= maxLiveCube)
                {
                    GameObject tokill = liveCubes[0];
                    liveCubes.RemoveAt(0);
                    Destroy(tokill);
                }
                Destroy(currentAnimation);
                liveCubes.Add(Instantiate(toCast, getProjectilSpawnPointInWorldCoord(), Quaternion.identity));
                SpellState = SpellState.Pending;
            }
            else if (toCast == handJets)
            {               
                if (!spellInUse)
                {
                    print("spawn handjets");
                    Destroy(currentAnimation);
                    castedSpell = Instantiate(toCast, getProjectilSpawnPointInWorldCoord(), Quaternion.identity);
                    spellInUse = true;
                } 
                else if (rightTriggerReference.action.triggered && !castedSpell.GetComponent<HandJets>().WaitingForCollisions && !castedSpell.GetComponent<HandJets>().ProjectileLaunched)
                {
                    castedSpell.GetComponent<HandJets>().launchProjectiles();
                } 
                else if (castedSpell.GetComponent<HandJets>().FlightComplete || castedSpell.GetComponent<HandJets>().CollisionOutOfRange)
                {
                    print("destroy handjets");
                    Destroy(castedSpell);
                    castedSpell = null;
                    spellInUse = false;
                    SpellState = SpellState.Pending;
                }
            }
            else if (rightTriggerReference.action.ReadValue<float>() > 0.5 && !spellInUse)
            {
                Destroy(currentAnimation);
                spellInUse = true;
                if (toCast == flameThrower)
                {
                    castedSpell = Instantiate(toCast, leftHand);
                }
                else if (toCast == palpatine)
                {
                    castedSpell = Instantiate(toCast, leftHand.position, leftHand.rotation, leftHand);
                }
                else if (toCast == windColumn)
                {
                    castedSpell= Instantiate(toCast, getProjectilSpawnPointInWorldCoord(), projectileSpawnPoint.rotation, projectileSpawnPoint);
                }
            }
            else if (spellInUse && rightTriggerReference.action.ReadValue<float>() <= 0.5)
            {
                spellInUse = false;
                Destroy(castedSpell);
                castedSpell = null;
                SpellState = SpellState.Pending;
            }
        }
        
        //if ()
        //{
        //    Destroy(castedSpell);
        //    castedSpell = null;
        //    SpellState = SpellState.Pending;
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
                SpellState = SpellState.Pending;
                StartCoroutine(spaceAnimation(spaceSpellDuration));
                StartCoroutine(ResetGrab(smallDistanceGrab, spaceSpellDuration));                
                break;
            case Blinkstep:
                SetTP(largeDistanceTP);
                SpellState = SpellState.Pending;
                StartCoroutine(spaceAnimation(spaceSpellDuration));
                StartCoroutine(ResetTP(smallDistanceTP, spaceSpellDuration));
                break;
            case Hub:
                StartCoroutine(spaceAnimation(minAnimationDuration));
                StartCoroutine(tpHub(minAnimationDuration));
                SpellState = SpellState.Pending;
                break;
            case Fireball:
                toCast = fireball;
                break;
            case FlameJet:
                toCast = flameThrower;
                break;
            case Emberlight:
                SpellState = SpellState.Pending;
                break;
            case ChargeShot:
                toCast = elecball;
                break;
            case ArcHands:
                toCast = palpatine;                
                break;
            case Earthball:
                toCast = earthball;
                break;
            case Cube:
                toCast = cubeCompanion;
                break;
            case HandJet:
                toCast = handJets;
                break;
            case WindColumn:
                toCast = windColumn;
                break;
            default:
                SpellState = SpellState.Pending;
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

    private IEnumerator spaceAnimation(float duration)
    {        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 spawnPoint = player.transform.position;
        spawnPoint.y = 0f;
        print($"Regarde jose le point de spawn il est la {spawnPoint} sur la transform {player.name}");
        currentAnimation = Instantiate(spaceSuccess, spawnPoint, Quaternion.identity, player.transform);
        yield return new WaitForSeconds(duration);
        Destroy(currentAnimation);
    }

    private IEnumerator tpHub(float duration)
    {
        yield return new WaitForSeconds(duration);
        GameObject.FindWithTag("Player").transform.position = hubTransform.position;
    }
}
