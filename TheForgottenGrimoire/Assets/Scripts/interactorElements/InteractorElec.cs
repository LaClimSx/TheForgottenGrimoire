using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractorElec : InteractorElement
{
    private void Awake()
    {
        Type = InteractorType.Elec;
    }
}
