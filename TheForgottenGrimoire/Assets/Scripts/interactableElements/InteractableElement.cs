using UnityEngine;

public class InteractableElement : MonoBehaviour
{
    public enum InteractableType
    {
        Flammable,
        Conductor,
        Chargeable,
        Moveable,
        Climbable
    }

    public InteractableType Type { get; protected set; }

    public float Power { protected get; set; } = 0f;
}
