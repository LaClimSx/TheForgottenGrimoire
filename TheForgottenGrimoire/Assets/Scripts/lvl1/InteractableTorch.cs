using UnityEngine;

public class InteractableTorch : MonoBehaviour
{
    private Light light;
    private bool is_alighten;
    [SerializeField] private bool original_light;
    private InteractorFire interactorFire;
    // private int test;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light = transform.parent.GetComponentInChildren<Light>();
        light.enabled = original_light;
        interactorFire = transform.parent.GetComponentInChildren<InteractorFire>();
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
            interactorFire.enabled = true;
        }
    }
}
