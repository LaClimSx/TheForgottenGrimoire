using UnityEngine;


public class DoorManager : MonoBehaviour
{
    public float openAngle = -90f; // Negative if door swings inward; adjust as needed
    public float openSpeed = 2f;
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + openAngle, transform.eulerAngles.z);
    }

    void Update()
    {
        if (isOpen)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * openSpeed);
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
    }
}
