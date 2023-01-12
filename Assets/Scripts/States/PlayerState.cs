/*======================================================*
 |  Author: Yifan Song
 |  Creation Date: 21/08/2021
 |  Latest Modified Date: 27/08/2021
 |  Description: OOP Implementation class
 |  Bugs: N/A
 *=======================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected GameController gameController;

    public PlayerState(GameController gameController)
    {
        this.gameController = gameController;
    }

    // Enable OnInputPointerDown in this state (perform action when players click the mouse button)
    public virtual void OnInputPointerDown(Vector3 position)
    {
        // Put Action that you want to perform here......
        // Note: Different state contains different actions!
    }

    // Enable OnInputPointerChange in this state (perform action when players move the mouse button)
    public virtual void OnInputPointerChange(Vector3 position)
    {

    }

    public virtual void OnInputPointerUp()
    {

    }

    public virtual void OnPuchasingEnergySystem(string objectName)
    {

    }

    public virtual void OnPuchasingAppliance(string objectName, string applianceName)
    {

    }

    public virtual Vector3 GetAppliancePosition(string applianceName)
    {
        Vector3 appliancePosition = new Vector3(0f, 0f, 0f);
        List<ApplianceBaseSO> applianceData = this.gameController.uiController.applianceRepository.GetApplianceObjects();

        foreach (var appliance in applianceData)
        {
            if (applianceName.Equals(appliance.name))
            {
                appliancePosition = appliance.objectPrefab.gameObject.transform.localPosition;
            }
        }

        return appliancePosition;
    }

    public virtual void EnterState(string objectVariable, string applianceName)
    {

    }


    public virtual void OnSellingObject()
    {
        this.gameController.TransitionToState(this.gameController.sellingObjectState, null, "");
    }

    /*public virtual void OnSellingAppliance()
    {
        this.gameController.TransitionToState(this.gameController.sellingApplianceState, null);
    }*/


    public abstract void OnCancel();

    public virtual void OnConfirm()
    {

    }
}