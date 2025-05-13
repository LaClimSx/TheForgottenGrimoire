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
            GameObject.FindWithTag("spellManager").GetComponent<SpellManager>().RemoveCompanionCube(gameObject);
            Destroy(gameObject);
        }
    }
}
