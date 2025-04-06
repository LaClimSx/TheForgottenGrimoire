using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using Toggle = UnityEngine.UI.Toggle;

public class UIManager : MonoBehaviour
{
    [SerializeField] private float smallDistanceTP = 5;
    [SerializeField] private float largeDistanceTP = 10;
    [SerializeField] private Toggle toggle;
    [SerializeField] private XRRayInteractor xrRayInteractor;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toggle.onValueChanged.AddListener(on =>
        {
            if (toggle && xrRayInteractor) {
                xrRayInteractor.velocity = on ? largeDistanceTP : smallDistanceTP;
            } else {
                print("Missing XR Ray XRRayInteractor or toggle");
            }

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
