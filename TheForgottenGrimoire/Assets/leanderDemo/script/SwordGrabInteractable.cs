using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SwordGrabInteractable : XRGrabInteractable
{
    [SerializeField] private GameObject _blade;
    [SerializeField] private Transform _attachPoint1;
    [SerializeField] private Transform _attachPoint2;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("roc"))
        {
            _blade.GetComponent<Collider>().providesContacts = false;
            attachTransform = _attachPoint2;
        }
        base.OnSelectEntering(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("roc"))
        {
            attachTransform = _attachPoint1;
            _blade.GetComponent<Collider>().providesContacts = true;
        }
        base.OnSelectExiting(args);
    }
}
