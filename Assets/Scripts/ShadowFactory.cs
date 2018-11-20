using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFactory : MonoBehaviour {

    [SerializeField] GameObject shadow;

    [SerializeField] private float shadowDelay;

    // Use this for initialization
    void Start () {
        //hare = GameObject.Find("Hare");
        //StartCoroutine(InstantiateShadow());
        //hare.GetComponent<StateRecorder>().StartRecording();                
        //shadow = Instantiate(shadow, new Vector3(3,0.2f,0), hare.transform.rotation);
        
        //shadow.GetComponent<ShadowController>().EnableShadowVisuals();
        //shadow.transform.position = new Vector3(-3, 0, 0);
    }

    IEnumerator InstantiateShadow(Vector3 pos, float delay)
    {
        if (ShadowController.instance == null)
        {
            GetComponent<StateRecorder>().StartRecording();
            yield return new WaitForSeconds(shadowDelay);
            GameObject shadowObj = Instantiate(shadow, pos, transform.rotation);
            shadowObj.GetComponent<BoxCollider>().enabled = false;
            shadowObj.GetComponent<ShadowController>().setObjToFollow(this.gameObject);
            shadowObj.GetComponent<ShadowController>().StartFollowing(Mathf.Max(delay, shadowDelay));
        }
    }

    public void SpawnShadow(Vector3 pos, float delay)
    {
        GetComponent<StateRecorder>().StartRecording();
        GameObject shadowObj = Instantiate(shadow, pos, transform.rotation);
        shadowObj.GetComponent<BoxCollider>().enabled = false;
        shadowObj.GetComponent<ShadowController>().setObjToFollow(this.gameObject);
        shadowObj.GetComponent<ShadowController>().StartFollowing(Mathf.Max(delay, shadowDelay));
    }
}
