using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterBurrow : MonoBehaviour {

    // Other end of the burrow
    [SerializeField]
    private MasterBurrow end;

    // Whether the burrow is locked or not
    [SerializeField]
    private int locks = 0;

    // If the player is on top of the burrow
    private bool playerInRange = false;

    // If the teleportation is enabled or not
    private bool teleportationEnabled = true;

    // The player GameObject
    private Digger digger;

    // Floating object above the burrow
    private MeshRenderer floater;

    // Material for when the player is in range
    [SerializeField]
    private Material inRangeMaterial;

    // Material for when the player is away
    [SerializeField]
    private Material awayMaterial;

    [SerializeField]
    private Material[] lockedMaterials;

    [SerializeField]
    private Hinter hinter;

    void Start()
    {
        FadeArrow(true);
        floater = transform.Find("Floater").GetComponent<MeshRenderer>();
        floater.enabled = end != null;
        locks = lockedMaterials.Length;
    }

    void Update() {
        // Update floater material
        if (Locked())
        {
            floater.material = lockedMaterials[locks - 1];
        } else
        {
            FadeArrow(!playerInRange);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (end && playerInRange && teleportationEnabled && !Locked())
            {
                teleportationEnabled = false;
                digger.Dig(this, end);
            }
        }
        else
        {
            // Allow teleportation if the user stops holding the down arrow
            teleportationEnabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;
        Digger digger = go.GetComponent<Digger>();
        if (!Locked() && digger)
        {
            this.digger = digger;
            playerInRange = true;
            if (hinter)
            {
                hinter.StartTimer();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject go = other.gameObject;
        Digger digger = go.GetComponent<Digger>();
        if (!Locked() && digger)
        {
            playerInRange = false;
            teleportationEnabled = true;
            if (hinter)
            {
                hinter.StopTimer();
            }
        }
    }

    private void FadeArrow(bool shouldFade)
    {
        
        if (floater != null)
        {
            if (shouldFade)
            {
                floater.material = awayMaterial;
            }
            else
            {
                floater.material = inRangeMaterial;
            }
        }
    }

    public void DisableTeleportation() { teleportationEnabled = false; }

    public void Lock() { locks++; }

    public void Unlock(bool both = true) {
        locks--;
        if (both)
        {
            end.Unlock(false);
        }
    }
    
    private bool Locked()
    {
        return locks > 0;
    }
}
