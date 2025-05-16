using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HandJetsProjectile : MonoBehaviour
{    
    public bool IsLeft { get; set; } = false;
    public HandJets MainSpell { get; set; }
    private float range;
    private Vector3 launchPoint;
    private bool launched = false;

    private void Start()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; 
        GetComponent<Collider>().enabled = false;
    }

    public void launch(Vector3 direction, float speed, float range)
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        this.range = range;
        launchPoint = transform.position;        
        GetComponent<Rigidbody>().linearVelocity = direction * speed;
        launched = true;
    }

    private void Update()
    {
        if (launched && Vector3.Distance(launchPoint, transform.position) > range)
        {
            MainSpell.CollisionOutOfRange = true;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("leftHand") && 
            !collision.collider.CompareTag("rightHand") && 
            !collision.collider.CompareTag("magicStaff") && 
            !collision.collider.CompareTag("Player"))
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
