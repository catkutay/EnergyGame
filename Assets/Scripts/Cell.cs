/*======================================================*
 |  Author: Yifan Song
 |  Creation Date: 16/08/2021
 |  Latest Modified Date: 22/08/2021
 |  Description: To store the information about what the player has already placed.
 |  Bugs: N/A
 *=======================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    // Field to store what object was placed
    GameObject objectModel = null;
    EnergySystemGeneratorBaseSO energySystemData;
    ApplianceBaseSO applianceData;

    // Feild showing if this cell has stored any object
    bool isTaken = false;

    public bool IsTaken { get => isTaken; }

    // Store an object on this cell
    public void SetObject(GameObject objectModel, EnergySystemGeneratorBaseSO energySystemData, ApplianceBaseSO applianceData)
    {
        if (objectModel == null)
            return;
        // Object stored
        this.objectModel = objectModel;
        this.energySystemData = energySystemData;
        this.applianceData = applianceData;

        // Cell has been tatekn
        this.isTaken = true;
    }

    // Read Object
    public GameObject GetObject()
    {
        return objectModel;
    }

    public EnergySystemGeneratorBaseSO GetEnergySystemData()
    {
        return energySystemData;
    }

    public ApplianceBaseSO GetApplianceData()
    {
        return applianceData;
    }

    // Remove Object
    public void RemoveObject()
    {
        objectModel = null;
        isTaken = false;
        energySystemData = null;
        applianceData = null;
    }
}
