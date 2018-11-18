using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeManager : MonoBehaviour {
    
    [SerializeField]
    private ParticleSystem deathParticles;
    [SerializeField]
    private ParticleSystem hareParticles;
    [SerializeField]
    private GameObject armature;    

    private SkinnedMeshRenderer hareRenderer;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        hareRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Spawn();        
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.tag == "Shadow")
        {
            obj.GetComponent<ShadowController>().StopFollowing();
            Die();
        }
        else if (obj.tag == "hazard")
        {
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
        StartCoroutine(waitAndLoad(3f));
    }

    IEnumerator waitAndLoad(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Spawn(){
        GetComponent<PlayerController>().enabled = false;
        hareRenderer.enabled = false;
        StartCoroutine(waitAndEnable(1f));        
    }

    IEnumerator waitAndEnable(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        hareRenderer.enabled = true;
        armature.SetActive(true);
        GetComponent<PlayerController>().enabled = true;
        hareParticles.Play();
    }
}
