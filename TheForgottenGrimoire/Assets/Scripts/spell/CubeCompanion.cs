using UnityEngine;

public class CubeCompanion : MonoBehaviour
{
    [SerializeField] private float lifetime;
    private float death;

    private void Start()
    {
        death = Time.time + lifetime;
    }

    void Update()
    {
        if (Time.time > death)
        {
            SpellManager manager = GameObject.FindWithTag("spellManager").GetComponent<SpellManager>();
            if (manager == null) Debug.LogError($"Spell manager null in cube companion {name}");
            manager.RemoveCompanionCube(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("grimoire"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
        }
    }
}
