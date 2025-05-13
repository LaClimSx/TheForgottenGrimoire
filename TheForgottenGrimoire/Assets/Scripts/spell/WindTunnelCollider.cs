using UnityEngine;

public class WindTunnelCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print("Colliding");
        GetComponentInParent<WindTunnel>().Blocked = true;
        GetComponentInParent<WindTunnel>().CollisionPoint = collision.GetContact(0).point;
        //float len = GetComponent<Renderer>().bounds.size.z;
        //print("z size: " + len);
        //print("collision point: " + collision.GetContact(0).point);
        //float newSize = transform.worldToLocalMatrix.MultiplyPoint3x4(collision.GetContact(0).point).z;
        //print("new size: " + newSize);
        //GetComponentInParent<WindTunnel>().RescaleLen = newSize * transform.localScale.z / len;
    }

    private void OnCollisionExit(Collision collision)
    {
        print("exit Colliding");
        GetComponentInParent<WindTunnel>().RescaleLen = 0;
        GetComponentInParent<WindTunnel>().Blocked = false;
        GetComponentInParent<WindTunnel>().Rescaled = false;
    }
}
