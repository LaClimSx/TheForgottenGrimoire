using UnityEngine;

public class InteractableCandles : MonoBehaviour
{

    private Light light;
    private int test;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light = transform.parent.GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (++test % 500 == 0) {
            light.enabled = !light.enabled;
        }
    }
}
