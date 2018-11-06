using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour {

    [SerializeField]
    Transform spawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject player = other.gameObject;
            player.transform.position = spawn.position;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
