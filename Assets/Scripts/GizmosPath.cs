using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosPath : MonoBehaviour {

    [SerializeField] private Color color;
    private Vector3[] path;

    private void Start()
    {
        GizmosPoint[] points = GetComponentsInChildren<GizmosPoint>();
        path = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            path[i] = points[i].transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        GizmosPoint[] points = GetComponentsInChildren<GizmosPoint>();
        if (points.Length > 1)
        {
            Vector3 from = points[0].transform.position;
            for (int i = 1; i < points.Length; i++)
            {
                Vector3 pos = points[i].transform.position;
                Gizmos.DrawLine(from, pos);
                from = pos;
            }
        }
    }

    public Vector3[] GetPath()
    {
        return path;
    }
}
