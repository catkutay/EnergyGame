/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSellingApplianceState : PlayerState
{
    ApplianceObjectController purchasingApplianceController;
    public PlayerSellingApplianceState(GameController gameController, ApplianceObjectController purchasingApplianceController) : base(gameController)
    {
        this.purchasingApplianceController = purchasingApplianceController;
    }

    public override void OnCancel()
    {
        this.purchasingApplianceController.CancelModification();
        //this.gameController.TransitionToState(this.gameController.selectionState, null);
    }

    public override void OnConfirm()
    {
        this.purchasingApplianceController.ConfirmModification();
        this.gameController.TransitionToState(this.gameController.selectionState, null);
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this.purchasingApplianceController.PrepareApplianceForSellingAt(position);
    }

    public override void OnInputPointerUp()
    {
        return;
    }
    public override void EnterState(string objectVariable)
    {
        this.purchasingApplianceController.PreparePurchasingApplianceController(GetType());
    }


}
*/