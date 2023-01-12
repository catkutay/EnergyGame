using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class EnergySystemGeneratorBaseSO: ScriptableObject
{
    public Sprite objectIcon;
    public string objectName;
    public string objectType;
    [TextArea]public string objectDescription;
    public GameObject objectPrefab;

    public int purchaseCost;
    public int sellPrice;
    public int purchaseExperience;
    public float fuelAmount;
    public float batteryStorageAmount;

    public float powerGeneratedRate;
    public float powerGeneratedAmount;
    public float efficiency;
    public float emissionRate;
    public float emissionGeneratedAmount;
    public float powerInputRate;
    public float powerOutputRate;

    [SerializeField]
    protected int incomeRate;

    public int upkeepCost;

    internal void RemoveSolarPanelProvider(EnergySystemGeneratorBaseSO structure)
    {
        throw new NotImplementedException();
    }

    public bool isConnectedToSolarPanel;
    public bool isConnectedToChargeController;
    public bool isConnectedToInvertor;
    public bool isConnectedToWindTurbine;
    public bool isConnectedToBattery;

    public bool isTurnedOn;
    public bool isRunning;

    public Button purchaseFuelBtn;
    public Button maintenanceBtn;

    public int objectWidth;
    public int objectHeight;
    public int objectLength;

    public virtual int GetIncomeRate()
    {
        return incomeRate;
    }

  
}

//[System.Serializable]
//public struct Errors
//{
//    public int errorID;
//}

//[System.Serializable]
//public struct UpdatedVariation
//{
//    public GameObject objectPrefab;
//}

