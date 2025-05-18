using UnityEngine;

public class FireParticles : MonoBehaviour
{
    private ParticleSystem particles;    
    //public enum ParticleType
    //{
    //    Fireball,
    //    FlameThrower,
    //    Default
    //}
    //public ParticleType Type { get; private set; }
    //private GameObject parent;

    private void Awake()
    {
        setParticleSystemValue();
        //setParticleType();
        //parent = transform.parent.gameObject;
    }

    private void setParticleSystemValue()
    {
        particles = GetComponent<ParticleSystem>();
        var shape = particles.shape;
        shape.meshRenderer = transform.parent.GetComponent<MeshRenderer>();
    }

    public void OnLaunch(Vector3 dir)
    {
        transform.forward = dir.normalized;
        var vol = particles.velocityOverLifetime;
        vol.space = ParticleSystemSimulationSpace.Local;
        vol.xMultiplier = 0f;
        vol.yMultiplier = 0f;
        vol.zMultiplier = -7f;
        float oldOrbital = vol.orbitalYMultiplier;
        vol.orbitalYMultiplier = 0f;
        vol.orbitalZMultiplier = oldOrbital;
    }

    //private void setParticleType()
    //{
    //    switch (transform.parent.tag)
    //    {
    //        case "Fireball":
    //            Type = ParticleType.Fireball;
    //            break;
    //        case "FlameThrower":
    //            Type=ParticleType.FlameThrower;
    //            break;
    //        default:
    //            Type = ParticleType.Default;
    //            break;
    //    }
    //}    
}
