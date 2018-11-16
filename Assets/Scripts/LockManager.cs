using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour {

    // The number of locks associated with the burrow
    private int locks = 0;

    [SerializeField]
    private Material[] lockedMaterials;

    private Burrow burrow;

    void Start()
    {
        locks = lockedMaterials.Length;
        burrow = GetComponent<Burrow>();
        if (locks > 0) {
            burrow.Lock();
            burrow.SetLockMaterial(lockedMaterials[locks - 1]);
        }
    }

    void Update() {   
    }

    public void Unlock() {
        locks--;

        if (locks <= 0)
        {
            burrow.Unlock(true);
        }
        else  {
            burrow.SetLockMaterial(lockedMaterials[locks - 1]);
        }
    }
}
