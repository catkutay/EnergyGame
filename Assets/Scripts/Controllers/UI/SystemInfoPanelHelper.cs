using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using UnityEngine.Events;

public class SystemInfoPanelHelper : MonoBehaviour
{
    public TextMeshProUGUI objectName, storage, efficiency, inputRate, OutputRate, powerRate, powerAmount, emissionRate, emissionAmount, fuelAmount, batteryWarningText;

    public Image objectIcon;

    public Toggle solarPanelToggle, windTurbineToggle, batteryToggle, inverterToggle, chargeControllerToggle, statusToggle;

    public SwitchManager invertorSwitch, chargeControllerSwitch, dieselGeneratorSwitch, applianceSwitch;
    public Button invertorSwitchBtn, chargeControllerSwitchBtn, dieselGeneratorSwitchBtn, applianceSwitchBtn;

    public Button fuelPurchaseBtn, maintenanceBtn;

    private string batteryWarning;

    EnergySystemGeneratorBaseSO data;
    ApplianceBaseSO applianceData;

    public string BatteryWarning { get => batteryWarning; set => batteryWarning = value; }

    // Start is called before the first frame update
    void Start()
    {
        Hide();
        data = ScriptableObject.CreateInstance<NullObjectSO>();
        applianceData = ScriptableObject.CreateInstance<NullApplianceSO>();
        invertorSwitchBtn.onClick.AddListener(RefreshInverterStatus);
        chargeControllerSwitchBtn.onClick.AddListener(RefreshChargeControllerStatus);
        dieselGeneratorSwitchBtn.onClick.AddListener(RefreshDieselGeneratorStatus);
        applianceSwitchBtn.onClick.AddListener(ToggleAppliance);

    }

    

    void Update()
    {
        if(data.objectName=="Solar Panel")
        {
            UpdateSolarPanelInfo(data);
        }
        if(data.objectName == "Battery")
        {
            UpdateBatteryInfo(data);
        }
        if (data.objectName == "Diesel Generator")
        {
            UpdateDieselGeneratorInfo(data);
        }
    }

    public void RefreshInverterStatus()
    {
        UpdateInvertorStatus(data);
        
    }

    private void RefreshDieselGeneratorStatus()
    {
        UpdateDieselGeneratorStatus(data);
    }

