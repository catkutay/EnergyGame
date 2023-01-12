using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerHelper
{
    // Diesel Generator
    private float dieselGeneratorPreviousFuelAmount = 0f;
    private float dieselGeneratorEmissionRate = 0f;
    private float dieselGeneratorPreviousEmissionAmount = 0f;
    private float dieselGeneratorPreviousPowerRate = 0f;
    private float dieselGeneratorPreviousPowerAmout = 0f;
    // On-Grid Power
    private float powerLinesEmissionRate = 0f;
    private float powerLinesPreviousEmissionAmount = 0f;
    private float powerLinesPreviousPowerRate = 0f;
    private float powerLinesPreviousPowerAmout = 0f;
    // Breaker Panel
    bool isInverterSwitchOnBreaker = false;
    bool isMainLoadSwitchOnBreaker = false;
    bool isDGSwitchOnBreaker = false;
    bool isMainSwitchOnBreaker = false;

    // solarPanelInfo
    int solarPanelCount = 0;
    float solarPanelPowerRateTemp = 0f;
    float solarPanelsOutputRate = 0f;
    float solarPanelsOutputAmount = 0f;
    private float loadValueUpdate;
    float solarPanelOutputWentToMainLoad = 0f;
    float solarPanelOutputWentToBattery = 0f;

    float invertorlPowerRateTemp = 0f;
    float batteryPreviousSavedAmount = 0f;
    bool batteryOutOfPower = false;
    float loadValue = 0f;
    private float restTaken = 0f;
    public bool pop = false;
    private float loadDiff = 0f;
    private bool needMoreFuel = false;
    private float energyRequired = 0f;
    bool isSolarPanelRunning = false;
    bool isInvertorExisted = false;
    bool isInvertorRunning = false;
    bool isChargeControllerExisted = false;
    bool isChargeControllerRunning = false;
    bool isBatteryExisted = false;
    bool isBatteryRunning = false;
    bool isDGExisted = false;
    bool isDGRunning = false;
    bool isPowerLinesExisted = false;
    bool isPowerLinesRunning = false;

    private bool canRenewableSystemHandleLoad = false;
    private bool renewablesConnected = false;

    float totalOutputRate = 0f;
    public float LoadDiff { get => loadDiff; set => loadDiff = value; }
    public float TotalOutputRate { get => totalOutputRate;  }

    string batteryWarningText = "";
    public string BatteryWarningText { get => batteryWarningText; set => batteryWarningText = value; }
    public bool CanRenewableSystemHandleLoad { get => canRenewableSystemHandleLoad; set => canRenewableSystemHandleLoad = value; }
    public bool IsDGRunning { get => isDGRunning; set => isDGRunning = value; }
    public bool IsPowerLinesRunning { get => isPowerLinesRunning; set => isPowerLinesRunning = value; }
    public float LoadValue { get => loadValue; set => loadValue = value; }
    public bool RenewablesConnected { get => renewablesConnected; set => renewablesConnected = value; }

    public void CalculatePowerOutput(IEnumerable<EnergySystemGeneratorBaseSO> objects, float period, float poa)
    {
        solarPanelCount = 0;
        solarPanelPowerRateTemp = 0f;

        foreach (var obj in objects)
        {
            if (obj.objectName == "Invertor")
            {
                //UpdateInvertorInfoFromObject(obj);
            }
            if (obj.objectName == "Charge Controller")
            {
                UpdateChargeControllerInfoFromObject(obj);
            }
            if (obj.objectName == "Battery")
            {
                UpdateBatteryInfoFromObject(obj);
            }
            if (obj.objectName == "Diesel Generator")
            {
                UpdateDGInfoFromObject(obj);
            }
            if (obj.objectName == "On-Grid Power")
            {
                UpdatePowerLinesInfoFromObject(obj);
            }
            if (obj.objectName == "Solar Panel")
            {
                //UpdateSolarPanelPowerInfoFromObject(obj, period, poa);
            }
        }

        /*List<EnergySystemGeneratorBaseSO> renewablesData = GetRenewablesData(objects);
        if (renewablesData != null)
        {
            foreach (var obj in renewablesData)
            {
                renewablesOutput += obj.powerGeneratedAmount;
            }
        }*/



        // Update this first
        solarPanelsOutputRate = solarPanelCount * solarPanelPowerRateTemp;

        if (LoadExisted())
        {
            LoadBalance(period);
        }
        else
        {
            if (isSolarPanelRunning && isBatteryExisted && isBatteryRunning && isChargeControllerExisted && isChargeControllerRunning)
            {
                solarPanelOutputWentToBattery = CalculateSolarPanelOutputDistribution(0.81f);

            }
        }

        foreach (var obj in objects)
        {
            if (obj.objectName == "Battery")
            {
                //UpdateBatteryStorageAmount(obj);
            }
            if (obj.objectName == "Diesel Generator")
            {
                UpdateDGInformationAfterRunning(obj);
            }
            if (obj.objectName == "On-Grid Power")
            {
                UpdatePowerLinesInfoAfterRunning(obj);
            }
        }

        if (loadDiff != 0f)
        {
            Debug.Log("No energy system supporting your property's load. Build your system!!!Your property's load has been set to ZERO.Check all switches and connections");

            totalOutputRate = loadValue - loadDiff;
        }
        else
        {
            if (LoadExisted())
            {
                totalOutputRate = loadValue + batteryPreviousSavedAmount + solarPanelOutputWentToBattery;

            }
            else
            {
                totalOutputRate = batteryPreviousSavedAmount + solarPanelOutputWentToBattery;
            }

        }

        //RefreshValues();
    }

    public void CalculateRenewablesOutput(IEnumerable<EnergySystemGeneratorBaseSO> objects, float period, float poa)
    {
        UpdateEnergySystemInfo(objects, period, poa);
        float renewablesOutputRate = GetRenewablesOutputRate(GetRenewablesData(objects));

        if (isBatteryRunning && isChargeControllerExisted && isInvertorExisted && isInverterSwitchOnBreaker && isInvertorRunning)
        {
            renewablesConnected = true;
            float batteryChargeRate = GetBatteryChargeRate(renewablesOutputRate);

            bool canRenewablesHandleLoad;
            foreach (var obj in objects)
            {
                if (obj.objectName.Equals("Battery") && loadValue >= 0 && renewablesOutputRate > 0)
                {
                    // Check if renewables can handle load
                    canRenewablesHandleLoad = CheckRenewables(renewablesOutputRate);

                    // Check if battery was out off power
                    CheckBatteryPower(period, renewablesOutputRate, batteryChargeRate, canRenewablesHandleLoad, obj);

                    //Debug.Log(obj.powerInputRate + " * " + period);


                    BalanceLoad(period, renewablesOutputRate, canRenewablesHandleLoad, obj);

                    // If the battery runs out of power
                    if (obj.batteryStorageAmount <= 0)
                    {
                        canRenewableSystemHandleLoad = false;
                        BatteryOutOfPower(renewablesOutputRate, obj);
                    }

                    // Max battery power at 9 kwh
                    if (obj.batteryStorageAmount > 9)
                    {
                        obj.batteryStorageAmount = 9;
                    }
                }
            }
        }
        else if (loadValue >= 0 && (isPowerLinesRunning || (isDGRunning && isDGSwitchOnBreaker)))
        {
            canRenewableSystemHandleLoad = false;
            totalOutputRate = loadValue;
        }
        else
        {
            totalOutputRate = 0f;
            batteryWarningText = "Renewables not connected.";
            renewablesConnected = false;
            foreach (var obj in objects)
            {
                if (obj.objectName.Equals("Battery"))
                {
                    obj.powerInputRate = 0;
                }
            }
        }

        /*foreach (var obj in objects)
        {
            if (obj.objectName == "Diesel Generator")
            {
                UpdateDGInformationAfterRunning(obj);
            }
            if (obj.objectName == "On-Grid Power")
            {
                UpdatePowerLinesInfoAfterRunning(obj);
            }
        }*/

        RefreshValues();
    }

    private void BatteryOutOfPower(float renewablesOutputRate, EnergySystemGeneratorBaseSO obj)
    {
        obj.batteryStorageAmount = 0;
        //Debug.Log("Battery out of power");

        if (!isPowerLinesRunning && !isDGRunning)
        {
            BatteryWarningText = "Battery out of power.";
            Debug.Log("Nothing to support load. Install a diesel generator or power lines to handle load.");
        }
        else if (renewablesOutputRate > 0)
        {
            batteryOutOfPower = true;
            totalOutputRate = loadValue;
            BatteryWarningText = "Battery out of power. Charging...";
        }
    }

    private void BalanceLoad(float period, float renewablesOutputRate, bool canRenewablesHandleLoad, EnergySystemGeneratorBaseSO obj)
    {
        // If there is a load and the battery CAN handle it
        if (loadValue > 0 && obj.batteryStorageAmount > 0 && batteryPreviousSavedAmount >= loadValue)
        {
            canRenewableSystemHandleLoad = true;
            batteryOutOfPower = false;
            obj.batteryStorageAmount += obj.powerInputRate * period;
            // If renewables can't handle load but battery can
            if (!canRenewablesHandleLoad)
            {
                totalOutputRate = obj.batteryStorageAmount;
                BatteryWarningText = "Insufficient renewables power. Transferring load to battery";
            }
            // If renewables can handle load and do not require battery output
            else
            {
                totalOutputRate = renewablesOutputRate - (renewablesOutputRate - loadValue);
                BatteryWarningText = "Excess renewables power. Charging...";
            }
        }

        // If there is a load and the battery CANNOT handle it
        else if (loadValue > 0 && obj.batteryStorageAmount > 0 && batteryOutOfPower == false)
        {
            obj.batteryStorageAmount += obj.powerInputRate * period;
            if (!canRenewablesHandleLoad)
            {
                totalOutputRate = obj.batteryStorageAmount;
                BatteryWarningText = "Insufficient battery power. Your battery will run out of juice!";
            }
            else
            {
                canRenewableSystemHandleLoad = true;
                totalOutputRate = renewablesOutputRate - (renewablesOutputRate - loadValue);
                BatteryWarningText = "Excess renewables power. Charging...";
            }
        }

        // If there is no load
        else if (loadValue == 0 && batteryOutOfPower == false)
        {
            totalOutputRate = 0f;
            BatteryWarningText = "Charging...";
            obj.batteryStorageAmount += renewablesOutputRate * period;
        }
    }

    private bool CheckRenewables(float renewablesOutputRate)
    {
        if (renewablesOutputRate >= loadValue)
        {
            batteryOutOfPower = false;
            return true;
        }
        return false;
    }

    private void CheckBatteryPower(float period, float renewablesOutputRate, float batteryChargeRate, bool canRenewablesHandleLoad, EnergySystemGeneratorBaseSO obj)
    {
        if (batteryOutOfPower == true && canRenewablesHandleLoad == false)
        {
            // Charge battery at full renewables power rate
            obj.powerInputRate = renewablesOutputRate;
            obj.batteryStorageAmount += renewablesOutputRate * period;
        }
        else
        {
            // Charge/Drain battery
            obj.powerInputRate = batteryChargeRate;
            obj.batteryStorageAmount += obj.powerInputRate * period;
        }
    }

    private void UpdateEnergySystemInfo(IEnumerable<EnergySystemGeneratorBaseSO> objects, float period, float poa)
    {
        foreach (var obj in objects)
        {
            switch (obj.objectName)
            {
                case "Invertor":
                    UpdateInvertorInfoFromObject(obj);
                    break;
                case "Solar Panel":
                    UpdateSolarPanelPowerInfoFromObject(obj, period, poa);
                    break;
                case "Charge Controller":
                    UpdateChargeControllerInfoFromObject(obj);
                    break;
                case "Battery":
                    UpdateBatteryInfoFromObject(obj);
                    break;
                case "On-Grid Power":
                    UpdatePowerLinesInfoFromObject(obj);
                    break;
                case "Diesel Generator":
                    UpdateDGInfoFromObject(obj);
                    break;
            }
        }
    }

    private List<EnergySystemGeneratorBaseSO> GetRenewablesData(IEnumerable<EnergySystemGeneratorBaseSO> objects)
    {
        List<EnergySystemGeneratorBaseSO> renewablesData = new List<EnergySystemGeneratorBaseSO>();
        foreach (var obj in objects)
        {
            if (obj.objectName.Equals("Solar Panel") || obj.objectName.Equals("Wind Turbine"))
            {
                //Debug.Log(true);
                renewablesData.Add(obj);
            }
        }
        return renewablesData;
    }

    private float GetRenewablesOutputRate(List<EnergySystemGeneratorBaseSO> renewablesData)
    {
        float renewablesOutputRate = 0;
        foreach (var obj in renewablesData)
        {
            renewablesOutputRate += obj.powerGeneratedRate;
        }
        //Debug.Log("Renewables Output: " + renewablesOutputRate);
        return renewablesOutputRate;
    }

    private float GetBatteryChargeRate(float renewablesOutputRate)
    {
        float batteryChargeRate = 0;
        if (isBatteryRunning && isChargeControllerExisted && isInvertorExisted && isInverterSwitchOnBreaker && isInvertorRunning)
        {
            batteryChargeRate = renewablesOutputRate - loadValue;
        }
        return batteryChargeRate;
    }

    private void RefreshValues()
    {

        dieselGeneratorPreviousFuelAmount = 0f;
        dieselGeneratorEmissionRate = 0f;
        dieselGeneratorPreviousEmissionAmount = 0f;
        dieselGeneratorPreviousPowerRate = 0f;
        dieselGeneratorPreviousPowerAmout = 0f;

        powerLinesEmissionRate = 0f;
        powerLinesPreviousEmissionAmount = 0f;
        powerLinesPreviousPowerRate = 0f;
        powerLinesPreviousPowerAmout = 0f;

        solarPanelCount = 0;
        solarPanelPowerRateTemp = 0f;
        solarPanelsOutputRate = 0f;
        solarPanelsOutputAmount = 0f;

        solarPanelOutputWentToMainLoad = 0f;
        solarPanelOutputWentToBattery = 0f;

        invertorlPowerRateTemp = 0f;
        batteryPreviousSavedAmount = 0f;
        restTaken = 0f;

        energyRequired = 0f;
        loadValueUpdate = 0f;

        isSolarPanelRunning = false;
        isInvertorExisted = false;
        isInvertorRunning = false;
        isChargeControllerExisted = false;
        isChargeControllerRunning = false;
        isBatteryExisted = false;
        isBatteryRunning = false;
        isDGExisted = false;
        isDGRunning = false;
        needMoreFuel = false;
        isPowerLinesExisted = false;
        //isPowerLinesRunning = false;
    }

    // Need to be Refactored
    private void LoadBalance(float period)
    {
        // pre-set


        // 1 - see if solar panel system can provide all power for property's load
        if (isSolarPanelRunning && CheckInvertorAccessibility() && CheckInvertorLoadBalance())
        {
            if (CheckChargeControllerAccessibility())
            {
                CheckBatteryAccessibility();
            }
        }
        else if (isBatteryExisted && isBatteryRunning && isChargeControllerExisted && isChargeControllerRunning && isInvertorRunning && isInvertorExisted && isMainLoadSwitchOnBreaker && isInverterSwitchOnBreaker)
        {
            if (loadValueUpdate == 0f)
                loadValueUpdate = loadValue;

            CheckBatteryStorageAmount(period, loadValueUpdate);
        } else if (isDGSwitchOnBreaker && isDGExisted && isDGRunning)
        {
            if (loadValueUpdate == 0f)
                loadValueUpdate = loadValue;
            CheckDieselGeneratorAccessibility(period, loadValueUpdate);
        }
        else if (isMainSwitchOnBreaker && isPowerLinesExisted && isPowerLinesRunning)
        {
            if (loadValueUpdate == 0f)
                loadValueUpdate = loadValue;
            CheckPowerLinesAccessibility(period, loadValueUpdate);
        }
        else
        {
            loadDiff = loadValue;
        }
    }
    #region Load Balance Processors

    //reviewed
    private bool CheckInvertorAccessibility()
    {
        // Make sure that the invertor is working properly
        if (isInvertorExisted && isInvertorRunning && isInverterSwitchOnBreaker)
        {
            //Then, convert solar panels rate into invertor's rate (efficiency)
            invertorlPowerRateTemp = solarPanelsOutputRate * 0.9f;
            return true;
        }
        return false;
    }

    //reviewed
    private void CheckBatteryAccessibility()
    {
        if (isBatteryExisted)
        {
            if (isBatteryRunning)
            {
                if (restTaken != 0f)
                {
                    solarPanelOutputWentToBattery = CalculateSolarPanelOutputDistribution(restTaken * 0.81f);
                    return;
                }
            }
        }
    }

    //reviewed
    private void CheckBatteryStorageAmount(float period, float load)
    {
        if (batteryPreviousSavedAmount != 0f)
        {
            float energyNeeded = load * period;
            if (batteryPreviousSavedAmount < energyNeeded)
            {
                if (isDGSwitchOnBreaker && isDGExisted && isDGRunning)
                {
                    CheckDieselGeneratorAccessibility(period, (energyNeeded - batteryPreviousSavedAmount) / period);
                } else if (isMainSwitchOnBreaker && isPowerLinesExisted && isPowerLinesRunning)
                {
                    //CheckPowerLinesAccessibility(period, (energyNeeded - batteryPreviousSavedAmount) / period);
                }
                else
                {
                    loadDiff = (energyNeeded - batteryPreviousSavedAmount) / period;
                }
            }
            else
            {
                batteryPreviousSavedAmount -= energyNeeded;
            }
        }
    }

    // reviewed
    private float CalculateSolarPanelOutputDistribution(float percentage)
    {
        return solarPanelsOutputAmount * percentage;

    }

    // reviewed
    private bool CheckChargeControllerAccessibility()
    {
        if (isChargeControllerExisted)
        {
            //if cc existing 
            if (isChargeControllerRunning)
            {
                // if cc turned on
                return true;
            }
        }
        // lose energy
        return false;
    }

    // reviewd
    private void CheckDieselGeneratorAccessibility(float period, float load)
    {
        energyRequired = load * period;
        float estimatedFuelAmount = energyRequired * 2.12f;
        if (CanIRunIt(dieselGeneratorPreviousFuelAmount, estimatedFuelAmount))
        {
            dieselGeneratorPreviousFuelAmount -= estimatedFuelAmount;
            dieselGeneratorPreviousEmissionAmount += dieselGeneratorEmissionRate * energyRequired;
        }
        else
        {
            loadDiff = load;
            needMoreFuel = true;
        }
    }

    private void CheckPowerLinesAccessibility(float period, float load)
    {
        if (isMainSwitchOnBreaker && isPowerLinesExisted && isPowerLinesRunning)
        {
            energyRequired = load * period;
            powerLinesPreviousEmissionAmount += powerLinesEmissionRate * energyRequired;
        }
    }

    // Need to be refactored
    private bool CheckInvertorLoadBalance()
    {
        if (invertorlPowerRateTemp >= loadValue)
        {
            //not overload
            float invertorTaken = loadValue / invertorlPowerRateTemp;
            restTaken = (1f - invertorTaken) / 0.9f;
            //not neccessary but further usage
            solarPanelOutputWentToMainLoad = CalculateSolarPanelOutputDistribution(invertorTaken);
            return true;
        }
        else
        {
            loadValueUpdate = (loadValue - invertorlPowerRateTemp) / 0.9f;
            //not neccessary but further usage
            solarPanelOutputWentToMainLoad = CalculateSolarPanelOutputDistribution(1f);

            return false;
        }
    }
    #endregion

    #region Update Info To Object
    //reviewed
    private void UpdateSolarPanelPowerInfoFromObject(EnergySystemGeneratorBaseSO solarPanel, float period, float poa)
    {
        solarPanelCount++;
        isSolarPanelRunning = solarPanel.isRunning;

        UpdateSingleSolarPanelOutputRate(solarPanel, poa);
        solarPanelPowerRateTemp = solarPanel.powerGeneratedRate;

        UpdateSingleSolarPanelOutputAmount(solarPanel, period);
    }

    // reviewed
    private void UpdateSingleSolarPanelOutputRate(EnergySystemGeneratorBaseSO solarPanel, float poa)
    {
        if (poa > 0 && poa < 300)
        {
            solarPanel.powerGeneratedRate = (poa / 1000f) * (solarPanel.efficiency / 100f);
        }
        else if (poa >= 300 && poa < 400)
        {
            solarPanel.powerGeneratedRate = 0.3f * (solarPanel.efficiency / 100f);
        }
        else if (poa >= 400)
        {
            solarPanel.powerGeneratedRate = 0.4f * (solarPanel.efficiency / 100f);

        }
    }

 
    private void UpdateSingleSolarPanelOutputAmount(EnergySystemGeneratorBaseSO solarPanel, float period)
    {
        if (IsSolarPanelConnectingToValidObject())
        {
            solarPanel.powerGeneratedAmount += solarPanel.powerGeneratedRate * period;
            solarPanelsOutputAmount += solarPanel.powerGeneratedRate * period;
          
        }
    }

    private bool IsSolarPanelConnectingToValidObject()
    {
        
        // power goes to mainload/battery.
        if (isSolarPanelRunning && isInvertorRunning && isInverterSwitchOnBreaker && isMainLoadSwitchOnBreaker && loadValue != 0f)
        {
            return true;
        }
        else if (isSolarPanelRunning && isChargeControllerRunning && isBatteryRunning)
        {
            return true;
        }
        return false;
    }

    //reviewed
    private void UpdateInvertorInfoFromObject(EnergySystemGeneratorBaseSO invertor)
    {
        isInvertorExisted = true;
        if (invertor.isRunning && isInverterSwitchOnBreaker)
        {
            isInvertorRunning = invertor.isRunning;
        }

    }

    //reviewed
    private void UpdateChargeControllerInfoFromObject(EnergySystemGeneratorBaseSO obj)
    {
        isChargeControllerExisted = true;
        isChargeControllerRunning = obj.isRunning;
        // take 22.60% off power
    }

    //reviewed
    private void UpdateBatteryInfoFromObject(EnergySystemGeneratorBaseSO obj)
    {
        isBatteryExisted = true;
        isBatteryRunning = obj.isRunning;
        // battery's maximum output rate - 9 kw/h
        if (obj.batteryStorageAmount >= 9f)
        {
            obj.powerOutputRate = 9f;
        }
        else
        {
            obj.powerOutputRate = obj.batteryStorageAmount;
        }
        
        batteryPreviousSavedAmount = obj.batteryStorageAmount;
    }

    private void UpdateDGInfoFromObject(EnergySystemGeneratorBaseSO obj)
    {
        isDGRunning = obj.isRunning;
        isDGExisted = true;
        dieselGeneratorPreviousFuelAmount = obj.fuelAmount;
        dieselGeneratorEmissionRate = obj.emissionRate;
        dieselGeneratorPreviousEmissionAmount = obj.emissionGeneratedAmount;
        dieselGeneratorPreviousPowerRate = obj.powerGeneratedRate;
        dieselGeneratorPreviousPowerAmout = obj.powerGeneratedAmount;
    }
    private void UpdatePowerLinesInfoFromObject(EnergySystemGeneratorBaseSO obj)
    {
        isPowerLinesRunning = obj.isRunning;
        isPowerLinesExisted = true;
        powerLinesEmissionRate = obj.emissionRate;
        powerLinesPreviousEmissionAmount = obj.emissionGeneratedAmount;
        powerLinesPreviousPowerRate = obj.powerGeneratedRate;
        powerLinesPreviousPowerAmout = obj.powerGeneratedAmount;
    }
    #endregion

    #region Update Info After Running
    private void UpdateBatteryStorageAmount(EnergySystemGeneratorBaseSO obj)
    {
        obj.powerInputRate = solarPanelsOutputRate;
        if (batteryPreviousSavedAmount + solarPanelOutputWentToBattery >= 25f)
        {
            obj.batteryStorageAmount = 25f;
            Debug.Log("Battery capacity is full");
        }
        else
        {
            obj.batteryStorageAmount = batteryPreviousSavedAmount + solarPanelOutputWentToBattery;
        }
        
        
    }

    private void UpdateDGInformationAfterRunning(EnergySystemGeneratorBaseSO obj)
    {
        obj.fuelAmount = dieselGeneratorPreviousFuelAmount;
        obj.emissionGeneratedAmount = dieselGeneratorPreviousEmissionAmount;
        obj.powerGeneratedRate = loadValue;
        obj.powerGeneratedAmount += energyRequired;


        if (needMoreFuel)
        {
            obj.isTurnedOn = false;
        }
        if (!obj.isRunning)
        {
            loadDiff = loadValue;
        }

        dieselGeneratorPreviousFuelAmount = 0f;
        dieselGeneratorPreviousEmissionAmount = 0f;
        energyRequired = 0f;
    }

    private void UpdatePowerLinesInfoAfterRunning(EnergySystemGeneratorBaseSO obj)
    {
        obj.emissionGeneratedAmount = powerLinesPreviousEmissionAmount;
        obj.powerGeneratedRate = loadValue;
        obj.powerGeneratedAmount += energyRequired;

        if (!obj.isRunning)
        {
            loadDiff = loadValue;
        }

        powerLinesPreviousEmissionAmount = 0f;
        energyRequired = 0f;
    }
    #endregion

    private bool CanIRunIt(float fuelAmount, float estimatedFuelAmount)
    {

        if (fuelAmount >= estimatedFuelAmount)
        {
            return true;
        }
        return false;
    }

    public void GetBreakerSwitchesValue(bool iValue, bool mLValue, bool dValue, float lValue, bool mValue)
    {
        isInverterSwitchOnBreaker = iValue;
        isMainLoadSwitchOnBreaker = mLValue;
        isDGSwitchOnBreaker = dValue;
        isMainSwitchOnBreaker = mValue;
        loadValue = lValue;
    }
    private bool LoadExisted()
    {
        if (loadValue == 0)
        {
            //Debug.Log("Please Set your Load");
            return false;
        }

        if (isMainLoadSwitchOnBreaker == false)
        {
            //Debug.Log("Please Switch Main Load On");
            return false;
        }
        //load!=0 && main (switch on)
        return true;
    }
}


