using UnityEngine;
using UnityEngine.InputSystem;

public class HandJets : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileRange;
    private Transform leftHand;
    private Transform rightHand;
    [SerializeField] private float flightSpeed;
    //[SerializeField] private float jetForceStrength;
    [SerializeField] private float flightTime;

    private GameObject p1; 
    private GameObject p2;
    private Vector3? jetForce;
    private CharacterController player;

    public Vector3? LeftCollision { get; set; }
    public Vector3? RightCollision { get; set; }
    public bool WaitingForCollisions { get; private set; } = false;
    public bool ProjectileLaunched { get; private set; } = false;
    public bool CollisionOutOfRange { get; set; } = false;

    private float startflight;
    private float endflight = 0;

    private void spawnProjectiles()
    {
        p1 = Instantiate(projectile, leftHand.position, leftHand.rotation);
        p1.GetComponent<HandJetsProjectile>().IsLeft = true;
        p1.GetComponent<HandJetsProjectile>().MainSpell = this;
        p2 = Instantiate(projectile, rightHand.position, rightHand.rotation);
        p2.GetComponent<HandJetsProjectile>().MainSpell = this;
    }

    public void launchProjectiles()
    {
        p1.GetComponent<HandJetsProjectile>().launch(leftHand.forward, projectileSpeed, projectileRange);
        p2.GetComponent<HandJetsProjectile>().launch(rightHand.forward, projectileSpeed, projectileRange);
        WaitingForCollisions = true;
        ProjectileLaunched = true;
    }

    private Vector3 computeJetForce()
    {
        Vector3 leftForce = (leftHand.position - LeftCollision ?? Vector3.zero);
        Vector3 rightForce = (rightHand.position - RightCollision ?? Vector3.zero);
        return (leftForce + rightForce).normalized; //* jetForceStrength;
    }

    private void Awake()
    {
        leftHand = GameObject.FindGameObjectWithTag("leftHand").GetComponent<Transform>();
        rightHand = GameObject.FindGameObjectWithTag("rightHand").GetComponent<Transform>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        spawnProjectiles();
    }

    private void Update()
    {
        if (!CollisionOutOfRange)
        {
            if (LeftCollision != null && RightCollision != null && jetForce == null)
            {
                print("left collision: " + LeftCollision);
                print("right collision: " + RightCollision);
                Debug.DrawRay(leftHand.position, (LeftCollision - leftHand.position) ?? Vector3.zero, Color.red);
                Debug.DrawRay(rightHand.position, (RightCollision - rightHand.position) ?? Vector3.zero, Color.red);
                jetForce = computeJetForce();
                print("initial jet force: " + jetForce);
                startflight = Time.time;
                endflight = Time.time + flightTime;
                //GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().AddForce(jetForce * 300 ?? Vector3.zero);
                //GetComponent<Rigidbody>().useGravity = true;
                LeftCollision = null;
                RightCollision = null;
                WaitingForCollisions = false;
            }
            if (jetForce != null && Time.time < endflight && Time.time > startflight)
            {
                print("time since collision: " + (Time.time - startflight));
                print("force coef: " + (1 / (Time.time - startflight)) * Time.deltaTime);
                print("applied motion: " + (jetForce.Value * flightSpeed / (Time.time - startflight)) * Time.deltaTime);
                player.Move((jetForce.Value * flightSpeed / Mathf.Pow(Time.time - startflight, 1.1f)) * Time.deltaTime);
            }
            else if (Time.time >= endflight)
            {
                jetForce = null;
            }
        }
        else print("Collision out of range");
    }   
}
