﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
    public bool ResetGameWhenLoaded;

    private void Awake()
    {
        if(ResetGameWhenLoaded)
        {
            BatteryInventory.batteries = 0;
            UI_Inventory.lastDurs = new int[4];
            UI_Inventory.lastWeaponTypes = new string[4];
            UI_Inventory.lastItemButtonNums = new int[4];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
