using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractorElec : InteractorElement
{
    private void Awake()
    {
        Type = InteractorType.Elec;
    }

    private void OnCollisionEnter(Collision collision)
    {
        InteractableElement interactable = collision.collider.GetComponent<InteractableElement>();
        if (interactable == null || (interactable.Type != InteractableElement.InteractableType.Conductor || interactable.Type != InteractableElement.InteractableType.Chargeable))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
        }
    }
}
