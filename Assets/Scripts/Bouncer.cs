using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour {

    public float force = 500;
    public float bounceDuration = 0.5f;
    public float minShroomSize = 0.7f;

    private ParticleSystem spores;
    private Transform shroomTransform;
    private bool bouncing = false;
    private float startShroomSize;
    private float a;
    private float currentTime = 0f;

    // Use this for initialization
    void Start () {
        spores = this.gameObject.GetComponentInChildren<ParticleSystem>();
        spores.Pause();
        shroomTransform = transform.root;
        startShroomSize = shroomTransform.localScale.y;
        FindA();
    }
	
	// Update is called once per frame
	void Update () {
        if (bouncing)
        {
            currentTime += Time.deltaTime;
            UpdateScale(currentTime);
        }
	}

    private void FindA()
    {
        a = (startShroomSize - (startShroomSize * minShroomSize)) / (Mathf.Pow(bounceDuration/2, 2));
    }

    private void UpdateScale(float time)
    {
        float y = (a * Mathf.Pow(time - (bounceDuration / 2), 2)) + (startShroomSize * minShroomSize);

        if (y > startShroomSize)
        {
            y = startShroomSize;
            bouncing = false;
        }

        shroomTransform.localScale = new Vector3(shroomTransform.localScale.x, y, shroomTransform.localScale.z);
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.name == "Hare")
        {
            Vector3 velocity = c.gameObject.GetComponent<Rigidbody>().velocity;
            velocity.y = 0;
            c.gameObject.GetComponent<Rigidbody>().velocity = velocity;            
            c.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0)  * force);
            c.gameObject.GetComponent<Jumper>().resetAirJumpCount();           
            spores.Play();
            bouncing = true;
            currentTime = 0;
            c.gameObject.GetComponent<Jumper>().DisableJump(0.5f);
        }
    }
}
