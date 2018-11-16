using UnityEngine;

public class OrbKey : MonoBehaviour {

    // Start moving towards destination
    private bool go = false;

    private Vector3 origin;

    // The burrow's LockManager
    [SerializeField]
    private LockManager lockManager;
    private Vector3 destination;

    // The speed to reach the burrow
    [SerializeField]
    private float speed = 3;

    // The height of the curve
    [SerializeField]
    private float curveHeight = 2;

    // The trajectory
    private QuadraticCurve trajectory;

    // The spiral
    [SerializeField]
    private Helix helix = new Helix();

    void Start()
    {
        origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        destination = lockManager.transform.position;
        GetCurve2D();
    }

    // Update is called once per frame
    void Update() {
        if (go)
        {
            if (trajectory == null)
            {
                GetCurve2D();
            }

            Vector3 pos = transform.position;
            if (Mathf.Abs(pos.x - destination.x) <= 0.5) {
                lockManager.Unlock();
                Destroy(gameObject);
            } else
            {
                // Quadratic curve for the trajectory
                // Spiral for extra coolness
                float direction = pos.x < destination.x ? 1 : -1;
                float x = pos.x + direction * Time.deltaTime * speed;
                float ox = x - origin.x;
                float y = trajectory.compute(x) + helix.computeY(ox);
                float z = origin.z + helix.computeZ(ox);
                transform.position = new Vector3(x, y, z);
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

    [System.Serializable]
    class Helix
    {
        [SerializeField]
        float frequency = 1, amplitude = 1;

        public float computeY(float x)
        {
            return amplitude * Mathf.Sin(frequency * x);
        }

        public float computeZ(float x)
        {
            return amplitude * Mathf.Cos(frequency * x);
        }
    }
}
