using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGate : MonoBehaviour {

    private bool gateUsed = false;

    void Start()
    {
        SpawnManager.instance.RegisterLightGate(this);
    }

    [SerializeField]
    private GameObject harePassageEffect;

    void OnTriggerExit(Collider other)
    {
        GameObject hare = other.gameObject;

        if (hare.tag == "Player" && !gateUsed)
        {
            ShowHarePassageEffect(hare);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject hare = other.gameObject;

        if (hare.tag == "Shadow")
        {
            gateUsed = true;
            ShowShadowPassageEffect(hare);
        }
    }

    private void ShowHarePassageEffect(GameObject hare)
    {
        if (harePassageEffect)
        {
            Instantiate(harePassageEffect, hare.transform.position, hare.transform.rotation);
        }
    }

    private void ShowShadowPassageEffect(GameObject shadow)
    {
        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem system in systems)
        {
            system.Stop();
        }
        shadow.GetComponent<ShadowController>().Die();
        //Destroy(gameObject, 2f);
    }

    public void RestartGate()
    {
        gateUsed = false;
        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in systems)
        {
            system.Play();
        }
    }
}
