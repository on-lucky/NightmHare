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

            if (!gateUsed) {
                ShowPassageEffect(hare);
                hare.GetComponent<StateRecorder>().StartRecording();

                GameObject shadowObject = Instantiate(shadow, hare.transform.position, hare.transform.rotation);

                shadowObject.GetComponent<ShadowController>().setObjToFollow(hare);
                shadowObject.GetComponent<ShadowController>().StartFollowing(initialDelay);
                DestroyGate();
                gateUsed = true;
            }
        }
       
    }

    private void ShowPassageEffect(GameObject hare)
    {
        if (passageEffect)
        {
            Instantiate(passageEffect, hare.transform.position, hare.transform.rotation);
        }
    }

    private void DestroyGate()
    {
        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in systems)
        {
            system.Stop();
        }
        Destroy(gameObject, 3f);
    }
}