    private void RefreshChargeControllerStatus()
    {
        UpdateChargeControllerStatus(data);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public void DisplaySolarPanelInfo(EnergySystemGeneratorBaseSO data)
    {
        Show();
        this.data = data;
        HideElement(batteryWarningText.gameObject);
        HideElement(storage.gameObject);
        HideElement(inputRate.gameObject);
        HideElement(OutputRate.gameObject);
        HideElement(solarPanelToggle.gameObject);
        HideElement(windTurbineToggle.gameObject);
        HideElement(batteryToggle.gameObject);
        HideElement(invertorSwitch.gameObject);
        HideElement(chargeControllerSwitch.gameObject);
        HideElement(dieselGeneratorSwitch.gameObject);
        HideElement(fuelAmount.gameObject);
        HideElement(fuelPurchaseBtn.gameObject);
        HideElement(emissionRate.gameObject);
        HideElement(emissionAmount.gameObject);
        HideElement(applianceSwitch.gameObject);
        HideElement(powerAmount.gameObject);

        //further Usage
        //todo
        HideElement(maintenanceBtn.gameObject);

        SetText(objectName, data.objectName);
        SetIcon(objectIcon, data.objectIcon);

        SetToggle(inverterToggle, data.isConnectedToInvertor);
        SetToggle(chargeControllerToggle, data.isConnectedToChargeController);

        UpdateSolarPanelInfo(data);
    }

    private void UpdateSolarPanelInfo(EnergySystemGeneratorBaseSO data)
    {
        SetText(efficiency, data.efficiency+" %");
        SetText(powerRate, Math.Round(data.powerGeneratedRate, 2) + " kwh");
        //SetText(powerAmount, data.powerGeneratedAmount + " kwh");
        SetToggle(statusToggle, data.isRunning);
    }

    public void DisplayInvertorInfo(EnergySystemGeneratorBaseSO data)
    {
        Show();
        this.data = data;
        HideElement(batteryWarningText.gameObject);
        HideElement(storage.gameObject);
        HideElement(inputRate.gameObject);
        HideElement(OutputRate.gameObject);
        HideElement(fuelAmount.gameObject);
        HideElement(fuelPurchaseBtn.gameObject);
        HideElement(powerRate.gameObject);
        HideElement(powerAmount.gameObject);
        HideElement(emissionRate.gameObject);
        HideElement(emissionAmount.gameObject);
        HideElement(batteryToggle.gameObject);
        HideElement(inverterToggle.gameObject);
        HideElement(maintenanceBtn.gameObject);
        HideElement(chargeControllerSwitch.gameObject);
        HideElement(dieselGeneratorSwitch.gameObject);
        HideElement(applianceSwitch.gameObject);

        SetText(objectName, data.objectName);
        SetIcon(objectIcon, data.objectIcon);
        SetText(efficiency, data.efficiency.ToString());
        SetToggle(chargeControllerToggle, data.isConnectedToChargeController);
        SetToggle(solarPanelToggle, data.isConnectedToSolarPanel);
        SetToggle(windTurbineToggle, data.isConnectedToWindTurbine);
        // default: is_on == is_turn_On ==> false
        SetSwitch(invertorSwitch, data.isTurnedOn);
        //default is_running = false
        SetToggle(statusToggle, data.isRunning);


    }

    private void UpdateInvertorStatus(EnergySystemGeneratorBaseSO data)
    {
        if (!invertorSwitch.isOn)
        {
            data.isTurnedOn = true;
            data.isRunning = true;
        }
        else
        {
            data.isTurnedOn = false;
            data.isRunning = false;
        }
        SetToggle(statusToggle, data.isRunning);
    }

    public void DisplayChargeControllerInfo(EnergySystemGeneratorBaseSO data)
    {
        Show();
        this.data = data;
        HideElement(batteryWarningText.gameObject);
        HideElement(inputRate.gameObject);
        HideElement(OutputRate.gameObject);
        HideElement(storage.gameObject);

        HideElement(fuelAmount.gameObject);
        HideElement(fuelPurchaseBtn.gameObject);

        HideElement(powerRate.gameObject);
        HideElement(powerAmount.gameObject);
        HideElement(emissionRate.gameObject);
        HideElement(emissionAmount.gameObject);
        HideElement(chargeControllerToggle.gameObject);

        HideElement(maintenanceBtn.gameObject);
        HideElement(invertorSwitch.gameObject);
        HideElement(dieselGeneratorSwitch.gameObject);
        HideElement(applianceSwitch.gameObject);

        SetText(objectName, data.objectName);
        SetIcon(objectIcon, data.objectIcon);
        SetText(efficiency, data.efficiency.ToString());
        SetToggle(inverterToggle, data.isConnectedToInvertor);
        SetToggle(batteryToggle, data.isConnectedToBattery);
        SetToggle(solarPanelToggle, data.isConnectedToSolarPanel);
        SetToggle(windTurbineToggle, data.isConnectedToWindTurbine);
        SetSwitch(chargeControllerSwitch, data.isTurnedOn);
        SetToggle(statusToggle, data.isRunning);
    }

    private void UpdateChargeControllerStatus(EnergySystemGeneratorBaseSO data)
    {
        if (!chargeControllerSwitch.isOn)
        {
            data.isTurnedOn = true;
            data.isRunning = true;
        }
        else
        {
            data.isTurnedOn = false;
            data.isRunning = false;
        }
        SetToggle(statusToggle, data.isRunning);
    }

    public void DisplayBatteryInfo(EnergySystemGeneratorBaseSO data)
    {
        Show();
        this.data = data;
        HideElement(fuelAmount.gameObject);
        HideElement(fuelPurchaseBtn.gameObject);
        HideElement(solarPanelToggle.gameObject);
        HideElement(windTurbineToggle.gameObject);
        HideElement(batteryToggle.gameObject);
        HideElement(inverterToggle.gameObject);
        HideElement(powerRate.gameObject);
        HideElement(powerAmount.gameObject);
        HideElement(emissionRate.gameObject);
        HideElement(emissionAmount.gameObject);
        HideElement(maintenanceBtn.gameObject);
        HideElement(invertorSwitch.gameObject);
        HideElement(dieselGeneratorSwitch.gameObject);
        HideElement(chargeControllerSwitch.gameObject);
        HideElement(applianceSwitch.gameObject);

        SetText(objectName, data.objectName);
        SetIcon(objectIcon, data.objectIcon);
        SetText(efficiency, data.efficiency + " %");
        SetToggle(chargeControllerToggle, data.isConnectedToChargeController);
        
        UpdateBatteryInfo(data);
    }

    private void UpdateBatteryInfo(EnergySystemGeneratorBaseSO data)
    {
        SetText(batteryWarningText, batteryWarning);
        if (data.powerInputRate < 0)
        {
            SetText(inputRate, "0 kwh");
        } else
        {
            SetText(inputRate, Math.Round(data.powerInputRate, 2) + " kwh");
        }
        SetText(OutputRate, Math.Round(data.powerOutputRate, 2) + " kwh");
        SetText(storage, Math.Round(data.batteryStorageAmount, 2) + " kwh");
        SetToggle(statusToggle, data.isRunning);
    }

    // leave it for now // further usage
    //todo
    public void DisplayWindTurbineInfo(EnergySystemGeneratorBaseSO data)
    {
        Show();
        this.data = data;
        HideElement(batteryWarningText.gameObject);
        HideElement(inputRate.gameObject);
        HideElement(OutputRate.gameObject);
        HideElement(storage.gameObject);

        HideElement(fuelAmount.gameObject);
        HideElement(fuelPurchaseBtn.gameObject);

        HideElement(solarPanelToggle.gameObject);
        HideElement(windTurbineToggle.gameObject);
        HideElement(batteryToggle.gameObject);

        HideElement(invertorSwitch.gameObject);
        HideElement(chargeControllerSwitch.gameObject);
        HideElement(dieselGeneratorSwitch.gameObject);
        HideElement(applianceSwitch.gameObject);
        HideElement(maintenanceBtn.gameObject);

        HideElement(emissionRate.gameObject);
        HideElement(emissionAmount.gameObject);

        SetIcon(objectIcon, data.objectIcon);
        SetText(objectName, data.objectName);
        SetText(efficiency, data.efficiency+" %");
        SetText(powerRate, data.powerGeneratedRate +"kw");
        SetText(powerAmount, data.powerGeneratedAmount+"kwh");
        SetToggle(inverterToggle, data.isConnectedToInvertor);
        SetToggle(chargeControllerToggle, data.isConnectedToChargeController);
        SetToggle(statusToggle, data.isRunning);
    }

    public void DisplayDieselGeneratorInfo(EnergySystemGeneratorBaseSO data)
    {
        Show();
        this.data = data;
        HideElement(batteryWarningText.gameObject);
        HideElement(inputRate.gameObject);
        HideElement(OutputRate.gameObject);
        HideElement(storage.gameObject);
        HideElement(solarPanelToggle.gameObject);
        HideElement(windTurbineToggle.gameObject);
        HideElement(chargeControllerToggle.gameObject);
        HideElement(inverterToggle.gameObject);
        HideElement(batteryToggle.gameObject);
        HideElement(maintenanceBtn.gameObject);
        HideElement(chargeControllerSwitch.gameObject);
        HideElement(invertorSwitch.gameObject);
        HideElement(efficiency.gameObject);
        HideElement(applianceSwitch.gameObject);

        SetText(objectName, data.objectName);
        SetIcon(objectIcon, data.objectIcon);
        SetText(powerRate, data.powerGeneratedRate + " kw");
        SetText(powerAmount, data.powerGeneratedAmount + " kwh");
        SetText(emissionRate, data.emissionRate + " kgCO2/kWh");
        SetText(emissionAmount, data.emissionGeneratedAmount + " kg");
        SetSwitch(dieselGeneratorSwitch, data.isTurnedOn);
        SetText(fuelAmount, data.fuelAmount + "/60L");
        //SetButton(fuelPurchaseBtn, data.purchaseFuelBtn);
        SetToggle(statusToggle, data.isRunning);
        /*Debug.Log("dieselGeneratorSwitch_IsOn: " + dieselGeneratorSwitch.isOn);
        Debug.Log("is_Turned_On: " + data.isTurnedOn);
        Debug.Log("Is_Running: " + data.isRunning);*/
    }

    public void DisplayApplianceInfo(ApplianceBaseSO applianceData)
    {
        this.data = ScriptableObject.CreateInstance<NullObjectSO>();
        Show();
        this.applianceData = applianceData;

        HideElement(batteryWarningText.gameObject);
        HideElement(efficiency.gameObject);
        HideElement(storage.gameObject);
        HideElement(inputRate.gameObject);
        HideElement(OutputRate.gameObject);
        HideElement(powerRate.gameObject);
        HideElement(powerAmount.gameObject);
        HideElement(emissionRate.gameObject);
        HideElement(emissionAmount.gameObject);
        HideElement(solarPanelToggle.gameObject);
        HideElement(windTurbineToggle.gameObject);
        HideElement(chargeControllerToggle.gameObject);
        HideElement(inverterToggle.gameObject);
        HideElement(batteryToggle.gameObject);
        HideElement(maintenanceBtn.gameObject);
        HideElement(chargeControllerSwitch.gameObject);
        HideElement(dieselGeneratorSwitch.gameObject);
        HideElement(invertorSwitch.gameObject);
        HideElement(statusToggle.gameObject);
        HideElement(fuelAmount.gameObject);
        HideElement(fuelPurchaseBtn.gameObject);


        SetText(objectName, applianceData.objectName);
        SetIcon(objectIcon, applianceData.objectIcon);
        SetSwitch(applianceSwitch, applianceData.isTurnedOn);
    }

    private void ToggleAppliance()
    {
        if (!applianceSwitch.isOn)
        {
            applianceData.isTurnedOn = true;
        }
        else
        {
            applianceData.isTurnedOn = false;
        }
    }

    private void UpdateDieselGeneratorInfo(EnergySystemGeneratorBaseSO data)
    {
        SetText(powerRate, data.powerGeneratedRate + " kw");
        SetText(powerAmount, data.powerGeneratedAmount + " kwh");
        SetText(emissionAmount, data.emissionGeneratedAmount + " kg");
        SetText(fuelAmount, data.fuelAmount + "/60L");
    }

    private void UpdateDieselGeneratorStatus(EnergySystemGeneratorBaseSO data)
    {
        if (!dieselGeneratorSwitch.isOn)
        {
            data.isTurnedOn = true;
            data.isRunning = true;
        }
        else
        {
            data.isTurnedOn = false;
            data.isRunning = false;
        }

        SetToggle(statusToggle, data.isRunning);
        /*Debug.Log("dieselGeneratorSwitch_IsOn: " + dieselGeneratorSwitch.isOn);
        Debug.Log("is_Turned_On: " + data.isTurnedOn);
        Debug.Log("Is_Running: " + data.isRunning);*/
    }


    private void HideElement(GameObject element)
    {
        element.transform.parent.gameObject.SetActive(false);
    }

    private void ShowElement(GameObject element)
    {
        element.transform.parent.gameObject.SetActive(true);
    }

    private void SetText(TextMeshProUGUI element, string value)
    {
        ShowElement(element.gameObject);
        element.text = value;
    }

    private void SetToggle(Toggle element, bool value)
    {
        ShowElement(element.gameObject);
        element.isOn = value;
    }

    private void SetIcon(Image element, Sprite objectIcon)
    {
        ShowElement(element.gameObject);
        element.sprite = objectIcon;
    }

    private void SetButton(Button element, Button target)
    {
        ShowElement(element.gameObject);
        element = target;
    }

    private void SetSwitch(SwitchManager element, bool value)
    {
        ShowElement(element.gameObject);
        element.isOn = value;
    }



}
