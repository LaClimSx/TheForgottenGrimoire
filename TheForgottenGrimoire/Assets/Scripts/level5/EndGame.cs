using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject finalText;

    private void OnTriggerEnter(Collider other)
    {     
        if (other.CompareTag("Player"))
        {
            finalText.SetActive(true);
            GetComponent<ParticleSystem>().Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            finalText.SetActive(false);
            GetComponent<ParticleSystem>().Stop();
        }
    }
}
