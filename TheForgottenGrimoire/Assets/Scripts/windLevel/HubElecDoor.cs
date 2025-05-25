using UnityEngine;

public class HubElecDoor : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<InteractorElec>() != null) gameObject.SetActive(false);
    }
}
