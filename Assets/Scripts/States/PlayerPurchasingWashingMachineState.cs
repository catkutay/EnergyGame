/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurchasingWashingMachineState : PlayerState
{
    ApplianceObjectController purchasingApplianceController;
    UIController uiController;
    string objectName;
    Vector3 position;

    Vector3 cameraPosition = new Vector3(71.93f, 8f, 46.89f);
    Quaternion cameraRotation = Quaternion.Euler(0f, -8.76f, 0f);

    public PlayerPurchasingWashingMachineState(GameController gameController, ApplianceObjectController purchasingApplianceController, Vector3 position, UIController uiController) : base(gameController)
    {
        this.purchasingApplianceController = purchasingApplianceController;
        this.position = position;
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
        if (objectName != "Washing Machine")
        {
            OnCancel();
        }
        base.OnPuchasingAppliance(objectName, applianceName);
    }
}*/
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

public class PlayerPurchasingWashingMachineState : PlayerState
{
    ApplianceObjectController purchasingApplianceController;
    UIController uiController;
    string objectName;

    public PlayerPurchasingWashingMachineState(GameController gameController, ApplianceObjectController purchasingApplianceController, UIController uiController) : base(gameController)
    {
        this.purchasingApplianceController = purchasingApplianceController;
        this.uiController = uiController;
    }

    public override void EnterState(string objectName, string applianceName)
    {
        // Get AC position
        Vector3 WasherPosition = new Vector3(0, 0, 0);
        WasherPosition = GetWasherPosition(applianceName, WasherPosition);

        // Get AC camera position & rotation
        Vector3 cameraPosition = new Vector3(0f, 0f, 0f);
        Quaternion cameraRotation = Quaternion.Euler(0f, 0f, 0f);
        GetCameraOptions(applianceName, ref cameraPosition, ref cameraRotation);

        // Set camera
        this.uiController.CameraMovementController.SetPosition(cameraPosition, cameraRotation);

        // Prepare AC purchase
        purchasingApplianceController.PreparePurchasingApplianceController(GetType());
        this.objectName = objectName;
        if (!WasherPosition.Equals(Vector3.zero))
        {
            purchasingApplianceController.PrepareApplianceForModification(WasherPosition, this.objectName, applianceName, null);
        }
    }

    private void GetCameraOptions(string applianceName, ref Vector3 cameraPosition, ref Quaternion cameraRotation)
    {
        switch (applianceName)
        {
            case "Washer 7kg":
                cameraPosition = new Vector3(68.86999f, 9f, 49.99437f);
                cameraRotation = Quaternion.Euler(0f, 1.569f, 0f);
                break;
            case "Washer 10kg":
                cameraPosition = new Vector3(66.18116f, 12f, 57.93915f);
                cameraRotation = Quaternion.Euler(0f, 33.957f, 0f);
                break;
            default:
                break;
        }
    }

    private Vector3 GetWasherPosition(string applianceName, Vector3 WasherPosition)
    {
        switch (applianceName)
        {
            case "Washer 7kg":
                WasherPosition = new Vector3(69f, 0f, 68.24f);
                break;
            case "Washer 10kg":
                WasherPosition = new Vector3(75.9f, 0f, 67.28f);
                break;
            default:
                break;
        }
        return WasherPosition;
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
        if (objectName != "Washing Machine")
        {
            OnCancel();
        }
        base.OnPuchasingAppliance(objectName, applianceName);
    }
}

