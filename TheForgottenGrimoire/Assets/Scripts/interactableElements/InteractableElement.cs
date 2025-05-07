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

    //private float power = 0f;
    public float Power {
        get;
        set;
    }
}
