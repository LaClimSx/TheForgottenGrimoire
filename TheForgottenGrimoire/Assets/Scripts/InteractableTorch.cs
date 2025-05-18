using UnityEngine;

public class InteractableTorch : MonoBehaviour
{
    private Light light;
    private bool is_alighten;
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
        if (interactor != null & !is_alighten)
        {
            print($"bonked interactor {interactor.Type}");
            light.enabled = true;
            is_alighten = true;
        }
    }
}
