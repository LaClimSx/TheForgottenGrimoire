using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private bool isPressed = false;
    public bool IsPressed
	{
		get { return isPressed; }
	}

    void OnTriggerEnter(Collider other)
    {
        print("pressed \n");
        isPressed = true;
    }

    void OnTriggerExit(Collider other)
    {
        print("unpressed \n");
        isPressed = false;
    }
}
