using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundStateManager : MonoBehaviour {

    public static SoundStateManager instance;

    private string currentMusic = "NightDreams";
    private string nextMusic = "NightDreams";

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one SoundStateManager in the scene");
        }
        else
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        AudioManager.instance.PlaySound("NightDreams", 1f, 35f);
    }

    public void ChangeMusic(string clipName, float volume = 1f, float transitionTime = 0f, float time = 0)
    {
        AudioManager.instance.TransitionSound(currentMusic, 0f, transitionTime);
        AudioManager.instance.TransitionSound(clipName, volume, transitionTime, time);
        currentMusic = clipName;
    }

    public string GetCurrentSound()
    {
        return currentMusic;
    }
}
