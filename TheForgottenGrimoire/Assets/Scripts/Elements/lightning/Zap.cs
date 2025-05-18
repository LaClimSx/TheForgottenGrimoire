using UnityEngine;

public class Zap : MonoBehaviour
{
    public float LifeSpan { get; set; }
    private float death;

    private void Start()
    {
        death = Time.time + LifeSpan;             
    }

    private void Update()
    {
        if (death < Time.time) {
            Destroy(gameObject);
        }
    }
}
