using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hinter : MonoBehaviour {

    // How long to wait before showing the hint
    [SerializeField]
    private float waitTime = 5;

    // When we started the timer
    private float startTime;
    
    private Image hint;

    private float alpha = 0.5f;
    private float t = 0.01f;

    void Start()
    {
        hint = GetComponent<Image>();
    }

    void Update () {
		if (startTime > 0 && Time.time - startTime > waitTime)
        {
            // Fade the hint in
            float alpha = Mathf.Lerp(hint.color.a, this.alpha, t);
            hint.color = new Color(hint.color.r, hint.color.g, hint.color.b, alpha);
        }
        else
        {
            // Disapear
            hint.color = new Color(hint.color.r, hint.color.g, hint.color.b, 0);
        }
	}

    public void StartTimer()
    {
        startTime = Time.time;
    }

    public void StopTimer()
    {
        startTime = -1;
    }
}
