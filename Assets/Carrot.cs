using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        CarrotCollector collector = other.gameObject.GetComponent<CarrotCollector>();
        if (collector)
        {
            collector.Collect(this);
            Destroy(gameObject);
        }
    }
}
