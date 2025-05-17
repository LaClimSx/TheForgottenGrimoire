using UnityEngine;

public class FireParticles : MonoBehaviour
{
    private ParticleSystem particles;

    private void Awake()
    {
        setParticleSystemValue();
    }

    private void setParticleSystemValue()
    {
        particles = GetComponent<ParticleSystem>();
        var shape = particles.shape;
        shape.meshRenderer = transform.parent.GetComponent<MeshRenderer>();
    }
}
