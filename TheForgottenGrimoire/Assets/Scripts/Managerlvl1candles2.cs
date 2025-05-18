using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Managerlvl1candles2 : MonoBehaviour
{
    // private const int NUMBER_OF_LIGHTS = 8;
    private InteractableCandles[] lights;

    [SerializeField] private InteractableCandles solutionCandle1;
    [SerializeField] private InteractableCandles solutionCandle2;
    [SerializeField] private InteractableCandles solutionCandle3;

    void Start()
    {
        // Get all child InteractableCandles only once
        lights = GetComponentsInChildren<InteractableCandles>();

        print("end of start, lights length = " + lights.Length);
    }

    void Update()
    {
        if (Time.frameCount % 100 != 0) return;
        if (lights == null || lights.Length == 0)
        {
            print("update null check");
            return;
        }

        // foreach (var item in lights)
        // {
        //     print("light : " + item + " is lit ? " + item.IsLit());
        // }   
        

        // Get all candles that are currently lit
        InteractableCandles[] litCandles = lights.Where(c => c.IsLit()).ToArray();

        // print("Lit candles count: " + litCandles.Length);
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

        print("reset puzzle called, lights length = " + lights.Length);
        for (int i = 0; i < lights.Length; i++)
        {
            print(lights[i]);
            lights[i].BlowOutCandle();
            // print($"reset light {i}");
        }
    }


}
