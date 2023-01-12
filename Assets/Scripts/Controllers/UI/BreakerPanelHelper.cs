using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreakerPanelHelper : MonoBehaviour
{
    public UIController uiController;
    public LoadPanelHelper loadPanelHelper;

    public SwitchManager invertorSwitch, mainLoadSwitch, dieselGeneratorSwitch, mainSwitch;
    public Button mainSwitchBtn;

    public Button closeBreakerPanelBtn, openLoadPanelBtn;

    private float load = 0;
    private bool isInterverSwitchOn = false;
    private bool isMainLoadSwitchOn = false;
    private bool isDGSwitchOn = false;
    private bool isMainSwitchOn = false;

    public bool IsInterverSwitchOn { get => isInterverSwitchOn;  }
    public bool IsMainLoadSwitchOn { get => isMainLoadSwitchOn; }
    public bool IsDGSwitchOn { get => isDGSwitchOn; }
    public bool IsMainSwitchOn { get => isMainSwitchOn; }
    public float Load { get => load; set => load = value; }

    private List<EnergySystemGeneratorBaseSO> energySystemData;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);

        closeBreakerPanelBtn.onClick.AddListener(CloseBreakerPanel);
        openLoadPanelBtn.onClick.AddListener(ShowLoadPanel);
        mainSwitchBtn.onClick.AddListener(ToggleMainSwitch);
    }

    private void ToggleMainSwitch()
    {
        EnergySystemGeneratorBaseSO gridPower = GetGridPowerObject();
        if (mainSwitch.isOn && gridPower != null)
        {
            gridPower.isRunning = false;
            gridPower.isTurnedOn = false;
        } else if (!mainSwitch.isOn && gridPower != null)
        {
            gridPower.isRunning = true;
            gridPower.isTurnedOn = true;
        } else
        {
            Debug.Log("House must be connected to the grid!");
        }
        
    }

    private EnergySystemGeneratorBaseSO GetGridPowerObject()
    {
        this.energySystemData = uiController.InstalledEnergySystems;
        if (energySystemData != null)
        {
            foreach (var obj in energySystemData)
            {
                if (obj.objectName.Equals("On-Grid Power"))
                {
                    return obj;
                }
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMainLoadSwitch();
        UpdateInvertorSwitch();
        UpdateDieselGeneratorSwitch();
        UpdateMainSwitch();
    }

    private void UpdateMainSwitch()
    {
        if (mainSwitch.isOn)
        {
            isMainSwitchOn = true;
        }
        else
        {
            isMainSwitchOn = false;
        }
    }

    private void UpdateDieselGeneratorSwitch()
    {
        if (dieselGeneratorSwitch.isOn)
        {
            isDGSwitchOn = true;
        }
        else
        {
            isDGSwitchOn = false;
        }
    }

    private void UpdateInvertorSwitch()
    {
        if (invertorSwitch.isOn)
        {
            isInterverSwitchOn = true;
        }
        else
        {
            isInterverSwitchOn = false;
        }
    }

    private void UpdateMainLoadSwitch()
    {
        if (mainLoadSwitch.isOn)
        {
            isMainLoadSwitchOn = true;
        }
        else
        {
            isMainLoadSwitchOn = false;
        }
    }

    public void HideLoadPanel()
    {
        loadPanelHelper.gameObject.SetActive(false);
    }

    public void ShowLoadPanel()
    {
        loadPanelHelper.gameObject.SetActive(true);
    }
    private void CloseBreakerPanel()
    {
        gameObject.SetActive(false);
    }

    /*private void Save()
    {
        GetLoadValue();
        //UpdateLoadValueUI();
    }

    private void GetLoadValue()
    {
        try
        {
            load = float.Parse(loadValue.text);
            saveddNotification.OpenNotification();
        }
        catch
        {
            failedNotification.OpenNotification();
        }
    }*/

}

struct Appliance
{
    public ApplianceBaseSO appliance;
    public SwitchManager applianceSwitch;

    public Appliance(ApplianceBaseSO appliance, SwitchManager applianceSwitch)
    {
        this.appliance = appliance;
        this.applianceSwitch = applianceSwitch;
    }
}
