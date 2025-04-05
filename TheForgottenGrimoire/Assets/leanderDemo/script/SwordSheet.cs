using UnityEngine;

public class SwordSheet : MonoBehaviour
{
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<MeshCollider>();
    }

    public void onGrabbed() { _collider.enabled = false; }

    public void onThrow() { _collider.enabled = true; }
}
