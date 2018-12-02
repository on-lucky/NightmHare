using UnityEngine;

public class CameraPathFollower : MonoBehaviour {

    [SerializeField] float speed = 5;
    [SerializeField] GizmosPath gizmosPath;

    private Vector3[] path;
    private int target = 0;

    private bool isFollowing = false;

    // Update is called once per frame
    void Update () {
		if (isFollowing && path.Length > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[target], Time.deltaTime * speed);
            float distance = Vector3.Distance(transform.position, path[target]);
            if (distance <= 0.01)
            {
                // We reached the target
                if (target < path.Length -1)
                {
                    // Go to next target
                    target++;
                }
                else
                {
                    // Or stop following if we are at the end
                    isFollowing = false;
                }
            }
        }
	}

    public void Follow(bool follow = true)
    {
        isFollowing = follow;
        target = 0;
        path = gizmosPath.GetPath();
        CameraFollower.instance.EnableFollowing(!follow);
    }
}
