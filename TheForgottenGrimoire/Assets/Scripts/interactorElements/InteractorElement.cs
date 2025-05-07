using UnityEngine;

public abstract class InteractorElement : MonoBehaviour
{
    public enum InteractorType
    {
        Earth,
        Wind,
        Fire,
        Elec,
        Space
    }
 
    public InteractorType Type { get; protected set; }

    [SerializeField] private float power = 0f;
    public float Power 
    { 
        get
        {
            return power;
        } 
        set
        {
            power = value;
        } 
    }

    //[SerializeField] public SpellForm form { get; protected set; } don't make sense to me
}
