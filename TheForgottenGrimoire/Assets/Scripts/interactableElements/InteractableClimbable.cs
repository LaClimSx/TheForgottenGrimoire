using UnityEngine;

public class InteractableClimbable : InteractableElement
{
    [SerializeField] private GameObject _handle;
    [SerializeField] private GameObject _handles;

    private void Awake()
    {
        Type = InteractableType.Climbable;
    }

    private void OnCollisionEnter(Collision collision)
    {
        InteractorElement interactor = collision.gameObject.GetComponent<InteractorElement>();
        if (interactor != null && interactor.Type == InteractorElement.InteractorType.Earth)
        {
            createSingleHandle(collision.GetContact(0).point);
            Destroy(collision.gameObject);
        }
    }

    private void createSingleHandle(Vector3 coord)
    {
        Instantiate(_handle, coord, Quaternion.identity, transform.parent.Find(_handles.name).transform);
    }
}
