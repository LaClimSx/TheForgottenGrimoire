using System.Collections.Generic;
using System.Linq;
using Spells;
using UnityEngine;

public class level2Manager : MonoBehaviour
{
    [SerializeField] private SlidingDoor slidingDoor1;
    [SerializeField] private SlidingDoor slidingDoor2;
    [SerializeField] private GameObject fan;
    private int waitForZap = 200;
    [SerializeField] private AudioSource elecAudio;
    private Animation fanAnim;
    private AudioSource fanAudio;

    [SerializeField] private Lightable lightable;

    [SerializeField] private Light light1;
    [SerializeField] private Light light2;
    [SerializeField] private Light light3;
    [SerializeField] private Light light4;
    private List<Light> lightSequence;
    private int currentLight;

    [SerializeField] private InteractableCandles candle1;
    [SerializeField] private InteractableCandles candle2;
    [SerializeField] private InteractableCandles candle3;
    [SerializeField] private InteractableCandles candle4;
    private List<InteractableCandles> winningSequence;
    private List<InteractableCandles> currentSequence;

    private bool hasWon1 = true; // to debug
    private bool hasWon2 = false;
    private bool done = false;

    [SerializeField] private SpellManager spellManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fanAnim = fan.GetComponent<Animation>();
        fanAudio = fan.GetComponent<AudioSource>();
        lightSequence = new List<Light> { light1, light3, light2, light4 };
        winningSequence = new List<InteractableCandles> { candle1, candle3, candle2, candle4 };
        currentSequence = new List<InteractableCandles> { };
        currentLight = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (done) return;
        if (waitForZap == 0)
        {
            elecAudio.enabled = true;
            done = true;
            return;
        }
        if (hasWon2)
        {
            waitForZap = waitForZap - 1;
            return;
        }
        if (hasWon1)
        {
            // check puzzle 2 
            if (lightable.done)
            {
                turnOnCurrent();
            }
        }
        else
        {
            // check puzzle 1
            if (Time.frameCount % 500 == 0)
            {
                currentSequence.ForEach(print);
                updateLightSequence();
            }
            if (currentSequence.Count == winningSequence.Count)
            {
                if (currentSequence.SequenceEqual(winningSequence))
                {
                    hasWon1 = true;
                    openDoors();
                }
                else
                {
                    // sequence in wrong order, reset puzzle
                    ResetPuzzle1();
                }
            }
            else
            {
                foreach (var candle in winningSequence)
                {
                    if (candle.IsLit() && !currentSequence.Contains(candle))
                    {
                        currentSequence.Add(candle);
                    }
                }
            }

        }
        
    }

    // win puzzle1
    void openDoors()
    {
        slidingDoor1.ToggleDoorOpen();
        slidingDoor2.ToggleDoorOpen();
    }

    // win puzzle2
    public void turnOnCurrent()
    {
        hasWon2 = true;
        fanAnim.enabled = true;
        fanAudio.enabled = true;
        spellManager.UnlockSpell(SpellType.ChargeShot);
        spellManager.UnlockSpell(SpellType.ArcHands);
    }

    private void ResetPuzzle1()
    {
        currentSequence = new List<InteractableCandles>();
        for (int i = 0; i < winningSequence.Count; i++)
        {
            winningSequence[i].BlowOutCandle();
        }
    }

    // puzzle1
    private void updateLightSequence()
    {
        if (currentLight != lightSequence.Count)
        {
            lightSequence[currentLight].color = Color.white;
            lightSequence[currentLight].intensity = 0.2f;
        }
        print("update light sequence, current = " + currentLight);
        
        currentLight = (currentLight + 1) % (lightSequence.Count + 1);

        if (currentLight != lightSequence.Count)
        {
            lightSequence[currentLight].color = Color.green;
            lightSequence[currentLight].intensity = 1.5f;
        }
        
    }
}
