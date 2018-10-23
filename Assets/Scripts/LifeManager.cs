using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeManager : MonoBehaviour {

    [SerializeField]
    private string sceneName = "";
    [SerializeField]
    private ParticleSystem deathParticles;
    [SerializeField]
    private ParticleSystem hareParticles;
    [SerializeField]
    private GameObject armature;

    private SkinnedMeshRenderer renderer;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Spawn();
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject shadow = other.gameObject;

        if (shadow.tag == "Shadow")
        {
            shadow.GetComponent<ShadowController>().StopFollowing();
            Die();
        }
    }

    private void Die()
    {
        rb.isKinematic = true;
        GetComponent<PlayerController>().enabled = false;
        GetComponent<StateRecorder>().StopRecording();


        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        if (deathParticles)
        {
            Instantiate(deathParticles, this.transform);
            deathParticles.Play();
        }
        StartCoroutine(waitAndLoad(2f));
    }

    IEnumerator waitAndLoad(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void Spawn(){
        GetComponent<PlayerController>().enabled = false;
        renderer.enabled = false;
        StartCoroutine(waitAndEnable(1f));
    }

    IEnumerator waitAndEnable(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        renderer.enabled = true;
        armature.SetActive(true);
        GetComponent<PlayerController>().enabled = true;
        hareParticles.Play();
    }
}
