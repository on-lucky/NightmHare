using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    [SerializeField]
    private Vector3 spawnPosition;
    [SerializeField]
    private bool isHazard = false;

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.tag == "Player" && spawnPosition != null)
        {
            if (isHazard)
            {
                SpawnManager.instance.SetSpikeSpawnPoint(this.transform.position);
            }
            else
            {
                SpawnManager.instance.SetSpawnPoint(this.transform.position);
            }
        }
    }
}
