using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileSpell : MonoBehaviour
{
    [SerializeField] float initialSpeed;
    private Rigidbody rb;

    public bool Launched { private set; get; }    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Launched = false;
        GetComponent<Collider>().enabled = false;
    }

    public void launch(Vector3 aim)
    {
        
        rb.isKinematic = false;
        rb.AddForce(aim.normalized * initialSpeed);
        Launched = true;
        GetComponent<Collider>().enabled = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
