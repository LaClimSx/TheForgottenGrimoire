using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class InteractorWind : InteractorElement
{

    private void Awake()
    {
        GetComponent<Rigidbody>().useGravity = false;
        Type = InteractorType.Wind;
    }
}
