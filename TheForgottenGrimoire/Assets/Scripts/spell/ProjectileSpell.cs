using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileSpell : MonoBehaviour
{
    [SerializeField] private InputActionReference rightTriggerReference;
    [SerializeField] float initialSpeed;
    private Rigidbody rb;

    public bool Launched { private set; get; }

    private void Update()
    {
        if (rightTriggerReference.action.triggered) launch(new Vector3(0, 0, 1));
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        transform.localPosition += new Vector3(0, 0, 1);
        Launched = false;
    }

    public void launch(Vector3 aim)
    {
        aim = aim.normalized;
        rb.AddRelativeForce(aim * initialSpeed);
        Launched = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
