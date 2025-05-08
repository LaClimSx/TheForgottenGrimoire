using UnityEngine;
using UnityEngine.InputSystem;

public class DummyManager : MonoBehaviour
{
    [SerializeField] private InputActionReference rightTriggerReference;
    [SerializeField] private GameObject fireball;
    
    void Update()
    {
        if (rightTriggerReference.action.triggered)
        {
            castFireball();
        }
    }

    private void castFireball()
    {
        GameObject castFireball = Instantiate(fireball, new Vector3(0, 1, 0), Quaternion.identity);
        castFireball.GetComponent<Fireball>().launch(new Vector3(0, 1, 0));
    }
}
