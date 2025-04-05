using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StaffGrabableScript : XRGrabInteractable
{
    [SerializeField] private Transform _grabAttachPoint;
    [SerializeField] private Transform _socketAttachPoint;

    private bool isInteractorTag(SelectEnterEventArgs args, string tag)
    {
        return args.interactableObject.transform.CompareTag(tag);
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        Transform interactor = args.interactorObject.transform;
        if (isInteractorTag(args, "StaffSocket")) attachTransform = _socketAttachPoint.transform;
        else if (isInteractorTag(args, "leftHandInteractor")) print("[DEBUG] staff is being grabbed");
        base.OnSelectEntering(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("StaffSocket")) attachTransform = _grabAttachPoint.transform;
        base.OnSelectExiting(args);
    }
}
