using UnityEngine;

public class openDoor : MonoBehaviour
{
    [SerializeField] private GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "key") door.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "key") door.SetActive(true); 
    }
}
