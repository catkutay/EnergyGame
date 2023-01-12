using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSellingObjectState : PlayerState
{
    EnergySystemObjectController purchasingObjectController;
    ApplianceObjectController purchasingApplianceController;
    UIController uiController;
    public PlayerSellingObjectState(GameController gameController, EnergySystemObjectController purchasingObjectController, ApplianceObjectController purchasingApplianceController, UIController uiController) : base(gameController)
    {
        this.purchasingObjectController = purchasingObjectController;
        this.purchasingApplianceController = purchasingApplianceController;
        this.uiController = uiController;
    }

    public override void OnCancel()
    {
        Debug.Log("cancel");
        //GameObject.Find("BreakerPrefab").gameObject.GetComponent<BoxCollider>().enabled = true;
        this.purchasingObjectController.CancelModification();
        this.purchasingApplianceController.CancelModification();
        this.gameController.TransitionToState(this.gameController.selectionState, null, "");
    }

    public override void OnConfirm()
    {
        //GameObject.Find("BreakerPrefab").gameObject.GetComponent<BoxCollider>().enabled = true;
        this.purchasingObjectController.ConfirmModification();
        this.purchasingApplianceController.ConfirmModification();
        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
        this.uiController.InstalledAppliances = purchasingApplianceController.GetListOfAllAppliances();
        this.uiController.InstalledEnergySystems = purchasingObjectController.GetListOfAllObjects();
        this.gameController.TransitionToState(this.gameController.selectionState, null, "");
    }

    public override void OnPuchasingEnergySystem(string objectName)
    {
        OnCancel();
        base.OnPuchasingEnergySystem(objectName);
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        
        EnergySystemGeneratorBaseSO obj = this.purchasingObjectController.GetEnergySystemDataFromPosition(position);
        ApplianceBaseSO applianceObj = this.purchasingApplianceController.GetApplianceDataFromPosition(position);
        if (obj != null && obj.objectType.Equals("Energy"))
        {
            this.purchasingObjectController.PrepareObjectForSellingAt(position, obj.objectName);
        } else if (applianceObj != null && applianceObj.objectType.Equals("Appliance"))
        {
            this.purchasingApplianceController.PrepareApplianceForSellingAt(position, applianceObj.objectDescription, applianceObj.name.Split('(')[0]);
        }
    }

    public override void OnInputPointerUp()
    {
        return;
    }
    public override void EnterState(string objectVariable, string applianceName)
    {
        //GameObject.Find("BreakerPrefab").gameObject.GetComponent<BoxCollider>().enabled = false;
        this.purchasingObjectController.PreparePurchasingObjectController(GetType());
        this.purchasingApplianceController.PreparePurchasingApplianceController(GetType());
        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
    }


}
