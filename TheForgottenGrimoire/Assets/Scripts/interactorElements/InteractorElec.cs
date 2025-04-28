using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractorElec : InteractorElement
{
    private void Awake()
    {
        Type = InteractorType.Elec;
    }

    public void GetCurrentFrom(InteractorElec originInteractor)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            InteractorElec colliderInteractor = collider.gameObject.GetComponent<InteractorElec>();
            bool isNotSrc = true;
            if (colliderInteractor != null) isNotSrc = !originInteractor.Equals(colliderInteractor);
            if (!collider.gameObject.Equals(gameObject) && isNotSrc)
            {
                InteractableConductor interactable = collider.gameObject.GetComponent<InteractableConductor>();
                if (interactable != null) interactable.ElecFromAlreadyCollidingSrc(this);
            }
        } 
    }

    public void RemoveCurrentFrom(InteractorElec originInteractor, float power)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            InteractorElec colliderInteractor = collider.gameObject.GetComponent<InteractorElec>();
            bool isNotSrc = true;
            if (colliderInteractor != null) isNotSrc = !originInteractor.Equals(colliderInteractor);
            if (!collider.gameObject.Equals(gameObject) && isNotSrc)
            {
                InteractableConductor interactable = collider.gameObject.GetComponent<InteractableConductor>();
                if (interactable != null) interactable.removeElecFromStillCollidingSource(this, power);
            }
        }
    }
}
