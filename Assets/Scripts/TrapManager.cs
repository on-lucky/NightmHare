using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour {
    void OnTriggerEnter(Collider other)
    {
        GameObject shadow = other.gameObject;

        if (shadow.tag == "Shadow")
        {
            shadow.GetComponent<ShadowController>().TrapShadow();            
            Destroy(this.gameObject);
        }
    }
}
