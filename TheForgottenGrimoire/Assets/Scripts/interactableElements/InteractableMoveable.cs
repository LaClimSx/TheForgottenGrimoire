using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class InteractableMoveable : InteractableElement
{
    [SerializeField] private float mass;
    [SerializeField] private PhysicsMaterial _physicMaterial;

    private void Awake()
    {
        Type = InteractableType.Moveable;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().mass = mass;
        GetComponent<Collider>().material = _physicMaterial;
    }

    private void OnCollisionEnter(Collision collision)
    {
        InteractorElement interactor = collision.gameObject.GetComponent<InteractorWind>();
        if (interactor != null)
        {
            Vector3 dir = collision.GetContact(0).normal;
            GetComponent<Rigidbody>().AddForceAtPosition(interactor.Power * dir, collision.GetContact(0).point);
        }
    }
}
