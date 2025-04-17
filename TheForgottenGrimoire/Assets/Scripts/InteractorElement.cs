using UnityEngine;
using static CustomTypes;

public class InteractorElement : MonoBehaviour
{
    public ElementType type; 
    public float power;

    [SerializeField] protected SpellForm form;

    protected virtual void Start()
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
