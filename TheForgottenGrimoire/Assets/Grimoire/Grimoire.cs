using System;
using UnityEngine;

public class Grimoire : MonoBehaviour
{
    public Transform target; // Set to target to follow
    public float followSpeed = 5f;    // Smoothing
    
    public Vector3 positionOffset;
    public Vector3 angle;

    private void Start()
    {
        float angleY = target.eulerAngles.y;
        // Smooth follow
        transform.position = GetTargetPosition(angleY);

        // Make the book face the same way as the player
        transform.rotation = Quaternion.Euler(angle.x, angle.y + angleY, angle.z);
    }

    void Update()
    {
        if (!target) return;
        // Get the yaw angle (rotation around Y) of the player
        float angleY = target.eulerAngles.y;

        // Build desired position
        Vector3 desiredPosition = GetTargetPosition(angleY);

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);

        // Make the book face the same way as the player
        Quaternion targetRotation = Quaternion.Euler(angle.x, angle.y + angleY, angle.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * followSpeed);
    }

    private Vector3 GetTargetPosition(float angleY)
    {
        float angleRad = angleY * Mathf.Deg2Rad;

        // Calculate position based on direction the player is looking
        float offsetX = Mathf.Sin(angleRad) * positionOffset.x + Mathf.Cos(angleRad) * positionOffset.z;
        float offsetZ = Mathf.Cos(angleRad) * positionOffset.x - Mathf.Sin(angleRad) * positionOffset.z;

        // Build desired position
       return new Vector3(
            target.position.x + offsetX,
            target.position.y + positionOffset.y,
            target.position.z + offsetZ
        );
    }
}
