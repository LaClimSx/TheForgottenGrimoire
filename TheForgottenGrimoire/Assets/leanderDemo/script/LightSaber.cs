using UnityEngine;

public class LightSaber : MonoBehaviour
{
    [SerializeField] private GameObject _blade;
    [SerializeField] private GameObject _visualSwitch;

    private Vector3 switchOffset;

    private void Start()
    {
        switchOffset = new Vector3(0.03f, 0f, 0f);
    }

    public void bladeOn()
    {
    
        if (!_blade.activeSelf)
        {
            _blade.SetActive(true);
            _visualSwitch.transform.localPosition -= switchOffset;
        }
        else
        {
            _blade.SetActive(false);
            _visualSwitch.transform.localPosition += switchOffset;
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
