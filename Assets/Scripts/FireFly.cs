using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFly : MonoBehaviour {

    public float LoopSpeed = 5f;
    public float loopWidth = 4f;
    public float loopHeight = 2f;
    public float intialDelay = 0f;

    private float intitialHeight;
    private float intitialPosX;

    private float timer = 0;

    // Use this for initialization
    void Start () {
        intitialHeight = transform.position.y;
        intitialPosX = transform.position.x;
        timer = intialDelay;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime * LoopSpeed;
        float y = loopHeight * Mathf.Sin(timer * 2) + intitialHeight;
        float x = loopWidth * Mathf.Cos(timer) + intitialPosX;
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
