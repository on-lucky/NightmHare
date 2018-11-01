using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingKey : MonoBehaviour {

    /*
     * A FloatingKey is an object that starts from its parent and
     * floats to its destination following a quadratic curve
     */

    // Start moving towards destination
    [SerializeField]
    private bool go = false;

    private Vector3 origin;

    [SerializeField]
    private Transform destinationTransform;

    private Vector3 destination;

    private float distance;

    [SerializeField]
    private float speed = 3;

    [SerializeField]
    private float curveHeight = 2;

    // Quadratic equation y = ax^2 + bx + c
    private float a, b, c;

    void Start()
    {
        origin = transform.parent.position;
        destination = destinationTransform.position;
        GetCurve2D();
    }

    // Update is called once per frame
    void Update () {
		if (go)
        {
            Vector3 pos = transform.position;
            if (Vector3.Distance(pos, destination) <= 0.5) {
                transform.position = destination;
            } else
            {
                float dx = Time.deltaTime * speed;
                float nx = pos.x + dx;
                float ny = a*Mathf.Pow(nx, 2) + b*nx + c;
                transform.position = new Vector3(nx, ny, pos.z);
            }
        }

        // TODO remove this
        else
        {
            transform.position = origin;
        }
	}

    // Starts the movement
    public void Go(Vector3 destination)
    {
        this.destination = destination;
        GetCurve2D();
        go = true;
    }

    private void GetCurve2D()
    {
        // Solve the quadratic curve that passes
        // through both the origin and the destination

        // Solve with three points: origin, destination and
        // a point in between higher than the highest point
        Vector3 p1 = origin;
        Vector3 p2 = destination;
        Vector3 p3 = new Vector3(
            (p1.x + p2.x)/2,
            Mathf.Max(p1.y, p2.y) + curveHeight,
            (p1.z + p2.z) / 2
            );

        // Calculate A, B, and C.
        /*
        A = [(Y2-Y1)(X1-X3) + (Y3-Y1)(X2-X1)]/[(X1-X3)(X2^2-X1^2) + (X2-X1)(X3^2-X1^2)]

        B = [(Y2 - Y1) - A(X2^2 - X1^2)] / (X2-X1)

        C = Y1 - AX1^2 - BX1
        */

        a = ((p2.y - p1.y) * (p1.x - p3.x) + (p3.y - p1.y) * (p2.x - p1.x)) /
            ((p1.x - p3.x) * (p2.x * p2.x - p1.x * p1.x) + (p2.x - p1.x) * (p3.x * p3.x - p1.x * p1.x));
        b = ((p2.y - p1.y) - a * (p2.x * p2.x - p1.x * p1.x)) / (p2.x - p1.x);
        c = p1.y - a * p1.x * p1.x - b * p1.x;

    }
}
