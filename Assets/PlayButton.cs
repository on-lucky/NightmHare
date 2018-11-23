using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour {

    public MeshRenderer overLayLight;

	// Use this for initialization
	void Start () {
        overLayLight.enabled = false;
    }

    void OnMouseEnter()
    {
        overLayLight.enabled = true;
    }

    void OnMouseExit()
    {
        overLayLight.enabled = false;
    }

    void OnMouseDown()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
