using UnityEngine;

public class BetterHandjets : MonoBehaviour
{
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private GameObject wind;
    private GameObject leftWind;
    private GameObject rightWind;

    public bool LeftCollided { get; set; } = false;
    public float LeftLastCollideTime { get; set; }
    public bool rightCollided { get; set; } = false;
    public float rightLastCollideTime { get; set; }

    private void Start()
    {
        leftWind = Instantiate(wind, leftHand);
        rightWind = Instantiate(wind, rightHand);
    }
}
