using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingDieselGeneratorState : PlayerState
{
    EnergySystemObjectController purchasingObjectController;
    UIController uiController;
    string objectName;
    Vector3 position;

    public PlayerPurchasingDieselGeneratorState(GameController gameController, EnergySystemObjectController purchasingObjectController, Vector3 position, UIController uiController) : base(gameController)
    {
        this.purchasingObjectController = purchasingObjectController;
        this.uiController = uiController;
        this.position = position;
    }

    public override void EnterState(string objectName, string applianceName)
    {
        this.purchasingObjectController.PreparePurchasingObjectController(GetType());
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
        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
        uiController.InstalledEnergySystems = purchasingObjectController.GetListOfAllObjects();
        this.gameController.TransitionToState(this.gameController.selectionState, null, "");
    }

    public override void OnPuchasingEnergySystem(string objectName)
    {
        if (objectName != "Diesel Generator")
        {
            OnCancel();
        }
        base.OnPuchasingEnergySystem(objectName);
    }

}
