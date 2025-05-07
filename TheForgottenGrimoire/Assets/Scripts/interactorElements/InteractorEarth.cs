using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractorEarth : InteractorElement
{
    private void Awake()
    {
        Type = InteractorType.Earth;
    }
}
