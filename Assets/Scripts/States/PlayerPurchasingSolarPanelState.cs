using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingSolarPanelState : PlayerState
{
    EnergySystemObjectController purchasingObjectController;
    UIController uiController;

    string objectName;


    public PlayerPurchasingSolarPanelState(GameController gameController, EnergySystemObjectController purchasingObjectController, UIController uiController) : base(gameController)
    {
        this.purchasingObjectController = purchasingObjectController;
        this.uiController = uiController;
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        if (position.x >= 41 && position.x <= 112 && position.y >= 20 && position.z >= 20 && position.z <= 62)
            this.purchasingObjectController.PrepareObjectForModification(position, this.objectName);
        else
            Debug.Log("Solar Panel must be installed on the roof");
        
    }

    public override void OnInputPointerUp()
    {
        return;
    }

    public override void EnterState(string objectName, string applianceName)
    {
        this.purchasingObjectController.PreparePurchasingObjectController(GetType());
        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
        this.objectName = objectName;
    }

    public override void OnCancel()
    {
        this.purchasingObjectController.CancelModification();
        this.gameController.TransitionToState(this.gameController.selectionState, null, "");
    }

    public override void OnConfirm()
    {
        this.purchasingObjectController.ConfirmModification();
        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
        uiController.InstalledEnergySystems = purchasingObjectController.GetListOfAllObjects();
        this.gameController.TransitionToState(this.gameController.selectionState, null, "");
    }

    public override void OnPuchasingEnergySystem(string objectName)
    {
        if (objectName != "Solar Panel")
        {
            OnCancel();
        }
        base.OnPuchasingEnergySystem(objectName);
    }
}
