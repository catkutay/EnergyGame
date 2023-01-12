using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ApplianceRepository : MonoBehaviour
{
    public ApplianceCollection applianceCollection;

    public List<ApplianceBaseSO> GetApplianceObjects()
    {
        List<ApplianceBaseSO> systemObjects = new List<ApplianceBaseSO>();
        // ACs
        systemObjects.Add(applianceCollection.aCSmallSO);
        systemObjects.Add(applianceCollection.aCMediumSO);
        systemObjects.Add(applianceCollection.aCLargeSO);
        // Washing Machines
        systemObjects.Add(applianceCollection.washerSmallSO);
        systemObjects.Add(applianceCollection.washerLargeSO);
        // Lights
        systemObjects.Add(applianceCollection.lightLivingRoomSO);
        systemObjects.Add(applianceCollection.lightKitchenSO);
        systemObjects.Add(applianceCollection.lightBedroomSO);
        systemObjects.Add(applianceCollection.lightLaundrySO);
        // Fridges
        systemObjects.Add(applianceCollection.fridgeSmallSO);
        systemObjects.Add(applianceCollection.fridgeLargeSO);
        // Fans
        systemObjects.Add(applianceCollection.fanLivingRoomSO);
        systemObjects.Add(applianceCollection.fanKitchenSO);
        systemObjects.Add(applianceCollection.fanBedroomSO);
        //systemObjects.Add(applianceCollection.dryerSO);
        return systemObjects;
    }

    #region Get individual appliance data
    public ApplianceBaseSO GetApplianceData(string objectName, string applianceName)
    {
        switch (objectName)
        {
            case "Air Conditioner":
                return GetACData(applianceName);
            case "Washing Machine":
                return GetWasherData(applianceName);
            case "Light":
                return GetLightData(applianceName);
            case "Fridge":
                return GetFridgeData(applianceName);
            case "Ceiling Fan":
                return GetFanData(applianceName);
            default:
                return null;
        }
    }

    private ApplianceBaseSO GetACData(string objectName)
    {
        switch (objectName)
        {
            case "AC Small":
                return applianceCollection.aCSmallSO;
            case "AC Medium":
                return applianceCollection.aCMediumSO;
            case "AC Large":
                return applianceCollection.aCLargeSO;
            default:
                return null;
        }
    }
    private ApplianceBaseSO GetWasherData(string objectName)
    {
        switch (objectName)
        {
            case "Washer 7kg":
                return applianceCollection.washerSmallSO;
            case "Washer 10kg":
                return applianceCollection.washerLargeSO;
            default:
                return null;
        }
    }
    private ApplianceBaseSO GetLightData(string objectName)
    {
        switch (objectName)
        {
            case "Light Bedroom":
                return applianceCollection.lightBedroomSO;
            case "Light Kitchen":
                return applianceCollection.lightKitchenSO;
            case "Light Laundry":
                return applianceCollection.lightLaundrySO;
            case "Light Living Room":
                return applianceCollection.lightLivingRoomSO;
            default:
                return null;
        }
    }
    private ApplianceBaseSO GetFridgeData(string objectName)
    {
        switch (objectName)
        {
            case "Fridge Small":
                return applianceCollection.fridgeSmallSO;
            case "Fridge Large":
                return applianceCollection.fridgeLargeSO;
            default:
                return null;
        }
    }
    private ApplianceBaseSO GetFanData(string objectName)
    {
        switch (objectName)
        {
            case "Fan Bedroom":
                return applianceCollection.fanBedroomSO;
            case "Fan Kitchen":
                return applianceCollection.fanKitchenSO;
            case "Fan Living Room":
                return applianceCollection.fanLivingRoomSO;
            default:
                return null;
        }
    }
    #endregion
}


