using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ApplyForce : MonoBehaviour
{
    [SerializeField] private Vector3 _force;

    void Start()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.InverseTransformDirection(_force);
    }
}
