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
        carrotCount++;
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (carrotCountText) carrotCountText.text = carrotCount.ToString();
    }
}
