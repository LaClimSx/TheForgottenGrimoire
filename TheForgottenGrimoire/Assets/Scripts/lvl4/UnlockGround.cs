using UnityEngine;
using Spells;

public class UnlockGround : MonoBehaviour
{
    [SerializeField] private SpellManager spellManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spellManager.UnlockSpell(SpellType.Earthball);
            spellManager.UnlockSpell(SpellType.Cube);
        }
    }
}
