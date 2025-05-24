using UnityEngine;

public class Lightable : MonoBehaviour
{
    private bool isLit = false;
    private int duration = 1000;
    public bool done = false;

    [SerializeField] private GameObject particlesPrefab;
    private GameObject particles;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (duration == 0 && particles != null)
        {
            done = true;
            audioSource.enabled = false;
            Destroy(particles);
            return;
        }
        if (isLit)
        {
            duration = duration - 1;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        print("bonked lightable");
        InteractorElement interactor = other.gameObject.GetComponent<InteractorFire>();
        if (interactor != null)
        {
            isLit = true;
            audioSource.enabled = true;
            particles = Instantiate(particlesPrefab, transform);

        }
    }

}
