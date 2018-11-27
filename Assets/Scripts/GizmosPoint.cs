using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosPoint : MonoBehaviour {

    [SerializeField] private float radius = 0.5f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
