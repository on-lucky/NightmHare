using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowGate : MonoBehaviour {

    public GameObject shadow;
    public float deathTime = 5f;
    public ParticleSystem ps;

    private bool gateUsed = false;
    private bool dying = false;
    private float currentTime = 0f;
    private float initialEmmission;
    

    [SerializeField]
    private float initialDelay = 2f;

    [SerializeField]
    private GameObject passageEffect;

    void Start()
    {
        initialEmmission = ps.emission.rateOverTime.constant;
        SpawnManager.instance.RegisterShadowGate(this);
    }

    void FixedUpdate()
    {
        if (dying)
        {
            currentTime += Time.deltaTime;

            if(currentTime >= deathTime)
            {
                ps.Stop();
                dying = false;
            }
            else
            {
                float multiplier = initialEmmission * ( 1 - (currentTime / deathTime));
                var emission = ps.emission;
                emission.rateOverTime = (int)multiplier;
            }
        }
    }

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
        //dying = true;
        //Destroy(gameObject, deathTime + 5f);
    }

    public void RestartGate()
    {
        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in systems)
        {
            system.Play();
        }
        gateUsed = false;
    }
}
