using UnityEngine;

public class PressurePlate : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        print("pressed \n");
    }

    void OnTriggerExit(Collider other)
    {
        print("unpressed \n");
    }
}
