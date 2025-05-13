using UnityEngine;

public class WindTunnel : MonoBehaviour
{
    [SerializeField] private float growthAmount;
    [SerializeField] private float growthSpeed;
    [SerializeField] private GameObject cylinder;
    [SerializeField] private GameObject endPoint;

    private float nextUpdate = 0;
    public bool Blocked { get; set; } = false;
    public Vector3 CollisionPoint { get; set; }
    public bool Rescaled { get; set; } = false;
    public float RescaleLen { get; set; }

    void Update()
    {
        if (Time.time >= nextUpdate)
        {
            if (!Blocked) grow();
            nextUpdate = Time.time + growthSpeed;           
        }
        if (Blocked && !Rescaled) resize();
    }

    private void grow()
    {        
        transform.localScale += new Vector3(0, 0, 1) * growthAmount;
    }
    
    private void resize()
    {
        Vector3 originToCollision = CollisionPoint - transform.position;
        Vector3 goalPosition = transform.position + Vector3.Project(originToCollision, transform.forward);
        while(Vector3.Distance(endPoint.transform.position, goalPosition) >= 0.1)
        {
            print("looping");
            transform.localScale += new Vector3(0, 0, 1) * 0.05f;
        }
    }
}
