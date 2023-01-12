/*======================================================*
 |  Author: Yifan Song
 |  Creation Date: 21/08/2021
 |  Latest Modified Date: 21/08/2021
 |  Description: 
 |  Bugs: N/A
 *=======================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionState : PlayerState
{
    EnergySystemObjectController purchasingObjectController;
    ApplianceObjectController purchasingApplianceController;
    Vector3? previousPosition;

    public PlayerSelectionState(GameController gameController, EnergySystemObjectController objectController, ApplianceObjectController purchasingApplianceController) : base(gameController)
    {
        this.purchasingObjectController = objectController;
        this.purchasingApplianceController = purchasingApplianceController;
    }
    public override void OnInputPointerChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        EnergySystemGeneratorBaseSO data = purchasingObjectController.GetEnergySystemDataFromPosition(position);
        ApplianceBaseSO applianceData = purchasingApplianceController.GetApplianceDataFromPosition(position);

        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
        if (data)
        {
            this.gameController.uiController.DisplaySystemInfo(data);
            previousPosition = position;
        }
        else if (applianceData)
        {
            this.gameController.uiController.DisplayApplianceInfo(applianceData);
            previousPosition = position;
        }
        else
        {
            this.gameController.uiController.HideSystemInfo();
            data = null;
            previousPosition = null;
        }
        return;
    }

    public override void OnInputPointerUp()
    {
        return;
    }

    public override void OnCancel()
    {
        return;
    }

    public override void OnPuchasingEnergySystem(string objectName)
    {
        
        switch (objectName)
        {
            case "Diesel Generator":
                this.gameController.TransitionToState(this.gameController.purchasingDieselGeneratorState, objectName, "");
                break;
            case "Battery":
                this.gameController.TransitionToState(this.gameController.purchasingBatteryState, objectName, "");
                break;
            case "Solar Panel":
                this.gameController.TransitionToState(this.gameController.purchasingSolarPanelState, objectName, "");
                break;
            case "Wind Turbine":
                this.gameController.TransitionToState(this.gameController.purchasingWindTurbineState, objectName, "");
                break;
            case "Invertor":
                this.gameController.TransitionToState(this.gameController.purchasingInvertorState, objectName, "");
                break;
            case "Charge Controller":
                this.gameController.TransitionToState(this.gameController.purchasingChargeControllerState, objectName, "");
                break;
            case "On-Grid Power":
                this.gameController.TransitionToState(this.gameController.purchasingPowerLinesState, objectName, "");
                break;
            default:
                throw new Exception("No such energy system type." + objectName);
        }
    }

    public override void OnPuchasingAppliance(string objectName, string applianceName)
    {
        switch (objectName)
        {
            case "Air Conditioner":
                this.gameController.TransitionToState(this.gameController.purchasingACState, objectName, applianceName);
                break;
            case "Washing Machine":
                this.gameController.TransitionToState(this.gameController.purchasingWashingMachineState, objectName, applianceName);
                break;
            case "Light":
                this.gameController.TransitionToState(this.gameController.purchasingLightState, objectName, applianceName);
                break;
            case "Fridge":
                this.gameController.TransitionToState(this.gameController.purchasingFridgeState, objectName, applianceName);
                break;
            case "Ceiling Fan":
                this.gameController.TransitionToState(this.gameController.purchasingFanState, objectName, applianceName);
                break;
            default:
                throw new Exception("No such appliance type." + objectName);
        }
    }

    public override void EnterState(string objectVariable, string applianceName)
    {
        if (this.gameController.uiController.GetSystemInfoVisibility())
        {
            EnergySystemGeneratorBaseSO data = purchasingObjectController.GetEnergySystemDataFromPosition(previousPosition.Value);
            ApplianceBaseSO applianceData = purchasingApplianceController.GetApplianceDataFromPosition(previousPosition.Value);
            
            if (data)
            {
                this.gameController.uiController.DisplaySystemInfo(data);
            } else if (applianceData)
            {
                this.gameController.uiController.DisplayApplianceInfo(applianceData);
            }
            else
            {
                this.gameController.uiController.HideSystemInfo();
                previousPosition = null;
            }
        }
    }
}
