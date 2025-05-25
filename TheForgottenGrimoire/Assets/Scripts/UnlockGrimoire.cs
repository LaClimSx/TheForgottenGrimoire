using UnityEngine;

public class UnlockGrimoire : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameObject.FindWithTag("grimoire").SetActive(true);
        Destroy(gameObject);
    }
}
