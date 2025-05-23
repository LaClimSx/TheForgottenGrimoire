using UnityEngine;

public class Feet : MonoBehaviour
{

    [SerializeField] private Transform _player; // Set to player camera

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_player.position.x, _player.position.y - 1.2f, _player.position.z) ;
    }

    void OnTriggerEnter(Collider other)
    {
        print("ohlala \n");
    }
}
