using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Casters;
using Toggle = UnityEngine.UI.Toggle;
public class UIManager : MonoBehaviour
{
    [SerializeField] private float smallDistanceTP = 5;
    [SerializeField] private float largeDistanceTP = 10;
    [SerializeField] private Toggle toggleTP;
    [SerializeField] private XRRayInteractor xrRayInteractor;

    [SerializeField] private float smallDistanceGrab = 5f;
    [SerializeField] private float largeDistanceGrab = 15f;

    [SerializeField] private Toggle toggleGrab;

    [SerializeField] private CurveInteractionCaster curveInteractionCasterLeft;
    [SerializeField] private CurveInteractionCaster curveInteractionCasterRight;

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toggleTP.onValueChanged.AddListener(on =>
        {
            if (toggleTP && xrRayInteractor) {
                xrRayInteractor.velocity = on ? largeDistanceTP : smallDistanceTP;
            } else {
                print("Missing XR Ray XRRayInteractor or toggleTP");
            }

        });

        toggleGrab.onValueChanged.AddListener(on =>
        {
            if (toggleGrab && curveInteractionCasterLeft && curveInteractionCasterRight) {
                float castDist = on ? largeDistanceGrab : smallDistanceGrab;
                curveInteractionCasterLeft.castDistance = castDist;
                curveInteractionCasterRight.castDistance = castDist;
            } else {
                print("Missing curveInteractionCasters or toggleGrab");
            }
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}