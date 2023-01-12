using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUpdateHelper 
{
    List<EnergySystemGeneratorBaseSO> objectDataList;
    List<ApplianceBaseSO> applianceDataList;
    GridStructure grid;

    public void GetSystemData(List<EnergySystemGeneratorBaseSO> objectDataList, List<ApplianceBaseSO> applianceDataList, GridStructure grid)
    {
        this.objectDataList = objectDataList;
        this.applianceDataList = applianceDataList;
        this.grid = grid;
    }

    public void UpdateSystemObjectAttributes()
    {
        objectDataList = grid.GetListOfAllObjects();
        foreach (var structure in objectDataList)
        {
            if (structure.GetType() == typeof(SolarPanelSO))
            {
                UpdateSolarPanelObjectAttributes(structure);
            }
            if (structure.GetType() == typeof(WindTurbineSO))
            {
                UpdateWindTurbineObjectAttributes(structure);
            }
            if (structure.GetType() == typeof(InvertorSO))
            {
                UpdateInvertorObjectAttributes(structure);
            }
            if (structure.GetType() == typeof(HybirdChargeControllerSO))
            {
                UpdateChargeControllerObjectAttributes(structure);
            }
            if (structure.GetType() == typeof(BatterySO))
            {
                UpdateBatteryObjectAttributes(structure);
            }
            if (structure.GetType() == typeof(DieselGeneratorSO))
            {
                UpdateDieselGeneratorObjectAttributes(structure);
            }
        }
    }

    private void UpdateDieselGeneratorObjectAttributes(EnergySystemGeneratorBaseSO structure)
    {
        if (structure.isTurnedOn)
        {
            structure.isRunning = true;
        }
        else
        {
            structure.isRunning = false;
        }
    }
    //reviewed
    private void UpdateBatteryObjectAttributes(EnergySystemGeneratorBaseSO structure)
    {
        if (HasChargeController())
        {
            structure.isConnectedToChargeController = true;
        }
        else
        {
            structure.isConnectedToChargeController = false;
        }

        structure.isRunning = true;       
    }

    //reviewed
    private void UpdateChargeControllerObjectAttributes(EnergySystemGeneratorBaseSO structure)
    {
        if (HasSolarPanel())
        {
            structure.isConnectedToSolarPanel = true;
        }
        else
        {
            structure.isConnectedToSolarPanel = false;
        }

        if (HasInvertor())
        {
            structure.isConnectedToInvertor = true;
        }
        else
        {
            structure.isConnectedToInvertor = false;
        }
        if (HasWindTurbine())
        {
            structure.isConnectedToWindTurbine = true;
        }
        else
        {
            structure.isConnectedToWindTurbine = false;
        }

        if (HasBattery())
        {
            structure.isConnectedToBattery = true;
        }
        else
        {
            structure.isConnectedToBattery = false;
        }

        if (structure.isTurnedOn)
        {
            structure.isRunning = true;
        }
        else
        {
            structure.isRunning = false;
        }

    }



    //reviewed
    private void UpdateWindTurbineObjectAttributes(EnergySystemGeneratorBaseSO structure)
    {

        if (HasChargeController())
        {
            structure.isConnectedToChargeController = true;
        }
        else
        {
            structure.isConnectedToChargeController = false;
        }

        if (HasInvertor())
        {
            structure.isConnectedToInvertor = true;
        }
        else
        {
            structure.isConnectedToInvertor = false;
        }

        structure.isRunning = true;
    }

    //reviewed
    private void UpdateSolarPanelObjectAttributes(EnergySystemGeneratorBaseSO structure)
    {

        if (HasChargeController())
        {
            structure.isConnectedToChargeController = true;
        }
        else
        {
            structure.isConnectedToChargeController = false;
        }

        if (HasInvertor())
        {
            structure.isConnectedToInvertor = true;
        }
        else
        {
            structure.isConnectedToInvertor = false;
        }

        structure.isRunning = true;
    }


    //reviewed
    private void UpdateInvertorObjectAttributes(EnergySystemGeneratorBaseSO structure)
    {
        if (HasSolarPanel())
        {
            structure.isConnectedToSolarPanel = true;
        }
        else
        {
            structure.isConnectedToSolarPanel = false;
        }

        if (HasChargeController())
        {
            structure.isConnectedToChargeController = true;
        }
        else
        {
            structure.isConnectedToChargeController = false;
        }

        if (HasInvertor())
        {
            structure.isConnectedToInvertor = true;
        }
        else
        {
            structure.isConnectedToInvertor = false;
        }

        if (HasWindTurbine())
        {
            structure.isConnectedToWindTurbine = true;
        }
        else
        {
            structure.isConnectedToWindTurbine = false;
        }

        if (structure.isTurnedOn)
        {
            structure.isRunning = true;
        }
        else
        {
            structure.isRunning = false;
        }
    }

    #region Search Object On Map
    public bool HasSolarPanel()
    {
        objectDataList = grid.GetListOfAllObjects();
        foreach (var structure in objectDataList)
        {
            if (structure.GetType() == typeof(SolarPanelSO))
            {
                return true;
            }

        }
        return false;
    }

    public bool HasWindTurbine()
    {
        objectDataList = grid.GetListOfAllObjects();
        foreach (var structure in objectDataList)
        {
            if (structure.GetType() == typeof(WindTurbineSO))
            {
                return true;
            }

        }
        return false;
    }

    public bool HasInvertor()
    {
        objectDataList = grid.GetListOfAllObjects();
        foreach (var structure in objectDataList)
        {
            if (structure.GetType() == typeof(InvertorSO))
            {
                return true;
            }

        }
        return false;
    }

    public bool HasChargeController()
    {
       objectDataList = grid.GetListOfAllObjects();
        foreach (var structure in objectDataList)
        {
            if (structure.GetType() == typeof(HybirdChargeControllerSO))
            {
                return true;
            }

        }
        return false;
    }

    public bool HasBattery()
    {
        objectDataList = grid.GetListOfAllObjects();
        foreach (var structure in objectDataList)
        {
            if (structure.GetType() == typeof(BatterySO))
            {
                return true;
            }

        }
        return false;
    }
    #endregion

}
