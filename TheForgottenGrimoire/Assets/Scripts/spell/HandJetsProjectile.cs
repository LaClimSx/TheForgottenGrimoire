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
        if (!collision.collider.CompareTag("leftHand") && !collision.collider.CompareTag("rightHand"))
        {
            if (IsLeft) MainSpell.LeftCollision = collision.GetContact(0).point;
            else MainSpell.RightCollision = collision.GetContact(0).point;
            Destroy(gameObject);
        }
        else
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider, true);
        }
        //GetComponent<Renderer>().material.color = Color.red;
    }
}
