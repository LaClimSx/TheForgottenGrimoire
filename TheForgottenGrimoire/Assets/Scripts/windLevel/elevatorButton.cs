using UnityEngine;

public class elevatorButton : MonoBehaviour
{
    [SerializeField] private Transform target;    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<InteractorElec>() != null)
        {
            GameObject.FindWithTag("Player").transform.position = target.position;
        }
    }
}
