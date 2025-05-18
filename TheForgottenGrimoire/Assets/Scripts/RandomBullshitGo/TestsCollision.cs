using UnityEngine;

public class TestsCollision : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;
        GetComponent<Rigidbody>().linearVelocity = transform.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Collision");
        Destroy(gameObject);
        Vector3 contact = collision.GetContact(0).point;
        GameObject arrow1 = Instantiate(arrow, contact, Quaternion.identity);
        arrow1.transform.up = (initialPos - contact).normalized;        
    }
}
