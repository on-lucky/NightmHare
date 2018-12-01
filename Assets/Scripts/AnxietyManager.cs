using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AnxietyManager : MonoBehaviour {
    private float maxAnxiety = 100;
    [SerializeField] private float currentAnxiety = 0;
    [SerializeField] private float sprintAnxietyLevel = 0;
    [SerializeField] private float trapAnxietyLevel = 0;
    [SerializeField] private float veilFactor = 1.0f;

    private RectTransform anxietyGauge;
    private RectTransform anxietyLevel;
    private RectTransform ability1;
    private RectTransform ability2;
    private RawImage veil;
    private float veilOriginalDimension;

    GameObject hare;
    GameObject shadowObject;
    GameObject shadow;    
    float distance;
    [SerializeField] float anxietyZoneRadius = 5f;
    [SerializeField] float anxietyAugmentation = .02f;   

    // Use this for initialization
    void Start () {
        // Get Ari
        hare = GameObject.Find("Hare");

        // Get anxiety level
        var anxiety = GameObject.Find("AnxietyLevel");
        if (anxiety) {
            anxietyLevel = anxiety.GetComponent<Image>().rectTransform;
        }

        // Setup gauge
        var anxietyGaugeGO = GameObject.Find("AnxietyGauge");
        if (anxietyGaugeGO)
        {
            anxietyGauge = anxietyGaugeGO.GetComponent<RectTransform>();
            ability1 = GameObject.Find("Ability_1").GetComponent<RectTransform>();
            ability2 = GameObject.Find("Ability_2").GetComponent<RectTransform>();
        }
        //SetupAbilitiesThreshold();

        // Setup veil
        veilOriginalDimension = Screen.width * 2.5f;
        maxAnxiety = anxietyLevel.sizeDelta.x;
        currentAnxiety = 0;
        GameObject veilGo = GameObject.Find("Veil");
        if (veilGo != null)
        {
            veil = veilGo.GetComponent<RawImage>();
            veil.rectTransform.sizeDelta = new Vector2(veilOriginalDimension, veilOriginalDimension);
            veil.color = new Vector4(veil.color.r, veil.color.g, veil.color.b, (1.5f * currentAnxiety) / maxAnxiety);
        }

        // Subscribe to HareDied event
        hare.GetComponent<LifeManager>().HareDied += OnHareDied;
    }

    private void OnHareDied(object sender, System.EventArgs e)
    {
        currentAnxiety = 0;
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
        } else
        {
            // FIXME: not very efficient. should be done when the shadow is spawned or maybe we never despawn the shadow simply disable it
            shadowObject = GameObject.Find("ShadowHare(Clone)");
            currentAnxiety = 0;
        }

        anxietyLevel.sizeDelta = new Vector2(currentAnxiety, anxietyLevel.sizeDelta.y);
        if (veil != null)
        {
            if (currentAnxiety / maxAnxiety < 0.6)
            {
                float dimension = veilOriginalDimension * (1 - (currentAnxiety * veilFactor) / maxAnxiety);
                veil.rectTransform.sizeDelta = new Vector2(dimension, dimension);
            }
            veil.color = new Vector4(veil.color.r, veil.color.g, veil.color.b, (1.5f * currentAnxiety) / maxAnxiety);
        }

        //SetupAbilitiesThreshold();
    }

    public bool CanSprint()
    {
        return currentAnxiety >= sprintAnxietyLevel;
    }

    public bool CanSetTrap()
    {
        return currentAnxiety >= trapAnxietyLevel;
    }

    private void SetupAbilitiesThreshold()
    {
        if (anxietyGauge)
        {
            float offset = 4 - ability1.sizeDelta.x/2; // CHIFFRE MAGIQUE WOOHOO
            float ratio1 = (sprintAnxietyLevel / 100);
            float ratio2 = (trapAnxietyLevel / 100);
            ability1.localPosition = new Vector3(offset + ratio1 * maxAnxiety, ability1.localPosition.y);
            ability2.localPosition = new Vector3(offset + ratio2 * maxAnxiety, ability2.localPosition.y);
        }
    }
}
