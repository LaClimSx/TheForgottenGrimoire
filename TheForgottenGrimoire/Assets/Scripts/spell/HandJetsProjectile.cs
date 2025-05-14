using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HandJetsProjectile : MonoBehaviour
{
    public bool IsLeft { get; set; } = false;
    public HandJets MainSpell { get; set; }

    public void launch(Vector3 direction, float speed)
    {
        GetComponent<Rigidbody>().linearVelocity = direction * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsLeft) MainSpell.LeftCollision = collision.GetContact(0).point;
        else MainSpell.RightCollision = collision.GetContact(0).point;
        Destroy(gameObject);
    }
}
