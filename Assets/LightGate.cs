using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGate : MonoBehaviour {

    [SerializeField]
    private GameObject harePassageEffect;

    void OnTriggerExit(Collider other)
    {
        GameObject hare = other.gameObject;

        if (hare.tag == "Player")
        {
            ShowHarePassageEffect(hare);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject hare = other.gameObject;

        if (hare.tag == "Shadow")
        {
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
        Destroy(gameObject, 2f);
    }
}
