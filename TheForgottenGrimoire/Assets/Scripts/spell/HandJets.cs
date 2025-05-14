using UnityEngine;
using UnityEngine.InputSystem;

public class HandJets : MonoBehaviour
{
    [SerializeField] private InputActionReference triggerRef;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    private GameObject p1; 
    private GameObject p2;
    private Vector3? jetForce;
    private CharacterController player;

    public Vector3? LeftCollision { get; set; }
    public Vector3? RightCollision { get; set; }
    private bool waitingForCollisions = false;

    private void spawnProjectiles()
    {
        p1 = Instantiate(projectile, leftHand.position, leftHand.rotation);
        p1.GetComponent<HandJetsProjectile>().IsLeft = true;
        p1.GetComponent<HandJetsProjectile>().MainSpell = this;
        p2 = Instantiate(projectile, rightHand.position, rightHand.rotation);
        p2.GetComponent<HandJetsProjectile>().MainSpell = this;
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (triggerRef.action.triggered && !waitingForCollisions)
        {
            spawnProjectiles();
            launchProjectiles();
            waitingForCollisions = true;
        }
        if (LeftCollision != null && RightCollision != null && jetForce == null)
        {
            print("left collision: " + LeftCollision);
            print("right collision: " + RightCollision);
            Debug.DrawRay(leftHand.position, (LeftCollision - leftHand.position) ?? Vector3.zero, Color.red);
            Debug.DrawRay(rightHand.position, (RightCollision - rightHand.position) ?? Vector3.zero, Color.red);
            jetForce = computeJetForce();            
            //GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().AddForce(jetForce * 300 ?? Vector3.zero);
            //GetComponent<Rigidbody>().useGravity = true;
            LeftCollision = null;
            RightCollision = null;
            waitingForCollisions = false;
        }
        if (jetForce != null && player.isGrounded)
        {
            jetForce = null;
        }
    }   
}
