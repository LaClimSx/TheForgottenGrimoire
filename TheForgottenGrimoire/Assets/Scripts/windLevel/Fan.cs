using UnityEngine;

[RequireComponent(typeof(InteractableConductor))]
public class Fan : MonoBehaviour
{
    [SerializeField] private GameObject rotors;
    [SerializeField] private float rotorSpeed;

    private bool _isOn;
    public bool IsOn 
    { 
        get { return _isOn; }
        set
        {
            if (!value) stopRotors();
            _isOn = value;
        }
    } 

    // Update is called once per frame
    void Update()
    {
        IsOn = GetComponent<InteractableConductor>().Power > 0;
        if (IsOn)
        {
            rotateRotors();
        }
    }

    private void rotateRotors()
    {
        rotors.transform.Rotate(transform.up, rotorSpeed);        
        GetComponent<ParticleSystem>().Play();
    }

    private void stopRotors()
    {
        GetComponent<ParticleSystem>().Stop();
    }        
}
