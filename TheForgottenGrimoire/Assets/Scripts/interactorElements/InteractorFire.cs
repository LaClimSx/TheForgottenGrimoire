using UnityEngine;
using InteractionTypes;

public class InteractorFire : InteractorElement
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        type = InteractorType.Fire;
        // for now, always the same :
        power = 1f;
        form = SpellForm.Fist;
        base.Start();
        print($"i'm {gameObject.tag}");
    }

    public void OnCollisionEnter(Collision collision)
    {
    }
}
