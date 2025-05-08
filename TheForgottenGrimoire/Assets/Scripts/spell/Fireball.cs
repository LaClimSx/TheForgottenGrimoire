using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Fireball : MonoBehaviour
{
    [SerializeField] float initialSpeed;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void launch(Vector3 aim)
    {
        aim = aim.normalized;
        rb.AddRelativeForce(aim * initialSpeed);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(this);
    }
}
