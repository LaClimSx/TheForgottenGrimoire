using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlammeThrowerProjectile : MonoBehaviour
{
    [SerializeField] private float range;
    private Vector3 spawnPos;

    private void Awake()
    {
        spawnPos = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(spawnPos, transform.position) > range) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("grimoire"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
        }
        else if (collision.collider.gameObject.GetComponent<InteractorFire>() == null && !collision.collider.CompareTag("leftHand"))
        {
            Destroy(gameObject);
        }
    }
}
