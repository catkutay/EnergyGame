using System;
using System.Collections.Generic;
using UnityEngine;

public class EmissionHelper
{
    private PowerHelper powerHelper;

    bool isPowerLinesRunning, isDGRunning = false;

    public EmissionHelper(PowerHelper powerHelper)
    {
        this.powerHelper = powerHelper;
    }


    public void CalculateEmissions(IEnumerable<EnergySystemGeneratorBaseSO> objects, float period)
    {
        float totalSolarPanelOutput = GetTotalSolarPanelOutput(objects);

        foreach (var obj in objects)
        {
            switch (obj.objectName)
            {
                case "On-Grid Power":
                    CalculateGridPowerEmissions(obj, period);
                    break;
                case "Diesel Generator":
                    CalculateDieselGeneratorEmissions(obj, period);
                    break;
                case "Solar Panel":
                    CalculateSolarPanelOffset(obj, totalSolarPanelOutput);
                    break;
                case "Wind Turbine":
                    CalculateWindTurbineOffset(obj);
                    break;
                default:
                    break;
            }
            /*//Debug.Log(obj.objectName);
            if (obj.objectName == "Solar Panel")
            {
                CalculateSolarPanelEmissions(period, obj);
            }
            if (obj.objectName == "Diesel Generator")
            {
                CalculateDieselGeneratorEmissions(period, obj);
            }
            if(obj.objectName == "Wind Turbine")
            {
                CalculateWindTurbineEmissions(period, obj);
            }
            //todo*/
        }

    }

    private float GetTotalSolarPanelOutput(IEnumerable<EnergySystemGeneratorBaseSO> objects)
    {
        float totalSolarPanelOutput = 0;
        foreach (var obj in objects)
        {
            if (obj.objectName.Equals("Solar Panel"))
            {
                totalSolarPanelOutput += obj.powerGeneratedRate;
            }
        }

        return totalSolarPanelOutput;
    }

    private void CalculateGridPowerEmissions(EnergySystemGeneratorBaseSO obj, float period)
    {
        if (obj.isRunning)
        {
            isPowerLinesRunning = true;
            obj.emissionGeneratedAmount += obj.emissionRate * period;
        } else
        {
            isPowerLinesRunning = false;
        }
    }
    private void CalculateDieselGeneratorEmissions(EnergySystemGeneratorBaseSO obj, float period)
    {
        if (obj.isRunning)
        {
            isDGRunning = true;
            obj.emissionGeneratedAmount += obj.emissionRate * period;
        }
        else
        { 
            isDGRunning = false;
        }
    }

    private void CalculateSolarPanelOffset(EnergySystemGeneratorBaseSO obj, float totalSolarPanelOutput)
    {
        if (isPowerLinesRunning || isDGRunning)
        {
            obj.emissionGeneratedAmount = 0f;
        } else if (powerHelper.RenewablesConnected && powerHelper.LoadValue > 0)
        {
            //Debug.Log(totalSolarPanelOutput + " / " + powerHelper.LoadValue);
            obj.emissionGeneratedAmount = (totalSolarPanelOutput / powerHelper.LoadValue) * 100;
        }
    }

    private void CalculateWindTurbineOffset(EnergySystemGeneratorBaseSO obj)
    {
        //if (powerHelper.CanRenewableSystemHandleLoad && (!isPowerLinesRunning && !isDGRunning))
        //{
        if (isPowerLinesRunning || isDGRunning)
        {
            obj.emissionGeneratedAmount = 0f;
        }
        else if (powerHelper.RenewablesConnected)
        {
            obj.emissionGeneratedAmount = (obj.powerGeneratedRate / powerHelper.LoadValue) * 100;
        }
        //}
    }

    

    
}
