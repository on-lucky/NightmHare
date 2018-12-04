using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour {

    public ParticleSystem collectionEffect;

    private void OnTriggerEnter(Collider other)
    {
        CarrotCollector collector = other.gameObject.GetComponent<CarrotCollector>();
        if (collector)
        {
            collector.Collect(this);

            AudioManager.instance.PlaySound("CoinGrab", 0.5f, 0, false);

            if (collectionEffect)
            {
                Instantiate(collectionEffect, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
