using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour {
    GameObject hare;
    ShadowController shadowController;
	// Use this for initialization
	void Start () {
        hare = GameObject.Find("Hare");
        shadowController = (ShadowController)hare.GetComponent(typeof(ShadowController));
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnTriggerEnter(Collider other)
    {
        GameObject shadow = other.gameObject;

        if (shadow.tag == "Shadow")
        {
            shadow.GetComponent<ShadowController>().TrapShadow();            
            Destroy(this.gameObject);
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "shadow")
        {
            shadowController.TrapShadow();
            //Destroy(this);
        }
    }*/
}
