using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Climbing;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InteractableClimbable : InteractableElement
{
    [SerializeField] private GameObject _handle;
    [SerializeField] private float handleScaleX;
    [SerializeField] private float handleScaleY;
    [SerializeField] private float handleScaleZ;
    [SerializeField] private GameObject _handles;
    private int handleCounter;
    private GameObject handles;

    private void Awake()
    {
        Type = InteractableType.Climbable;
        handles = Instantiate(_handles, transform.position, transform.rotation, transform);
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
        GameObject handle = Instantiate(_handle, coord, Quaternion.identity, handles.transform);
        handle.transform.localScale = new Vector3(handle.transform.localScale.x * handleScaleX, handle.transform.localScale.y * handleScaleY, handle.transform.localScale.z * handleScaleZ);
        handle.name = "generated handle " + handleCounter++;
        ClimbInteractable cInteractable = handles.GetComponent<ClimbInteractable>();
        XRInteractionManager manager = cInteractable.interactionManager;
        print("adding to climbing wall " + handle);
        cInteractable.colliders.Add(handle.GetComponent<Collider>());
        manager.UnregisterInteractable((IXRInteractable) cInteractable);
        manager.RegisterInteractable((IXRInteractable) cInteractable);
    }
}
