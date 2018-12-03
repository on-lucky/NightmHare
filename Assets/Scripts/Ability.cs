using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour {

    private float cooldown;
    private bool isCooldown = false;
    [SerializeField] private Image imageCooldown;
	
	// Update is called once per frame
	void Update () {
		if (isCooldown)
        {
            imageCooldown.fillAmount -= 1 / cooldown * Time.deltaTime;

            if (imageCooldown.fillAmount <= 0)
            {
                imageCooldown.fillAmount = 0;
                isCooldown = false;
            }
        }
	}

    public void StartCooldown(float cooldown)
    {
        Fill();
        this.cooldown = cooldown;
        isCooldown = true;
    }

    public void Fill()
    {
        imageCooldown.fillAmount = 1;
    }

    public void Empty()
    {
        imageCooldown.fillAmount = 0;
    }
}
