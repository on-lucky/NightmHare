using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPlayer : MonoBehaviour {

    Animator animator;
    PlayerController controller;
    Dashing dashing;
    Digger digger;
    [SerializeField] ParticleSystem finalParticle;
    GameObject armature;
    SkinnedMeshRenderer renderer;
    GameObject particle;

    bool isWalking = false;

    [SerializeField] float hareSpeed = 0.025f;
    [SerializeField] float runDuration = 3;
    [SerializeField] GameObject endMenu;
    float currentTime = 0;

    bool isFading = false;
    float fadeSpeed = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        dashing = GetComponent<Dashing>();
        digger = GetComponent<Digger>();
        armature = transform.Find("Armature").gameObject;
        renderer = transform.Find("Plane").GetComponent<SkinnedMeshRenderer>();
        particle = transform.Find("Particles").Find("Particle System").gameObject;
    }

    private void FixedUpdate()
    {
        if (isWalking)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= runDuration)
            {
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Final");
                StartCoroutine(StartFinalParticles());
                isWalking = false;
                isFading = true;
                StartCoroutine(ShowMenu(4f));
            } else
            {
                transform.Translate(new Vector3(0, 0, hareSpeed));
                float ratio = Mathf.Abs(hareSpeed) / 0.18f; // Magic numbers ???
                animator.SetFloat("Speed", ratio * 4); // L'animation est trop lente donc speed it up a bit (x4)
            }
        }
        else if (isFading)
        {
            Color color = renderer.material.color;
            renderer.material.color = new Color(color.r, color.g, color.b, Mathf.Max(0, color.a - fadeSpeed * Time.deltaTime));
        }
    }

    public void Play(float delay)
    {
        // Disable all controls
        controller.enabled = false;
        dashing.enabled = false;
        digger.enabled = false;
        animator.SetFloat("Speed", 0);

        // Disable camera effects
        CameraFollower.instance.enabled = false;
        Camera.main.GetComponent<CameraSlider>().enabled = false;

        // Look to the left
        if (GetComponent<OrientationManager>().IsLookingRight)
            GetComponent<OrientationManager>().LookTo(false);

        // Caluclate fade speed fade
        float alpha = renderer.material.color.a;
        fadeSpeed = alpha / finalParticle.main.duration;

        Camera.main.GetComponent<CameraPathFollower>().Follow();
        StartCoroutine(StartAnimation(delay));
    }

    IEnumerator StartAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        CameraFollower.instance.enabled = true;
        isWalking = true;
        currentTime = 0;
    }
    
    IEnumerator StartFinalParticles()
    {
        yield return new WaitForSeconds(1);
        finalParticle.Play();
        armature.SetActive(false);
        particle.SetActive(false);
    }

    IEnumerator ShowMenu(float delay)
    {
        yield return new WaitForSeconds(delay);

        endMenu.SetActive(true);
        ScoreManager.instance.StopCount();
    }
}
