using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Fireball : MonoBehaviour
{
    [SerializeField] float initialSpeed;
    private Rigidbody rb;
    private bool launched;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        launched = false;
        transform.localPosition += new Vector3(0, 0, 1);
    }

    public void launch(Vector3 aim)
    {
        aim = aim.normalized;
        rb.AddRelativeForce(aim * initialSpeed);
        launched = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
