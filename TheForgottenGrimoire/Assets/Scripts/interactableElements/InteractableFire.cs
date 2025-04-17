using UnityEngine;
using static CustomTypes;

public class InteractableFire : InteractableElement
{
    [SerializeField] private float life = 10f;
    private bool burning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        type = ElementType.Fire;
    }

    // Update is called once per frame
    void Update()
    {
        if (burning) {
            life -= power;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == $"{type}_interactor") {
            print($"bonked {type}_interactor");
        }

    }
}
