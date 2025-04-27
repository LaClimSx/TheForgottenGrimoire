using UnityEngine;

public class Staff : MonoBehaviour
{
    [SerializeField] private GameObject _fireball;

    public void castFireball()
    {
        if (!_fireball.activeSelf)
        {
            _fireball.SetActive(true);
        }
    }
}
