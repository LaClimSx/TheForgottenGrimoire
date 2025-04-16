using UnityEngine;
using static CustomTypes;

public class InteractableElement : MonoBehaviour
{
    public ElementType type; 
    [SerializeField] private float power;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == $"{type}_interactor") {

        }
    }

}
