using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour {

    public float floatingHeight = 0.2f;

    public float floatingSpeed = 0.5f;

    private float intitialHeight;

    private float timer = 0;

	// Use this for initialization
	void Start () {
        intitialHeight = transform.position.y;
        timer = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        timer += Time.deltaTime * floatingSpeed;
        transform.position = new Vector3(transform.position.x, floatingHeight * Mathf.Sin(timer) + intitialHeight, transform.position.z);
    }
}
