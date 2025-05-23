using UnityEngine;
using System.Collections.Generic;
using Spells;
using UnityEngine.InputSystem;
using static Spells.SpellType;

public class DrawSpell : MonoBehaviour
{
    [SerializeField] private InputActionReference rightTriggerInputActionReference;
    [SerializeField] private InputActionReference leftTriggerInputActionReference;
    [SerializeField] private InputActionReference leftGripInputActionReference;

    [SerializeField] private GameObject staff;

    [SerializeField] private float minPression = 0.25f;
    [SerializeField] private float spellScoreThreshold = 0.5f;

    [SerializeField] private float minDistanceBeforeNewPoint = 0.05f;

    [SerializeField] private float tubeDefaultWidth = 0.010f;
    [SerializeField] private int tubeSides = 8;

    [SerializeField] private Color defaultColor = Color.cyan;
    [SerializeField] private Material defaultLineMaterial;

    [SerializeField] private bool colliderTrigger = false;

    [SerializeField] private GameObject mainCamera; 

    private Vector3 prevPointDistance = Vector3.zero;

    private List<Vector3> points = new List<Vector3>();
    private int count = 0;

    private TubeRenderer currentTubeRenderer;

    private bool isPinchingReleased = false;

    private Transform staffSphere;

    private StaffGrabableScript staffGrabableScript;

    private SpellDetector SpellDetector = new SpellDetector();

    private SpellManager spellManager;


    void Start()
    {
        AddNewTubeRenderer();
        staffGrabableScript = staff.GetComponent<StaffGrabableScript>();
        spellManager = GetComponent<SpellManager>();

    }

    void Update()
    {
        if (!staffGrabableScript.InHand)
        {
            return;
        }

        staffSphere = staff.transform.Find("Sphere");

        bool rightTriggerPressed = rightTriggerInputActionReference.action.ReadValue<float>() > minPression;
        bool leftTriggerPressed = leftTriggerInputActionReference.action.ReadValue<float>() > minPression;
        bool leftGripPressed = leftGripInputActionReference.action.ReadValue<float>() > minPression;

        SpellState spellState = spellManager.SpellState;

        if (rightTriggerPressed && spellState == SpellState.Pending)
        {
            UpdateTube();
            isPinchingReleased = true;
            return;
        }
        if (isPinchingReleased)
        {

            /*//These are for debugging purposes
            string points_string = "";
            foreach (Vector3 point in points)
            {
                points_string += point.ToString() + ", ";
            }
            print($"Points: {points_string}");
            */
            spellManager.SpellState = SpellState.Drawing;
            SpellDetector.DetectSpell(points, mainCamera.transform.position, (SpellShape detected, double score) =>
            {
                Debug.Log("detected:" + detected + " score:" + score);
                OnSpellDetected(detected, score, leftTriggerPressed, leftGripPressed);
            });
            Destroy(currentTubeRenderer.gameObject, 1f);
            AddNewTubeRenderer();
            isPinchingReleased = false;
        }

    }

    private void OnSpellDetected(SpellShape detected, double score, bool pinch, bool grip)
    {
        if ((score < spellScoreThreshold && detected != SpellShape.Infinity && detected != SpellShape.Spiral) || score < 0.35f ) //If the score is too low (0.5 for most shapes, and 0.35 for infinity and spiral)
        {
            Debug.Log("Spell not detected");
            spellManager.SpellState = SpellState.Pending;
            return;
        }
        SpellType spellType = NoSpell;
        switch (detected)
        {
            case SpellShape.Lightning:
                switch (pinch, grip)
                {
                    case (false, true): //Only grip -> Far zap
                        spellType = ChargeShot;
                        break;
                    case (false, false): //Open hand -> Palpatine
                        spellType = ArcHands;
                        break;
                    default:
                        Debug.Log("Spell not detected");
                        break;
                }
                break;
            case SpellShape.Spiral:
                switch (pinch, grip)
                {
                    case (false, true): //Only grip -> Hand jet
                        spellType = HandJet;
                        break;
                    case (false, false): //Open hand -> Wind Column
                        spellType = WindColumn;
                        break;
                    default:
                        Debug.Log("Spell not detected");
                        break;
                }
                break;
            case SpellShape.Square:
                switch (pinch, grip)
                {
                    case (true, false): //Only pinch -> Companion cube
                        spellType = Cube;
                        break;
                    case (false, true): //Only grib -> Climbable rock projectile
                        spellType = Earthball;
                        break;
                    default:
                        Debug.Log("Spell not detected");
                        break;
                }
                break;
            case SpellShape.Infinity:
                switch (pinch, grip)
                {
                    case (true, false): //Only pinch -> Telekinesis
                        spellType = Telekinesis;
                        break;
                    case (false, true): //Only grip -> TP Hub
                        spellType = Hub;
                        break;
                    case (false, false): //Open hand -> TP far
                        spellType = Blinkstep;
                        break;
                    default:
                        Debug.Log("Spell not detected");
                        break;
                }
                break;
            case SpellShape.Triangle:
                switch (pinch, grip)
                {
                    case (true, false): //Only pinch -> Light that stays with you
                        spellType = Emberlight;
                        break;
                    case (false, true): //Only grip -> Fireball
                        spellType = SpellType.Fireball; //FOR SOME REASON JE PEUX PAS ENLEVER LE SPELLTYPE
                        break;
                    case (false, false): //Open hand -> Flame thrower
                        spellType = FlameJet;
                        break;
                    default:
                        Debug.Log("Spell not detected");
                        break;
                }
                break;
            default:
                Debug.Log("Spell not detected");
                spellManager.SpellState = SpellState.Pending;
                return;
        }
        if (spellType != NoSpell)
        {
            if (spellManager.IsSpellUnlocked(spellType))
            {
                spellManager.CurrentSpellType = spellType;
                spellManager.SpellState = SpellState.CastSuccessfull;
            }
            else
            {
                Debug.Log($"Spell not unlocked: {spellType}");
                spellManager.SpellState = SpellState.Pending;
                return;
            }
        }
        else
        {
            spellManager.SpellState = SpellState.Pending;
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