using Spells;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
public class UnlockSpell : MonoBehaviour
{
    [SerializeField] private SpellManager xrSpellCaster;
    [SerializeField] private SpellType[] spellTypes;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger enter");
        foreach (var type in spellTypes)
        {
            Debug.Log($"{type} unlocked");
            xrSpellCaster.UnlockSpell(type);
        }
        
        Destroy(gameObject);
    }
}