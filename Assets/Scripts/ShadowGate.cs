using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowGate : MonoBehaviour {

    public GameObject shadow;

    [SerializeField]
    private float initialDelay = 2f;

    void OnTriggerExit(Collider other)
    {
        GameObject hare = other.gameObject;

        if (hare.tag == "Player") {
            hare.GetComponent<StateRecorder>().StartRecording();

            GameObject shadowObject = Instantiate(shadow, this.transform);

            shadowObject.GetComponent<ShadowController>().setObjToFollow(hare);
            shadowObject.GetComponent<ShadowController>().StartFollowing(initialDelay);

            GetComponent<BoxCollider>().enabled = false;
        }
       
    }
}
