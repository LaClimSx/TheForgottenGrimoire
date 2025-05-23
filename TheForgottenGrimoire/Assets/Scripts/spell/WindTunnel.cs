using UnityEngine;
using System.Collections.Generic;

public class WindTunnel : MonoBehaviour
{    
    [SerializeField] private float columnRadius;
    [SerializeField] private float strength;
    [SerializeField] private float stopPushingAfter;
    [SerializeField] private float maxIntervalBetweenCollision;
    private ParticleSystem particles;
    private Dictionary<Rigidbody, float> bodiesToMove;
    private float nextCheck = 0;

    private void FixedUpdate()
    {
        if (Time.time > nextCheck)
        {
            if (bodiesToMove.Count != 0)
            {
                List<Rigidbody> toRetire = new List<Rigidbody>();
                foreach (KeyValuePair<Rigidbody, float> body in bodiesToMove)
                {
                    if (body.Value + stopPushingAfter < Time.time) toRetire.Add(body.Key);
                    else moveBody(body.Key);
                }
            }
            nextCheck = Time.time + maxIntervalBetweenCollision;
        }        
    }

    private void Awake()
    {
        bodiesToMove = new Dictionary<Rigidbody, float>();
        particles = GetComponent<ParticleSystem>();
        var shape = particles.shape;
        shape.radius = columnRadius;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<InteractableMoveable>() != null) moveableHit(other);        
    }

    private void moveableHit(GameObject interactable)
    {
        Rigidbody body = interactable.GetComponent<Rigidbody>();
        if (bodiesToMove.ContainsKey(body)) bodiesToMove[body] = Time.time;
        else bodiesToMove.Add(body, Time.time);
    }

    private void moveBody(Rigidbody body) 
    {
        body.AddForce(transform.forward.normalized * strength);
    }         
}
