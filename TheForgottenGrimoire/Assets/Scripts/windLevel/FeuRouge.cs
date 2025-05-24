using UnityEngine;

public class FeuRouge : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject light;
    [SerializeField] private Material red;
    [SerializeField] private Material green;
    [SerializeField] private InteractableConductor elecSource;
    private bool doorOpen = false;

    // Update is called once per frame
    void Update()
    {
        if (!doorOpen && elecSource.Power <= 0)
        {
            doorOpen = true;
            door.SetActive(false);
            light.GetComponent<Renderer>().material = green;
        }
        else if (doorOpen && elecSource.Power > 0)
        {
            doorOpen = false;
            door.SetActive(true);
            light.GetComponent<Renderer>().material = red;
        }
    }
}
