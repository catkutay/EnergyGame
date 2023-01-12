using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingWindTurbineState : PlayerState
{
    EnergySystemObjectController purchasingObjectController;
    UIController uiController;

    string objectName;

    Vector3 position;

    public PlayerPurchasingWindTurbineState(GameController gameController, EnergySystemObjectController purchasingObjectController, Vector3 position, UIController uiController) : base(gameController)
    {
        this.purchasingObjectController = purchasingObjectController;
        this.position = position;
        this.uiController = uiController;
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

    public  override void EnterState(string objectName, string applianceName)
    {
        this.purchasingObjectController.PreparePurchasingObjectController(GetType());
        purchasingObjectController.UpdateSystemAttributesToEnergySystemData();
        this.objectName = objectName;
        if (!this.position.Equals(Vector3.zero))
        {
            this.purchasingObjectController.PrepareObjectForModification(this.position, this.objectName);
        }

    }

    public override void OnPuchasingEnergySystem(string objectName)
    {
        if (objectName != "Wind Turbine")
        {
            OnCancel();
        }
        base.OnPuchasingEnergySystem(objectName);
    }
}
