using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DrawSpell : MonoBehaviour
{
    [SerializeField] private InputActionReference triggerInputActionReference;

    [SerializeField] private GameObject staff;

    [SerializeField] private float minTrigger = 0.25f;

    [SerializeField] private float minDistanceBeforeNewPoint = 0.05f;

    [SerializeField] private float tubeDefaultWidth = 0.010f;
    [SerializeField] private int tubeSides = 8;

    [SerializeField] private Color defaultColor = Color.cyan;
    [SerializeField] private Material defaultLineMaterial;

    [SerializeField] private bool colliderTrigger = false;

    private Vector3 prevPointDistance = Vector3.zero;

    private List<Vector3> points = new List<Vector3>();
    private int count = 0;

    private TubeRenderer currentTubeRenderer;

    private bool isPinchingReleased = false;

    private Transform staffSphere;

    private StaffGrabableScript staffGrabableScript;

    void Start()
    {
        AddNewTubeRenderer();
        staffGrabableScript = staff.GetComponent<StaffGrabableScript>();

    }

    void Update()
    {
        if (!staffGrabableScript.inHand)
        {
            return;
        }

        staffSphere = staff.transform.Find("Sphere");

        float triggerValue = triggerInputActionReference.action.ReadValue<float>();

        if (triggerValue > minTrigger)
        {
            UpdateTube();
            isPinchingReleased = true;
            return;
        }
        if (isPinchingReleased)
        {

            //These are for debugging purposes
            string points_string = "";
            foreach (Vector3 point in points)
            {
                points_string += point.ToString() + ", ";
            }
            print($"Points: {points_string}");
            
            Destroy(currentTubeRenderer, 1f);
            AddNewTubeRenderer();
            isPinchingReleased = false;
        }

    }

    private void AddNewTubeRenderer()
    {
        points.Clear();
        GameObject go = new GameObject($"TubeRenderer__{count}");
        go.transform.position = Vector3.zero;

        TubeRenderer goTubeRenderer = go.AddComponent<TubeRenderer>();
        count++;

        var renderer = go.GetComponent<MeshRenderer>();
        renderer.material = defaultLineMaterial;
        renderer.material.color = defaultColor;

        goTubeRenderer.ColliderTrigger = colliderTrigger;
        goTubeRenderer.SetPositions(points.ToArray());
        goTubeRenderer._radiusOne = tubeDefaultWidth;
        goTubeRenderer._radiusTwo = tubeDefaultWidth;
        goTubeRenderer._sides = tubeSides;

        currentTubeRenderer = goTubeRenderer;
    }

    private void UpdateTube()
    {
        if (prevPointDistance == Vector3.zero)
        {
            prevPointDistance = staffSphere.transform.position;
        }

        if (Vector3.Distance(prevPointDistance, staffSphere.transform.position) >= minDistanceBeforeNewPoint)
        {
            prevPointDistance = staffSphere.transform.position;
            AddPoint(prevPointDistance);
        }
    }

    private void AddPoint(Vector3 position)
    {
        points.Add(position);
        currentTubeRenderer.SetPositions(points.ToArray());
        currentTubeRenderer.GenerateMesh();
    }

    public void UpdateLineWidth(float newValue)
    {
        currentTubeRenderer._radiusOne = newValue;
        currentTubeRenderer._radiusTwo = newValue;
        tubeDefaultWidth = newValue;
    }

    public void UpdateLineColor(Color color)
    {
        defaultColor = color;
        defaultLineMaterial.color = color;
        currentTubeRenderer.material = defaultLineMaterial;
    }

    public void UpdateLineMinDistance(float newValue)
    {
        minDistanceBeforeNewPoint = newValue;
    }
}
