using UnityEngine;
using UnityEngine.InputSystem;

public class DummyManager : MonoBehaviour
{
    [SerializeField] private InputActionReference rightTriggerReference;
    [SerializeField] private Transform rightHandTransform;
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
        GameObject castFireball = Instantiate(fireball, rightHandTransform.localPosition, rightHandTransform.localRotation);
        castFireball.GetComponent<Fireball>().launch(new Vector3(0, 0, 1));
    }
}
