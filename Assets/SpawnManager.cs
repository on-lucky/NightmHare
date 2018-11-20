using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public static SpawnManager instance;

    [SerializeField]
    private Vector3 spawnPos;
    [SerializeField]
    private Vector3 spikeSpawnPos;

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

    public void SetSpawnPoint(Vector3 spawnPoint)
    {
        spawnPos = spawnPoint;
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPos;
    }

    public void SetSpikeSpawnPoint(Vector3 spawnPoint)
    {
        spikeSpawnPos = spawnPoint;
    }

    public Vector3 GetSpikeSpawnPoint()
    {
        return spikeSpawnPos;
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
