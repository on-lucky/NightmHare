using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarrotCollector : MonoBehaviour
{
    [SerializeField]
    private int carrotCount = 0;

    [SerializeField]
    private Text carrotCountText;

    public void Collect(Carrot carrot)
    {
        ScoreManager.instance.IncrementScore();

        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (carrotCountText) carrotCountText.text = ScoreManager.instance.GetScore().ToString();
    }
}
