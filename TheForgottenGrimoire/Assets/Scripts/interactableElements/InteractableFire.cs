using UnityEngine;
using static CustomTypes;

public class InteractableFire : InteractableElement
{
    [SerializeField] private float life = 50f;
    private bool burning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        type = ElementType.Fire;
        burning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (burning && life > 0) {
            life -= power;
            life = life < 0 ? 0 : life;
            print(life);
        } else if (life == 0) {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.tag == $"{type}_interactor") {
            print($"bonked {type}_interactor");
            burning = true;
        }

    }
}
