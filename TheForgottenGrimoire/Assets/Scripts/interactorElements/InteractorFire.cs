using UnityEngine;

public class InteractorFire : InteractorElement
{
    private void Awake()
    {
        Type = InteractorType.Fire;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        // for now, always the same :
        //Power = 1f;
        //form = SpellForm.Fist;
    }
}
