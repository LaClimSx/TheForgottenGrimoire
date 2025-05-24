using UnityEngine;

public class BoxValid : MonoBehaviour
{
    [SerializeField] private GameObject teleportHub;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "exitCube") teleportHub.SetActive(true);
    }
}
