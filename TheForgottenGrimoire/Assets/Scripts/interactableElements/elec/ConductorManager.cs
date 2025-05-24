using UnityEngine;
using System.Collections.Generic;

using Circuit = System.Collections.Generic.HashSet<InteractableConductor>;

public class ConductorManager : MonoBehaviour
{
    private List<Circuit> circuits;
    private Dictionary<Circuit, HashSet<InteractorElec>> sources;

    private void Awake()
    {
        circuits = new List<Circuit>();
        sources = new Dictionary<Circuit, HashSet<InteractorElec>>();
        computeAllCircuits();
        //printCircuitsAndSrc();
    }

    public void srcCollidedWithConductor(InteractorElec src, InteractableConductor conductor)
    {
        Circuit c = getCircuitOfConductor(conductor);
        sources[c].Add(src);
        propagateToCircuit(src, c, true);
    }

    public void srcLeavingConductor(InteractorElec src, InteractableConductor conductor)
    {
        Circuit c = getCircuitOfConductor(conductor);
        sources[c].Remove(src);
        propagateToCircuit(src, c, false);
    }

    public void ConductorCollidingWithConductor(InteractableConductor conductor1, InteractableConductor conductor2)
    {        
        if (!getCircuitOfConductor(conductor2).Equals(getCircuitOfConductor(conductor1)))
        {
            mergeAndRecomputeCircuit(conductor1, conductor2);
        }
        //printCircuitsAndSrc();
    }

    public void conductorLeavingConductor(InteractableConductor conductor1, InteractableConductor conductor2)
    {        
        if (getCircuitOfConductor(conductor1).Equals(getCircuitOfConductor(conductor2))) 
        {
            //print($"{conductor1.name} and {conductor2.name} had same circuit, need to recompute circuits to split it");
            Circuit c = getCircuitOfConductor(conductor1);
            List<InteractableConductor> toSplit = new List<InteractableConductor>();
            foreach (InteractableConductor conductor in c)
            {
                toSplit.Add(conductor);
            }
            foreach (InteractableConductor conductor in toSplit)
            {
                c.Remove(conductor);
            }
            cleanSourcesAndCircuit();
            computeCircuitFor(toSplit);
        }
        printCircuitsAndSrc();        
    }

    private void cleanSourcesAndCircuit()
    {
        List<Circuit> toRemove = new List<Circuit>();
        foreach (Circuit c in circuits)
        {
            if (c.Count == 0) toRemove.Add(c);
        }
        foreach (Circuit c in toRemove)
        {
            circuits.Remove(c);
            if (sources.ContainsKey(c)) sources.Remove(c);
        }
    }

    private Circuit createCircuit()
    {
        Circuit circuit = new Circuit();
        circuits.Add(circuit);
        sources.Add(circuit, new HashSet<InteractorElec>());
        return circuit;
    }

    private void deleteCircuit(Circuit c)
    {
        circuits.Remove(c);
        sources.Remove(c);
    }

    private void propagateToCircuit(InteractorElec elec, Circuit c, bool isEntering)
    {
        foreach (InteractableConductor conductor in c)
        {
            conductor.Power += isEntering ? elec.Power : -elec.Power;
        }
    }

    private void resetPowerInCircuit(in Circuit c)
    {
        foreach (InteractableConductor cond in c)
        {
            cond.Power = 0;
        }
    }

    private void computeAllCircuits()
    {
        computeCircuitFor(GameObject.FindObjectsByType<InteractableConductor>(FindObjectsSortMode.None));   
    }

    private void mergeAndRecomputeCircuit(InteractableConductor conductor1, InteractableConductor conductor2)
    {
        HashSet<InteractableConductor> conductors = new HashSet<InteractableConductor>();
        conductors.UnionWith(getCircuitOfConductor(conductor1));
        conductors.UnionWith(getCircuitOfConductor(conductor2));
        computeCircuitFor(conductors);
    }

    private Circuit mergeCircuits(Circuit c1, Circuit c2)
    {
        //print($"merging {c1} and {c2}");        

        Circuit c12 = new Circuit();
        c12.UnionWith(c1);
        c12.UnionWith(c2);

        HashSet<InteractorElec> src12 = new HashSet<InteractorElec>();
        src12.UnionWith(sources[c1]);
        src12.UnionWith(sources[c2]);

        deleteCircuit(c1);
        deleteCircuit(c2);

        circuits.Add(c12);
        sources.Add(c12, src12);
        return c12;
    }

    private void recomputePowerCircuits(in IEnumerable<Circuit> circuits)
    {
        foreach (Circuit c in circuits)
        {
            resetPowerInCircuit(c);
            foreach (InteractorElec src in sources[c])
            {
                propagateToCircuit(src, c, true);
            }
        }
    }

    private void computeCircuitFor(IEnumerable<InteractableConductor> conductors)
    {
        HashSet<Circuit> modifiedCircuit = new HashSet<Circuit>();
        foreach (InteractableConductor conductor in conductors)
        {
            //print("computing circuit for " + conductor.name);
            Circuit circuit = getCircuitOfConductor(conductor) ?? createCircuit();
            //print("found circuit size: " + circuit.Count);
            circuit.Add(conductor);

            var neighbors = conductor.getNeighbors();
           //print("neighbors sources found: " + neighbors.Item2.Count);

            //print("sources already existing: " + sources[circuit].Count);
            sources[circuit].UnionWith(neighbors.Item2);
            //print("sources found size: " + sources[circuit].Count);

            List<InteractableConductor> neighborsCond = neighbors.Item1;
            //print("neighbors conductor found: " + neighborsCond.Count);
            foreach (InteractableConductor n in neighborsCond)
            {
                Circuit nCircuit = getCircuitOfConductor(n);
                if (nCircuit != null)
                {
                    circuit = mergeCircuits(circuit, nCircuit);
                }
                else
                {
                    circuit.Add(n);
                }
            }
            modifiedCircuit.Add(circuit);
            //print("modified circuit: " + modifiedCircuit);
        }
        modifiedCircuit.IntersectWith(circuits);
        recomputePowerCircuits(modifiedCircuit);
    }

    private Circuit getCircuitOfConductor(InteractableConductor conductor)
    {
        Circuit res = null; 
        foreach (Circuit circuit in circuits)
        {
            if (circuit.Contains(conductor))
            {
                res = circuit;
                break;
            }
        }
        return res;
    } 

    private void printCircuitsAndSrc()
    {
        string msg = "\n[DEBUG] detected circuits in scene:\n";
        for (int i = 0; i < circuits.Count; i++)
        {
            msg += $" - circuit {i}:\n";
            foreach (InteractableConductor c in circuits[i])
            {
                msg += $"    - {c.name}, power level: {c.Power}\n";
            }
            msg += $"    - sources: {sources[circuits[i]]}, size: {sources[circuits[i]].Count}\n";
        }
        print(msg);
    }
}
