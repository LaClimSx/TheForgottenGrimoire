using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private Transform _origin;

    public void OnCollisionEnter(Collision collision)
    {
        this.gameObject.SetActive(false);
        this.transform.localPosition = _origin.localPosition;
        print("bob");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            transform.localPosition += new Vector3(0f, 0f, 0.5f);
        }  
    }
}
