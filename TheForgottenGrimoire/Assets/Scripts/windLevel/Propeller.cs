using UnityEngine;

public class Propeller : MonoBehaviour
{
    [SerializeField] private float strength;
    private float gravity = -9.81f;
    private CharacterController controller;    

    private void Awake()
    {
        controller = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        print("Controller found " + controller != null);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 move = new Vector3(0f, 0f, 0f);
            //float distanceCoef = 1 / Mathf.Pow(controller.transform.position.y - transform.position.y, 2f);
            move.y = -strength * gravity;
            move.y += gravity * Time.deltaTime;
            controller.Move(move * Time.deltaTime);
        }
    }
}
