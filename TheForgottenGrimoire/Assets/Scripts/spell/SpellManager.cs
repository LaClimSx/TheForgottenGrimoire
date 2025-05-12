using Spells;
using static Spells.SpellType;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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
        Triangle
    }
}
public class SpellManager : MonoBehaviour
{
    [SerializeField] private InputActionReference rightTriggerReference;
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject elecball;
    [SerializeField] private GameObject flameThrower;
    [SerializeField] private GameObject cubeCompanion;
    [SerializeField] private GameObject staff;

    private SpellState _spellState = SpellState.Pending;
    private GameObject toCast;
    private GameObject castedSpell;
    private List<GameObject> liveCubes = new List<GameObject>();
    [SerializeField] private int maxLiveCube = 4;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Transform leftHand;
    private bool triggerDown = false;

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
            case Fireball:
                toCast = fireball;
                break;
            case Earthball:
                _spellState = SpellState.Pending;
                break;
            case ChargeShot:
                toCast = elecball;
                break;
            case WindColumn:
                _spellState = SpellState.Pending;
                break;
            case Cube:
                toCast = cubeCompanion;
                break;
            case Hub:
                _spellState = SpellState.Pending;
                break;
            case Telekinesis:
                _spellState = SpellState.Pending;
                break;
            case Blinkstep:
                _spellState = SpellState.Pending;
                break;
            case FlameJet:
                toCast = flameThrower;
                break;
            case Emberlight:
                _spellState = SpellState.Pending;
                break;
            case ArcHands:
                _spellState = SpellState.Pending;
                break;
            case HandJet:
                _spellState = SpellState.Pending;
                break;
            default:
                _spellState = SpellState.Pending;
                break;
        }
    }

    public void removeCompanionCube(GameObject cube)
    {
        if (liveCubes.Contains(cube)) liveCubes.Remove(cube);
        print("live cubes " + liveCubes);
    }
}
