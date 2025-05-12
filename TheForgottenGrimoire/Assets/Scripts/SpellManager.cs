using System.Collections.Generic;
using Spells;
using UnityEngine;
using static Spells.SpellType;

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
    private SpellState _spellState = SpellState.Pending;
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

    private Dictionary<SpellType, bool> unlockedSpells = new Dictionary<SpellType, bool>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitDict(true);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void  CastSpell(SpellType spellType)
    {
        //TODO: stuff
        print("casting :" + spellType);
        _spellState = SpellState.Pending;
    }

    void InitDict(bool devCheat = false) //Remove the devCheat argument for release
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

    public void unlockSpell(SpellType spellType)
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
