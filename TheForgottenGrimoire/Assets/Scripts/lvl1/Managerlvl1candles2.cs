using System.Linq;
using Spells;
using UnityEngine;

public class Managerlvl1candles2 : MonoBehaviour
{
    // private const int NUMBER_OF_LIGHTS = 8;
    private InteractableCandles[] lights;

    [SerializeField] private InteractableCandles solutionCandle1;
    [SerializeField] private InteractableCandles solutionCandle2;
    [SerializeField] private InteractableCandles solutionCandle3;

    [SerializeField] private DoorManager door;
    [SerializeField] private SpellManager spellManager;
    private bool hasWon = false;
    // private bool allSpellsUnlocked = false;

    // 2nd part of the puzzle (unlock flamethrower)
    [SerializeField] private InteractableCandles extraCandle1;
    [SerializeField] private InteractableCandles extraCandle2;
    [SerializeField] private InteractableCandles extraCandle3;
    [SerializeField] private InteractableCandles extraCandle4;
    [SerializeField] private InteractableCandles extraCandle5;
    private InteractableCandles[] extraLights;
    private bool initExtra = false;
    private bool hasWon2 = false;

    void Start()
    {
        // Get 2nd part children InteractableCandles 
        lights = GetComponentsInChildren<InteractableCandles>();
        extraLights = new[] { extraCandle1, extraCandle2, extraCandle3, extraCandle4, extraCandle5 };
        
    }

    void Update()
    {
        if (hasWon2) return;
        if (hasWon)
        {
            // if (!allSpellsUnlocked)
            // {
            //     allSpellsUnlocked = true;
            //     UnlockAllSpells();
            // }
            if (!initExtra)
            {
                foreach (var item in extraLights)
                {
                    item.makeLightable();
                }
                initExtra = true;
            }
            else if (extraLights.Where(c => c.IsLit()).Count() == extraLights.Length)
            {
                print("has won 2");
                hasWon2 = true;
                spellManager.UnlockSpell(SpellType.FlameJet);
            }

            return;
        }
        if (lights == null || lights.Length == 0)
        {
            print("update null check");
            return;
        }

        // Get all candles that are currently lit
        InteractableCandles[] litCandles = lights.Where(c => c.IsLit()).ToArray();

        // Check if exactly 3 candles are lit
        if (litCandles.Length == 3)
        {
            // Check if they are the 3 solution candles (order doesn't matter)
            bool hasAllSolutionCandles =
                litCandles.Contains(solutionCandle1) &&
                litCandles.Contains(solutionCandle2) &&
                litCandles.Contains(solutionCandle3);

            if (hasAllSolutionCandles)
            {
                Debug.Log("Victory! Only the correct 3 candles are lit.");
                hasWon = true;
                door.OpenDoor();
            }
            else
            {
                print("nope");
            }
        }
    }

    public void ResetPuzzle()
    {
        // Add null check to avoid crash if lights is null
        if (lights == null || lights.Length == 0)
        {
            print("null check");
            lights = GetComponentsInChildren<InteractableCandles>();
            if (lights == null || lights.Length == 0)
            {
                Debug.LogWarning("ResetPuzzle failed: no candles found.");
                return;
            }
        }

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].BlowOutCandle();
        }
    }

    public void unlockFireSpell()
    {
        print("unlocking fireball");
        spellManager.UnlockSpell(SpellType.Fireball);
    }

    public void UnlockAllSpells()
    {
        foreach (SpellType spelltype in System.Enum.GetValues(typeof(SpellType)))
        {
            if (spelltype != SpellType.NoSpell)
            {
                spellManager.UnlockSpell(spelltype);
            }
        }
    }

}
