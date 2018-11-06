using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour {

    public float force = 500;
    ParticleSystem spores;
    // Use this for initialization
    void Start () {
        spores = this.gameObject.GetComponentInChildren<ParticleSystem>();
        spores.Pause();
    }
	
	// Update is called once per frame
	void Update () {
		
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
        }
    }
}
