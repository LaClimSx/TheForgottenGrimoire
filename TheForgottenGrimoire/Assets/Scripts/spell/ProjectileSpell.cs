using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileSpell : MonoBehaviour
{
    [SerializeField] float initialSpeed;
    private Rigidbody rb;
    
    public enum ProjectileType
    {
        Fireball,
        FlameThrower,
        Elec,
        Wind,
        earth,
    }
    public ProjectileType Type { get; private set; }

    public bool Launched { private set; get; }    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Launched = false;
        GetComponent<Collider>().enabled = false;
        setProjectileType();
    }

    private void setProjectileType()
    {        
        switch (transform.tag)
        {
            case "Fireball":
                print("Should be trigger");
                Type = ProjectileType.Fireball;
                break;
            case "FlameThrower":
                Type=ProjectileType.FlameThrower;
                break;
            default:
                Debug.LogError("Unknown projectile");
                break;
        }
    }

    public void launch(Vector3 aim)
    {
        
        rb.isKinematic = false;
        rb.AddForce(aim.normalized * initialSpeed);
        Launched = true;
        GetComponent<Collider>().enabled = true;
        if (Type == ProjectileType.Fireball)
        {
            GetComponentInChildren<FireParticles>().OnLaunch(aim);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
