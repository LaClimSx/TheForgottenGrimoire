using System.Collections.Generic;
using UnityEngine;

public class Managerlvl1candles2 : MonoBehaviour
{
    private const int NUMBER_OF_LIGHTS = 8;
    private InteractableCandles[] lights;

    void Start()
    {
        // Get all child InteractableCandles only once
        lights = GetComponentsInChildren<InteractableCandles>();

        // for (int i = 0; i < lights.Length; i++)
        // {
        //     print($"added light {i} : {lights[i]}");
        // }

        // print("lights: " + lights);
        // for (int i = 0; i < lights.Length; i++)
        // {
        //     print(lights[i]);
        //     lights[i].BlowOutCandle();
        // }
        print("end of start, lights length = " + lights.Length);
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
            print($"reset light {i}");
        }
    }
}
