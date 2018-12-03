using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager instance;

    public TextMeshPro timerText;
    public TextMeshPro scoreText;

    private float timer = 0f;
    private int score = 0;
    private bool isCounting = true;

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
        score = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (isCounting)
        {
            timer += Time.deltaTime;
            UpdateEndMenu();
        }
    }

    public string GetTime()
    {
        return timer.ToString("0.00");
    }

    public void IncrementScore()
    {
        score++;
        UpdateEndMenu();
    }

    public int GetScore()
    {
        return score;
    }

    public void StopCount()
    {
        isCounting = false;
    }

    private void UpdateEndMenu()
    {
        timerText.text = "Time: " + timer.ToString("0.00") + "s";
        scoreText.text = "Score: " + score.ToString() + " carrots";
    }
}
