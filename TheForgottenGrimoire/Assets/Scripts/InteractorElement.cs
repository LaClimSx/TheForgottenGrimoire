using UnityEngine;
using static CustomTypes;

public class InteractorElement : MonoBehaviour
{
    public ElementType type; 
    [SerializeField] private float power;

    [SerializeField] private SpellForm form;

    void Start()
    {
        gameObject.tag = $"{type}_interactor";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        
    }

}
