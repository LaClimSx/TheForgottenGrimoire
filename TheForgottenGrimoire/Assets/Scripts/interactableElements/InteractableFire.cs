using UnityEngine;

public class InteractableFire : InteractableElement
{
    [SerializeField] private float life = 50f;
    private bool burning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Type = InteractableType.Flammable;
        burning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (burning && life > 0) {
            life -= Power;
            life = life < 0 ? 0 : life;
            print(life);
        } else if (life == 0) {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        InteractorElement interactor = collision.gameObject.GetComponent<InteractorFire>();
        if (interactor != null) {
            print($"bonked interactor {interactor.Type}");
            burning = true;
            Power += interactor.Power;
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
