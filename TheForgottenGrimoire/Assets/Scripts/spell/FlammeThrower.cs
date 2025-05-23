using UnityEngine;

public class FlammeThrower : MonoBehaviour
{
    [SerializeField] private GameObject flameProjectile;
    [SerializeField] private float flameSpeed;
    [SerializeField] private float flameRate;
    private Transform leftHandTransform;
    private float nextShoot = 0;
    private GameObject previousProjectile;

    private void Awake()
    {
        leftHandTransform = GameObject.FindWithTag("leftHand").transform;        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextShoot)
        {
            shootFlameProjectile();
            nextShoot = Time.time + flameRate; 
        }
    }

    private void shootFlameProjectile()
    {
        GameObject projectile = Instantiate(flameProjectile, leftHandTransform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().AddForce(leftHandTransform.forward * flameSpeed);
        if (previousProjectile != null) Physics.IgnoreCollision(projectile.GetComponent<Collider>(), previousProjectile.GetComponent<Collider>(), true);
        previousProjectile = projectile;
    }
}
