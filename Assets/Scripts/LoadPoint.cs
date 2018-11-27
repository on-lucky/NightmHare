using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPoint : MonoBehaviour {

    public GameObject[] loadables;
    public GameObject[] unloadables;

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.tag == "Player")
        {
            foreach(GameObject o in loadables)
            {
                o.SetActive(true);
            }

            foreach (GameObject o in unloadables)
            {
                o.SetActive(false);
            }
        }
    }
}
