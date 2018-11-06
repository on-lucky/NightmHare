using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFactory : MonoBehaviour {

    [SerializeField] GameObject shadow;
    GameObject hare;
    // Use this for initialization
    void Start () {
        hare = GameObject.Find("Hare");
        //StartCoroutine(InstantiateShadow());
        hare.GetComponent<StateRecorder>().StartRecording();                
        shadow = Instantiate(shadow, new Vector3(3,0.2f,0), hare.transform.rotation);
        shadow.GetComponent<ShadowController>().setObjToFollow(hare);
        shadow.GetComponent<ShadowController>().EnableShadowVisuals();
        //shadow.transform.position = new Vector3(-3, 0, 0);
    }

    IEnumerator InstantiateShadow()
    {        
        hare.GetComponent<StateRecorder>().StartRecording();
        GameObject temp = new GameObject();
        temp.transform.position = new Vector3(5, 1, 0);
        shadow = Instantiate(shadow, temp.transform);
        shadow.GetComponent<ShadowController>().setObjToFollow(hare);
        shadow.GetComponent<ShadowController>().EnableShadowVisuals();
        yield return new WaitForSeconds(3);
        shadow.GetComponent<ShadowController>().StopFollowing();        
    }

    // Update is called once per frame
    void Update () {
        
    }
}
