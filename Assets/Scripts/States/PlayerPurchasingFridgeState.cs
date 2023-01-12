/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingFridgeState : PlayerState
{
    ApplianceObjectController purchasingApplianceController;
    UIController uiController;
    string objectName;

    Vector3 cameraPosition = new Vector3(80f, 10f, 54.84f);
    Quaternion cameraRotation = Quaternion.Euler(0f, -100f, 0f);

    public PlayerPurchasingFridgeState(GameController gameController, ApplianceObjectController purchasingApplianceController, UIController uiController) : base(gameController)
    {
        this.purchasingApplianceController = purchasingApplianceController;
        this.uiController = uiController;
    }

    public override void EnterState(string objectName, string applianceName)
    {
        // Set camera
        this.uiController.CameraMovementController.SetPosition(cameraPosition, cameraRotation);

        purchasingApplianceController.PreparePurchasingApplianceController(GetType());
        //purchasingApplianceController.UpdateSystemAttributesToApplianceData();
        this.objectName = objectName;
        if (!this.position.Equals(Vector3.zero))
        {
            purchasingApplianceController.PrepareApplianceForModification(this.position, this.objectName, applianceName);
        }
    }

    public override void OnCancel()
    {
        this.purchasingApplianceController.CancelModification();
        this.gameController.TransitionToState(this.gameController.selectionState, null, "");
    }

    public override void OnConfirm()
    {
        this.purchasingApplianceController.ConfirmModification();
        uiController.InstalledAppliances = purchasingApplianceController.GetListOfAllAppliances();
        this.gameController.TransitionToState(this.gameController.selectionState, null, "");
    }

    public override void OnPuchasingAppliance(string objectName, string applianceName)
    {
        //Debug.Log(objectName);
        if (objectName != "Fridge")
        {
            OnCancel();
        }
        base.OnPuchasingAppliance(objectName, applianceName);
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingFridgeState : PlayerState
{
    ApplianceObjectController purchasingApplianceController;
    UIController uiController;
    string objectName;

    public PlayerPurchasingFridgeState(GameController gameController, ApplianceObjectController purchasingApplianceController, UIController uiController) : base(gameController)
    {
        this.purchasingApplianceController = purchasingApplianceController;
        this.uiController = uiController;
    }

    public override void EnterState(string objectName, string applianceName)
    {
        // Get AC position
        Vector3 FridgePosition = new Vector3(0, 0, 0);
        FridgePosition = GetFridgePosition(applianceName, FridgePosition);

        // Get AC camera position & rotation
        Vector3 cameraPosition = new Vector3(0f, 0f, 0f);
        Quaternion cameraRotation = Quaternion.Euler(0f, 0f, 0f);
        GetCameraOptions(applianceName, ref cameraPosition, ref cameraRotation);

        // Set camera
        this.uiController.CameraMovementController.SetPosition(cameraPosition, cameraRotation);

        // Prepare AC purchase
        purchasingApplianceController.PreparePurchasingApplianceController(GetType());
        this.objectName = objectName;
        if (!FridgePosition.Equals(Vector3.zero))
        {
            purchasingApplianceController.PrepareApplianceForModification(FridgePosition, this.objectName, applianceName, null);
        }
    }

    private void GetCameraOptions(string applianceName, ref Vector3 cameraPosition, ref Quaternion cameraRotation)
    {
        switch (applianceName)
        {
            case "Fridge Small":
                cameraPosition = new Vector3(47.23401f, 9f, 51.08803f);
                cameraRotation = Quaternion.Euler(0f, 115.881f, 0f);
                break;
            case "Fridge Large":
                cameraPosition = new Vector3(78.21294f, 9f, 51.89795f);
                cameraRotation = Quaternion.Euler(0f, -98.958f, 0f);
                break;
            default:
                break;
        }
    }

    private Vector3 GetFridgePosition(string applianceName, Vector3 FridgePosition)
    {
        switch (applianceName)
        {
            case "Fridge Small":
                FridgePosition = new Vector3(63.12f, 6.12f, 40.83f);
                break;
            case "Fridge Large":
                FridgePosition = new Vector3(48.8f, 0f, 49.9f);
                break;
            default:
                break;
        }
        return FridgePosition;
    }

    public override void OnCancel()
    {
        this.purchasingApplianceController.CancelModification();
        this.gameController.TransitionToState(this.gameController.selectionState, null, "");
    }

    public override void OnConfirm()
    {
        this.purchasingApplianceController.ConfirmModification();
        uiController.InstalledAppliances = purchasingApplianceController.GetListOfAllAppliances();
        this.gameController.TransitionToState(this.gameController.selectionState, null, "");
    }

    public override void OnPuchasingAppliance(string objectName, string applianceName)
    {
        Debug.Log(objectName);
        if (objectName != "Fridge")
        {
            OnCancel();
        }
        base.OnPuchasingAppliance(objectName, applianceName);
    }
}

