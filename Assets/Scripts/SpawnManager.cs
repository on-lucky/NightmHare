using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public static SpawnManager instance;

    [SerializeField]
    private SpawnPoint spawnPoint;    
    [SerializeField]
    private SpawnPoint spikeSpawnPoint;

    private List<ShadowGate> shadowGates;
    private List<LightGate> lightGates;

    void Awake()
    {
        if(SpawnManager.instance == null)
        {
            SpawnManager.instance = this;
        }
        else
        {
            Debug.LogError("More than one SpawnManager in the scene!");
        }
        shadowGates = new List<ShadowGate>();
        lightGates = new List<LightGate>();
    }

    public void SetSpawnPoint(SpawnPoint spawnPoint_)
    {        
        spawnPoint = spawnPoint_;
    }

    public SpawnPoint GetSpawnPoint()
    {
        return spawnPoint;
    }

    public void SetSpikeSpawnPoint(SpawnPoint spawnPoint_)
    {
        spikeSpawnPoint = spawnPoint_;
    }

    public SpawnPoint GetSpikeSpawnPoint()
    {
        return spikeSpawnPoint;
    }

    public void RegisterShadowGate(ShadowGate gate)
    {
        shadowGates.Add(gate);
    }

    public void RegisterLightGate(LightGate gate)
    {
        lightGates.Add(gate);
    }

    public void RestartGates()
    {
        foreach(ShadowGate gate in shadowGates)
        {
            gate.RestartGate();
        }
        foreach (LightGate gate in lightGates)
        {
            gate.RestartGate();
        }
    }
}
