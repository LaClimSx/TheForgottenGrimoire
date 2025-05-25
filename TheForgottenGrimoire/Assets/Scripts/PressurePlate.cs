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
        if (other.CompareTag("WeightedCube"))
        {
            print("pressed \n");
            isPressed = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WeightedCube"))
        {
            print("unpressed \n");
            isPressed = false;
        }
    }
}
