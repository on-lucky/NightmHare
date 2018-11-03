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

    [SerializeField]
    private float speed = 3;

    [SerializeField]
    private float curveHeight = 2;

    private QuadraticCurve curve;

    private bool recalculateCurve = true;

    void Start()
    {
        origin = transform.parent.position;
        destination = destinationTransform.position;
        GetCurve2D();
    }

    // Update is called once per frame
    void Update() {
        if (go)
        {
            if (recalculateCurve)
            {
                GetCurve2D();
                recalculateCurve = false;
            }

            Vector3 pos = transform.position;
            if (Vector3.Distance(pos, destination) <= 0.5) {
                transform.position = destination;
            } else
            {
                // Quadratic curve for the trajectory
                // Sinus wave for extra coolness
                float x = pos.x + Time.deltaTime * speed;
                float ox = x - origin.x;
                float y = curve.compute(x) + Mathf.Sin(x);
                transform.position = new Vector3(x, y, pos.z);
            }
        }

        // TODO remove this
        else
        {
            transform.position = origin;
            recalculateCurve = true;
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
        Vector3 mid = new Vector3(
                    (origin.x + destination.x) / 2,
                    Mathf.Max(origin.y, destination.y) + curveHeight,
                    (origin.z + destination.z) / 2);
        curve = new QuadraticCurve(origin, mid, destination);
    }

    class QuadraticCurve
    {
        // y = ax^2 + bx + c
        float a, b, c;

        public QuadraticCurve(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            /* Calculate a, b and c
             * A = [(Y2-Y1)(X1-X3) + (Y3-Y1)(X2-X1)]/[(X1-X3)(X2^2-X1^2) + (X2-X1)(X3^2-X1^2)]
             * B = [(Y2 - Y1) - A(X2^2 - X1^2)] / (X2-X1)
             * C = Y1 - AX1^2 - BX1
            */
            a = ((p2.y - p1.y) * (p1.x - p3.x) + (p3.y - p1.y) * (p2.x - p1.x)) /
                ((p1.x - p3.x) * (p2.x * p2.x - p1.x * p1.x) + (p2.x - p1.x) * (p3.x * p3.x - p1.x * p1.x));
            b = ((p2.y - p1.y) - a * (p2.x * p2.x - p1.x * p1.x)) / (p2.x - p1.x);
            c = p1.y - a * p1.x * p1.x - b * p1.x;
        }

        public float compute(float x)
        {
            return a * Mathf.Pow(x, 2) + b * x + c;
        }
    }
}
