using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Solar Panel", menuName = "EnergySystemSimulation/ObjectData/SolarPanel")]
public class SolarPanelSO : EnergySystemGeneratorBaseSO
{
    public float efficiency_dust;
    public float efficiency_temperature;



    public float GetEfficiency()
    {
        return efficiency_dust + efficiency_temperature;
    }


}
