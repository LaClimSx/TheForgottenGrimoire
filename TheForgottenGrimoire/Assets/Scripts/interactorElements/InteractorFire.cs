using UnityEngine;
using static CustomTypes;

public class InteractorFire : InteractorElement
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        type = ElementType.Fire;
        // for now, always the same :
        power = 2f;
        form = SpellForm.Fist;
        base.Start();
        print($"i'm {gameObject.tag}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
    }
}
