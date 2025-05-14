using UnityEngine;

public class TestRaycast : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;

    private void Start()
    {
        raycast(left);
        raycast(right);
    }

    private void raycast(Transform origin)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin.position, origin.forward, out hit))
        {
            Debug.DrawRay(origin.position, origin.forward * hit.distance, Color.red);
            GameObject o = Instantiate(arrow, hit.point, Quaternion.identity);
            o.transform.up = hit.normal;
            o.transform.localScale *= hit.distance;
            print("normal: " + hit.normal);
        }
    }
}
