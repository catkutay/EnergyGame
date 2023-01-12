using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Object Collection", menuName = "EnergySystemSimulation/ScriptableObjectCollection")]
public class ScriptableObjectCollection : ScriptableObject
{
    public DieselGeneratorSO dieselGeneratorSO;
    public BatterySO batterySO;
    public SolarPanelSO solarPanelSO;
    public WindTurbineSO windTurbineSO;
    public InvertorSO invertorSO;
    public HybirdChargeControllerSO hybirdChargeControllerSO;
    public PowerLinesSO powerLinesSO;
}
