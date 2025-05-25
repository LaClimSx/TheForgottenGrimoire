using UnityEngine;

public class UnlockGrimoire : MonoBehaviour
{
    [SerializeField] private GameObject grim;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            grim.GetComponent<Grimoire>().enabled = true;
            Destroy(gameObject);
        }        
    }
}
