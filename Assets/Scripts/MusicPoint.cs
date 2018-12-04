using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPoint : MonoBehaviour {


    public string musicName = "";
    public float startingTime = 0;
    public string layerToCheck = "Player";
    public float transitionTime = 3f;
    public float volume = 1f;

    private bool used = false;

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.tag == layerToCheck && !used)
        {
            Debug.Log("Music Point {" + musicName + "} reached!");
            SoundStateManager.instance.ChangeMusic(musicName, volume, transitionTime, startingTime);
            used = true;
        }
    }

    public void ResetUsed()
    {
        used = false;
    }
}
