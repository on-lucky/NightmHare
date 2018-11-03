using UnityEngine;

public class OrbKey : MonoBehaviour {

    // Start moving towards destination
    [SerializeField]
    private bool go = false;

    private Vector3 origin;

    // The burrow to unlock
    [SerializeField]
    private Burrow burrow;
    private Vector3 destination;

    // The speed to reach the burrow
    [SerializeField]
    private float speed = 3;

    // The height of the curve
    [SerializeField]
    private float curveHeight = 2;

    // The trajectory
    private QuadraticCurve trajectory;

    void Start()
    {
        origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        destination = burrow.transform.position;
        GetCurve2D();
    }

    // Update is called once per frame
    void Update() {
        if (go)
        {
            Vector3 pos = transform.position;
            if (Vector3.Distance(pos, destination) <= 0.5) {
                burrow.Unlock();
                Destroy(gameObject);
            } else
            {
                // Quadratic curve for the trajectory
                // Sinus wave for extra coolness
                float x = pos.x + Time.deltaTime * speed;
                float ox = x - origin.x;
                float y = trajectory.compute(x) + Mathf.Sin(x);
                transform.position = new Vector3(x, y, pos.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            go = true;
        }
    }

    private void GetCurve2D()
    {
        Vector3 mid = new Vector3(
                    (origin.x + destination.x) / 2,
                    Mathf.Max(origin.y, destination.y) + curveHeight,
                    (origin.z + destination.z) / 2);
        trajectory = new QuadraticCurve(origin, mid, destination);
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
