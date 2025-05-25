using UnityEngine;

public class resetPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject respawnableCube;
    [SerializeField] private GameObject originalCube;


    public void respawnCube()
    {
        if (originalCube != null) return;
        originalCube = Instantiate(respawnableCube, new Vector3(-21.6f, 9.16f, 139.37f), new Quaternion(0.0f, 0.0f, 0.0f, 1f));
    }
}
