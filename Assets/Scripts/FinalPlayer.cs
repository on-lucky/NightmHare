using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPlayer : MonoBehaviour {

    Animator animator;
    PlayerController controller;
    Dashing dashing;
    Digger digger;

    bool isPlaying = false;

    [SerializeField] float hareSpeed = 0.025f;
    [SerializeField] float runDuration = 3;
    [SerializeField] GameObject endMenu;
    float currentTime = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        dashing = GetComponent<Dashing>();
        digger = GetComponent<Digger>();
    }

    private void FixedUpdate()
    {
        if (isPlaying)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= runDuration)
            {
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Final");
                StartCoroutine(ShowMenu(4f));
            } else
            {
                transform.Translate(new Vector3(0, 0, hareSpeed));
                float ratio = Mathf.Abs(hareSpeed) / 0.18f; // Magic numbers ???
                animator.SetFloat("Speed", ratio * 4); // L'animation est trop lente donc speed it up a bit (x4)
            }
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

        Camera.main.GetComponent<CameraPathFollower>().Follow();
        StartCoroutine(StartAnimation(delay));
    }

    IEnumerator StartAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        CameraFollower.instance.enabled = true;
        isPlaying = true;
        currentTime = 0;
    }

    IEnumerator ShowMenu(float delay)
    {
        yield return new WaitForSeconds(delay);

        endMenu.SetActive(true);
    }

}
