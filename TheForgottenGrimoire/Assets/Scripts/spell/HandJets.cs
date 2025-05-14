using UnityEngine;

public class HandJets : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    private GameObject p1; 
    private GameObject p2;
    private Vector3? jetForce;

    public Vector3? LeftCollision { get; set; }
    public Vector3? RightCollision { get; set; }

    private void spawnProjectiles()
    {
        p1 = Instantiate(projectile, leftHand.position, leftHand.rotation, leftHand);
        p1.GetComponent<HandJetsProjectile>().IsLeft = true;
        p1.GetComponent<HandJetsProjectile>().MainSpell = this;
        Physics.IgnoreCollision(p1.GetComponent<Collider>(), gameObject.GetComponent<Collider>(), true);
        p2 = Instantiate(projectile, rightHand.position, rightHand.rotation, rightHand);
        p2.GetComponent<HandJetsProjectile>().MainSpell = this;
        Physics.IgnoreCollision(p2.GetComponent<Collider>(), gameObject.GetComponent<Collider>(), true);
    }

    private void launchProjectiles()
    {
        p1.GetComponent<HandJetsProjectile>().launch(leftHand.forward, projectileSpeed);
        p2.GetComponent<HandJetsProjectile>().launch(rightHand.forward, projectileSpeed);
    }

    private Vector3 computeJetForce()
    {
        Vector3 leftForce = (leftHand.position - LeftCollision ?? Vector3.zero);
        Vector3 rightForce = (rightHand.position - RightCollision ?? Vector3.zero);
        return leftForce + rightForce;
    }

    private void Start()
    {
        spawnProjectiles();
        launchProjectiles();
    }

    private void Update()
    {
        if (LeftCollision != null && RightCollision != null && jetForce == null)
        {
            print("left collision: " + LeftCollision);
            print("right collision: " + RightCollision);
            Debug.DrawRay(leftHand.position, (LeftCollision - leftHand.position) ?? Vector3.zero, Color.red);
            Debug.DrawRay(rightHand.position, (RightCollision - rightHand.position) ?? Vector3.zero, Color.red);
            jetForce = computeJetForce();
            GetComponent<Rigidbody>().AddForce(jetForce ?? Vector3.zero);
            GetComponent<Rigidbody>().useGravity = true;

        }            
        if (jetForce != null) Debug.DrawRay(transform.position, jetForce ?? Vector3.zero, Color.blue);
    }   
}
