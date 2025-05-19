using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class Palpatine : MonoBehaviour
{
    [SerializeField] private float lightningLifeSpan;
    [SerializeField] private float lightningSpawnInterval;
    [SerializeField] private int lightningResolution;
    [SerializeField] private GameObject lightning;
    private float nextSpawn;
    private (float x, float y) hitBoxEndPlanSize = (1f, 0.15f);

    [SerializeField, Range(0, 1)] private float maxDisplacement;

    struct Branch
    {
        public Branch(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
        }

        public Vector3 Start { get; }
        public Vector3 End { get; }

        public override string ToString()
        {
            return $"Branch: {Start} -> {End}";
        }
    }

    //private void Awake()
    //{
    //    lightning = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/customPrefabs/Elements/lightning.prefab");    
    //}

    private void Start()
    {
        generateLightning();
        nextSpawn = Time.time + lightningSpawnInterval;
    }

    private void Update()
    {
        if (Time.time > nextSpawn)
        {
            generateLightning();
            nextSpawn += lightningSpawnInterval;
        }
    }

    private void generateLightning()
    {
        Vector3 start = transform.position;
        Vector3 end = randomEndPoint();
        Branch firstBranch = new Branch(start, end);
        List<Branch> previousGen = new List<Branch> { firstBranch };
        List<Branch> currentGen;
        for (int i = 0; i < lightningResolution; i++)
        {
            currentGen = new List<Branch>();
            foreach (Branch b in previousGen)
            {
                var subBranch = split(b);
                currentGen.Add(subBranch.Item1);
                currentGen.Add(subBranch.Item2);
            }
            previousGen = currentGen;
        }        
        List<Vector3> lightningPoints = branchToVector3(previousGen);
        GameObject spawnedLightning = Instantiate(lightning, transform);
        spawnedLightning.GetComponent<Zap>().LifeSpan = lightningLifeSpan;
        LineRenderer renderedLightning = spawnedLightning.GetComponent<LineRenderer>();
        renderedLightning.positionCount = lightningPoints.Count;
        renderedLightning.SetPositions(lightningPoints.ToArray());
    }

    private Vector3 randomEndPoint()
    {
        (float x, float y) randPoint = (Random.Range(-hitBoxEndPlanSize.x, hitBoxEndPlanSize.x), Random.Range(-hitBoxEndPlanSize.y, hitBoxEndPlanSize.y));
        Vector3 endPointLocal = new Vector3(randPoint.x, randPoint.y, 3);
        return transform.localToWorldMatrix.MultiplyPoint3x4(endPointLocal);
    }

    private (Branch, Branch) split(Branch toSplit)
    {
        Vector3 branch = toSplit.End - toSplit.Start;
        Vector3 midPoint = toSplit.Start + branch / 2f;

        float displacementDir = Random.Range(0, 360);
        Vector3 displacement = Quaternion.AngleAxis(displacementDir, branch.normalized) * Vector3.Cross(branch, Vector3.up).normalized * Random.Range(0, branch.magnitude * maxDisplacement);
        midPoint += displacement;        
        return (new Branch(toSplit.Start, midPoint), new Branch(midPoint, toSplit.End));
    }

    private List<Vector3> branchToVector3(List<Branch> l)
    {
        List<Vector3> res = new List<Vector3> { l[0].Start };
        foreach (Branch b in l)
        {
            res.Add(b.End);
        }
        return res;
    }
}
