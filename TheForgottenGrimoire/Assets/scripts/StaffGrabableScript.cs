using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StaffGrabableScript : XRGrabInteractable
{
    [SerializeField] private Transform _grabAttachPoint;
    [SerializeField] private Transform _socketAttachPoint;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("StaffSocket")) attachTransform = _socketAttachPoint.transform;
        base.OnSelectEntering(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("StaffSocket")) attachTransform = _grabAttachPoint.transform;
        base.OnSelectExiting(args);
    }
}
