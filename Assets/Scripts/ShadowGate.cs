using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowGate : MonoBehaviour {

    public GameObject shadow;

    private bool gateUsed = false;

    [SerializeField]
    private float initialDelay = 2f;

    [SerializeField]
    private GameObject passageEffect;

    void OnTriggerExit(Collider other)
    {
        GameObject hare = other.gameObject;

        if (hare.tag == "Player") {
            ShowPassageEffect(hare);

            if (!gateUsed) {
                hare.GetComponent<StateRecorder>().StartRecording();

                GameObject shadowObject = Instantiate(shadow, this.transform);

                shadowObject.GetComponent<ShadowController>().setObjToFollow(hare);
                shadowObject.GetComponent<ShadowController>().StartFollowing(initialDelay);
                gateUsed = true;
            }
        }
       
    }

    private void ShowPassageEffect(GameObject hare)
    {
        float direction = -1;
        if(hare.transform.position.x > transform.position.x)
        {
            direction = 1;
        }

        if (passageEffect)
        {
            Instantiate(passageEffect, hare.transform.position, hare.transform.rotation);
        }
    }
}
