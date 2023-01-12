using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingBatteryState : PlayerState
{
    // Inherited from abstract class
    EnergySystemObjectController purchasingObjectController;
    UIController uiController;

    // New field
    string objectName;
    Vector3 position;
   
 

    public PlayerPurchasingBatteryState(GameController gameController, EnergySystemObjectController purchasingObjectController, Vector3 position, UIController uiController) : base(gameController)
    {
        this.purchasingObjectController = purchasingObjectController;
        this.position = position;
        this.uiController = uiController;
    }

    public override void EnterState(string objectName, string applianceName)
    {
        purchasingObjectController.PreparePurchasingObjectController(GetType());
        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
        this.objectName = objectName;
        if (!this.position.Equals(Vector3.zero))
        {
            this.purchasingObjectController.PrepareObjectForModification(this.position, this.objectName);
        }
    }

    public override void OnCancel()
    {
        this.purchasingObjectController.CancelModification();
        this.gameController.TransitionToState(this.gameController.selectionState, null, "");
    }

    public override void OnConfirm()
    {
        this.purchasingObjectController.ConfirmModification();
        uiController.InstalledEnergySystems = purchasingObjectController.GetListOfAllObjects();
        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
        this.gameController.TransitionToState(this.gameController.selectionState, null, "");
        /*foreach (var obj in uiController.InstalledEnergySystems)
        {
            Debug.Log(obj.objectName);
        }*/
    }
    public override void OnPuchasingEnergySystem(string objectName)
    {
        if(objectName!="Battery")
        {
            OnCancel();
        }
        base.OnPuchasingEnergySystem(objectName);
    }
}
