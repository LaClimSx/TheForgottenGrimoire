using UnityEngine;
using static CustomTypes;

public class InteractableElement : MonoBehaviour
{
    public ElementType type; 
    [SerializeField] protected float power;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        power = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == $"{type}_interactor") {
            print($"super bonked {type}_interactor");
            power = collision.gameObject.GetComponent<InteractorElement>().power;
        }
    }

}
