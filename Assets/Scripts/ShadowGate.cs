using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowGate : MonoBehaviour {

    public GameObject shadow;
	
    void OnTriggerExit(Collider other)
    {
        GameObject hare = other.gameObject;

        if (hare.tag == "Player") {
            hare.GetComponent<StateRecorder>().StartRecording();

            GameObject shadowObject = Instantiate(shadow, this.transform);

            shadowObject.GetComponent<ShadowController>().ObjToFollow = hare;
            shadowObject.GetComponent<ShadowController>().StartFollowing(2f);

            GetComponent<BoxCollider>().enabled = false;
        }
       
    }
}
