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
    private float spawnDepth;
    private float spawnDelay;
    private bool dead = false;
    private bool invincible = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        hareRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Spawn();        
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;          

        if (!dead && !invincible)
        {
            if (obj.tag == "Shadow")
            {
                RefreshAbilities();
                ShadowController.instance.StopFollowing();
                ShadowController.instance.Die();
                isSpike = false;
                spawnPos = SpawnManager.instance.GetSpawnPoint().transform.position;
                spawnDepth = SpawnManager.instance.GetSpawnPoint().spawnDepth;
                Die();
            }
            else if (obj.tag == "hazard")
            {
                if (ShadowController.instance != null)
                {
                    RefreshAbilities();
                    ShadowController.instance.StopFollowing();
                    ShadowController.instance.Die();
                    shadowExists = true;
                }
                else
                {
                    shadowExists = false;
                }

                isSpike = true;
                spawnPos = SpawnManager.instance.GetSpikeSpawnPoint().transform.position;
                spawnDepth = SpawnManager.instance.GetSpikeSpawnPoint().spawnDepth;
                Die();
            }
        }
    }

    private void Die()
    {
        // Classic code
        dead = true;
        rb.isKinematic = true;
        PlayerController pc = GetComponent<PlayerController>();
        pc.SetCurrentSpeed(0);
        pc.enabled = false;
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

        // New code with event handling instead
        HareDied?.Invoke(this, new EventArgs());

        StartCoroutine(waitAndRespawn(3f)); 
    }

    private void RefreshAbilities()
    {
        PlayerController pc = GetComponent<PlayerController>();

        // Stop sprinting and make sprint available again                
        pc.StopSprinting_();
        pc.ResetSprintCooldown();

        // Delete all the traps currently active
        foreach (GameObject trap in GameObject.FindGameObjectsWithTag("trap"))
        {
            Destroy(trap);
        }

        // Make trap available again
        pc.ResetTrapCooldown();
    }

    IEnumerator waitAndRespawn(float timeToWait)
    {
        spawnDelay = GetComponent<StateRecorder>().GetDelay() * timeLossRatioOnSpike;
        GetComponent<StateRecorder>().ResetMomentStates();
        yield return new WaitForSeconds(timeToWait);
        Spawn(true);
        // FIXME hardcoded magic
        if (!isSpike)
        {
            SoundStateManager.instance.ChangeMusic("NightDreams", 1, 3, 0);
        }
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
            CameraFollower.instance.SetZPos(spawnDepth);
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

    public void SetInvinsible(bool shoulBe)
    {
        invincible = shoulBe;
    }
}
