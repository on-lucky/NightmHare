using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowButton : MonoBehaviour {

    [SerializeField]
    private Burrow burrow;                  // The burrow it unlocks

    [SerializeField]
    private bool pressed = false;           // Whether the button is pressed or not

    private float pressedRatio = 0.5f;
    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Vector3 pressedPosition;
    private Vector3 pressedScale;

    void Start()
    {
        float height = GetComponent<Collider>().bounds.size.y;
        initialPosition = transform.position;
        initialScale = transform.localScale;
        pressedPosition = new Vector3(initialPosition.x, initialPosition.y - pressedRatio*height/2, initialPosition.z);
        pressedScale = new Vector3(initialScale.x, pressedRatio*initialScale.y, initialScale.z);
    }

    void Update () {
        if (pressed)
        {
            // Put the button in the pressed position
            transform.position = pressedPosition;
            transform.localScale = pressedScale;
            
            burrow.Unlock();
        }
        else
        {
            // Reset the button position
            transform.position = initialPosition;
            transform.localScale = initialScale;

            burrow.Lock();
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.tag == "Player")
        {
            Debug.Log("Pressed");
            pressed = true;
        }
    }
}
