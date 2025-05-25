using UnityEngine;

public class InteractableCandles : MonoBehaviour
{

    private Light light;
    [SerializeField] private bool lightable;
    [SerializeField] private bool original_light;
    // private int test;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light = transform.parent.GetComponentInChildren<Light>();
        light.enabled = original_light;
    }


    void OnTriggerEnter(Collider other)
    {
        print("bonked");
        InteractorElement interactor = other.gameObject.GetComponent<InteractorFire>();
        if (interactor != null && interactor.enabled && lightable)
        {
            print($"bonked interactor {interactor.Type}");
            light.enabled = /*!light.enabled;*/ true;
        }
    }

    public void BlowOutCandle()
    {
        print("blow out candle");
        print(light);
        if (lightable && light != null)
        {
            light.enabled = false;
        }
    }

    public bool IsLit()
    {
        return light != null && light.enabled;
    }

    public void makeLightable()
    {
        lightable = true;
    }
}
