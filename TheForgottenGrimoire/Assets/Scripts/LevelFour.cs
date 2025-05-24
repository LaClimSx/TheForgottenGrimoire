using UnityEngine;

public class LevelFour : MonoBehaviour
{
    [SerializeField] private PressurePlate leftPlate;
    [SerializeField] private PressurePlate rightPlate;
    [SerializeField] private SlidingDoor door;
    private bool doorIsOpen = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (leftPlate.IsPressed && rightPlate.IsPressed)
		{
			if (!doorIsOpen)
			{
				door.ToggleDoorOpen();
				doorIsOpen = true;
			}
		}
		else
		{
			if (doorIsOpen)
			{
				door.ToggleDoorOpen();
				doorIsOpen = false;
			}
		}
        
    }
}
