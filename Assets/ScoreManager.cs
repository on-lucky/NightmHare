using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager instance;

    private float timer = 0f;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one ScoreManager in the scene");
        }
        else
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
	}

    public string GetTime()
    {
        return timer.ToString("0.00");
    }
}
