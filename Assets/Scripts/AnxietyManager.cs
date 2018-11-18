using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AnxietyManager : MonoBehaviour {
    private float maxAnxiety = 100;                            
    private float currentAnxiety = 0;                                  
    public RectTransform anxietyBar;
    [SerializeField] private float sprintAnxietyLevel;
    [SerializeField] private float trapAnxietyLevel;
    private RawImage veil;
    [SerializeField] private float veilFactor = 1.0f;
    private float veilOriginalDimension;

    GameObject hare;
    GameObject shadowObject;
    GameObject shadow;    
    float distance;
    [SerializeField] float anxietyZoneRadius = 5f;
    [SerializeField] float anxietyAugmentation = .02f;   

    // Use this for initialization
    void Start () {
        hare = GameObject.Find("Hare");               
        anxietyBar = GameObject.Find("AnxietyLevel").GetComponent<Image>().rectTransform;
        veilOriginalDimension = Screen.width * 2.5f;
        maxAnxiety = anxietyBar.sizeDelta.x;
        currentAnxiety = 0;
        GameObject veilGo = GameObject.Find("Veil");
        if (veilGo != null)
        {
            veil = veilGo.GetComponent<RawImage>();
            veil.rectTransform.sizeDelta = new Vector2(veilOriginalDimension, veilOriginalDimension);
            veil.color = new Vector4(veil.color.r, veil.color.g, veil.color.b, (1.5f * currentAnxiety) / maxAnxiety);
        }
    }    

    // Update is called once per frame
    void Update () {        
        if (shadowObject != null)
        {
            distance = (hare.transform.position - shadowObject.transform.position).magnitude;
            if (distance < anxietyZoneRadius)
            {
                if (currentAnxiety < maxAnxiety)
                {
                    currentAnxiety += anxietyAugmentation;
                }
            }
            else
            {
                if (currentAnxiety > 0)
                {
                    currentAnxiety -= anxietyAugmentation;
                }
            }
            if (currentAnxiety < 0)
            {
                currentAnxiety = 0;
            }                          
            anxietyBar.sizeDelta = new Vector2(currentAnxiety, anxietyBar.sizeDelta.y);
            if (veil != null)
            {
                if (currentAnxiety / maxAnxiety < 0.6)
                {
                    float dimension = veilOriginalDimension * (1 - (currentAnxiety * veilFactor) / maxAnxiety);
                    veil.rectTransform.sizeDelta = new Vector2(dimension, dimension);
                }
                veil.color = new Vector4(veil.color.r, veil.color.g, veil.color.b, (1.5f * currentAnxiety) / maxAnxiety);
            }            
        } else
        {            
            shadowObject = GameObject.Find("ShadowHare(Clone)");
        }
    }

    public bool CanSprint()
    {
        return currentAnxiety >= sprintAnxietyLevel;
    }

    public bool CanSetTrap()
    {
        return currentAnxiety >= trapAnxietyLevel;
    }
}
