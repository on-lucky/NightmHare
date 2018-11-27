using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalAnimation : MonoBehaviour {

    [SerializeField] private float delay = 2;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<FinalPlayer>();
        if (player)
        {
            player.Play(delay);
        }
    }
}
