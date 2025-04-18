using UnityEngine;
using InteractionTypes;

public abstract class InteractableElement : MonoBehaviour
{
    public InteractableType type; 
    [SerializeField] protected float power = 0f;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == $"{type}_interactor") {
            print($"super bonked {type}_interactor");
            power = collision.gameObject.GetComponent<InteractorElement>().power;
        }
    }

}
