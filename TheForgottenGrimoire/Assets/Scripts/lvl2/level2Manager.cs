using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class level2Manager : MonoBehaviour
{
    [SerializeField] private SlidingDoor slidingDoor1;
    [SerializeField] private SlidingDoor slidingDoor2;
    [SerializeField] private GameObject fan;
    private Animation fanAnim;
    private AudioSource fanAudio;

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

    private bool hasWon1 = false;
    private bool hasWon2 = false;


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
        if (hasWon2) return;
        if (hasWon1)
        {
            // check puzzle 2 
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
        fanAnim.enabled = true;
        fanAudio.enabled = true;
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
