using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform dest;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = dest.position;
        }
        else if (other.CompareTag("magicStaff")) 
        {
            if (other.transform.parent.GetComponent<StaffGrabableScript>().InHand) { return; }
        }
        else if (other.CompareTag("grimoire")) { return; }
        else
        {
            other.transform.position = dest.position;
            if (other.GetComponent<Rigidbody>() != null) other.GetComponent<Rigidbody>().AddForce(other.transform.forward.normalized / 10f, ForceMode.Impulse);
        }
    }
}
