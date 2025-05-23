using UnityEngine;

public class Grimoire : MonoBehaviour
{
    [Header("Follow Target")]
    [SerializeField] private Transform _target;         // What it follows
    [SerializeField] private float followSpeed = 5f;    // Position smoothing
    [SerializeField] private Vector3 positionOffset;    // Offset from target

    [Header("Camera Billboard")]
    [SerializeField] private Transform cameraTransform; // Your XR camera (or leave null for Camera.main)
    [SerializeField] private bool onlyYRotation = true; // If true, locks out pitch/roll

    [Header("Base Rotation Offset")]
    [Tooltip("Local Euler angles to apply on top of the camera-facing rotation.")]
    [SerializeField] private Vector3 baseRotationOffset = Vector3.zero;

    private void Reset()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        // Snap into place with offset
        transform.position = GetTargetPosition();
        LookAtCamera(instant: true);
    }

    private void Update()
    {
        // 1) Smooth follow
        Vector3 desiredPos = GetTargetPosition();
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * followSpeed);

        // 2) Billboard + base offset
        LookAtCamera();
    }

    private Vector3 GetTargetPosition()
    {
        return _target.position + positionOffset;
    }

    /// <summary>
    /// Rotates the grimoire to face the camera, then applies the baseRotationOffset.
    /// </summary>
    /// <param name="instant">If true, snaps immediately instead of lerping</param>
    private void LookAtCamera(bool instant = false)
    {
        if (cameraTransform == null)
            return;

        // Direction from book to camera
        Vector3 dir = cameraTransform.position - transform.position;
        if (onlyYRotation)
            dir.y = 0;

        if (dir.sqrMagnitude < 0.0001f)
            return;

        // Base look rotation
        Quaternion lookRot = Quaternion.LookRotation(dir);

        // Apply your base offset (in local Euler angles)
        Quaternion offsetRot = Quaternion.Euler(baseRotationOffset);
        Quaternion finalRot = lookRot * offsetRot;

        if (instant)
            transform.rotation = finalRot;
        else
            transform.rotation = Quaternion.Lerp(transform.rotation, finalRot, Time.deltaTime * followSpeed);
    }

    // Called externally when grab is released to recalculate offset
    public void SetNewTargetPositionActual()
    {
        Vector3 localOffset = transform.position - _target.position;
        positionOffset = localOffset;
        Debug.Log($"Recomputed local offset: {positionOffset}");
    }
}
