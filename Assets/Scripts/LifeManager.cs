using System;
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
    private ParticleSystem spawnParticles;
    [SerializeField]
    private float timeLossRatioOnSpike = 0.9f;

    [SerializeField]
    private GameObject armature;

    public event EventHandler HareDied;

    private SkinnedMeshRenderer hareRenderer;

    private Rigidbody rb;

    private bool isSpike = false;
    private bool shadowExists = false;
    private Vector3 spawnPos;
    private float spawnDelay;
    private bool dead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        hareRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Spawn();        
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (!dead)
        {
            if (obj.tag == "Shadow")
            {
                ShadowController.instance.StopFollowing();
                ShadowController.instance.Die();
                isSpike = false;
                spawnPos = SpawnManager.instance.GetSpawnPoint();
                Die();
            }
            else if (obj.tag == "hazard")
            {
                if (ShadowController.instance != null)
                {
                    ShadowController.instance.StopFollowing();
                    ShadowController.instance.Die();
                    shadowExists = true;
                }
                else
                {
                    shadowExists = false;
                }

                isSpike = true;
                spawnPos = SpawnManager.instance.GetSpikeSpawnPoint();
                Die();
            }
        }
    }

    private void Die()
    {
        // Classic code
        dead = true;
        rb.isKinematic = true;
        GetComponent<PlayerController>().SetCurrentSpeed(0);
        GetComponent<PlayerController>().enabled = false;
        GetComponent<StateRecorder>().StopRecording();

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        if (deathParticles)
        {
            ParticleSystem death = Instantiate(deathParticles, this.transform);
            death.Play();
        }

        // Delete all the traps currently active
        foreach (GameObject trap in GameObject.FindGameObjectsWithTag("trap"))
        {
            Destroy(trap);
        }

        // New code with event handling instead
        HareDied?.Invoke(this, new EventArgs());

        StartCoroutine(waitAndRespawn(3f)); 
    }

    IEnumerator waitAndRespawn(float timeToWait)
    {
        spawnDelay = GetComponent<StateRecorder>().GetDelay() * timeLossRatioOnSpike;
        GetComponent<StateRecorder>().ResetMomentStates();
        yield return new WaitForSeconds(timeToWait);
        Spawn(true);
    }

    IEnumerator waitAndLoad(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Spawn(bool shouldTeleport = false){

        if (shouldTeleport)
        {
            transform.position = spawnPos;
        }
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        if (!isSpike)
        {
            SpawnManager.instance.RestartGates();
        }
        
        armature.SetActive(false);
        GetComponent<PlayerController>().SetZPos(transform.position.z);
        GetComponent<PlayerController>().enabled = false;
        GetComponent<Climber>().ForceLeaveWall();
        hareRenderer.enabled = false;
        spawnParticles.Play();
        StartCoroutine(waitAndEnable(1f));
    }

    IEnumerator waitAndEnable(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        dead = false;
        hareRenderer.enabled = true;
        armature.SetActive(true);
        GetComponent<PlayerController>().enabled = true;
        hareParticles.Play();
        if (isSpike && shadowExists)
        {
            GetComponent<StateRecorder>().StartRecording();
            GetComponent<ShadowFactory>().SpawnShadow(spawnPos, spawnDelay);
        }
    }

    
}
