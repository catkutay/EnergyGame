using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Appliance Collection", menuName = "EnergySystemSimulation/ApplianceCollection")]
public class ApplianceCollection : ScriptableObject
{
    public ACSO aCSmallSO, aCMediumSO, aCLargeSO;
    public WashingMachineSO washerSmallSO, washerLargeSO;
    public LightSO lightLivingRoomSO, lightKitchenSO, lightBedroomSO, lightLaundrySO;
    public FridgeSO fridgeSmallSO, fridgeLargeSO;
    public FanSO fanLivingRoomSO, fanKitchenSO, fanBedroomSO;
    //public DryerSO dryerSO;

    // 49.42 -23.1 112.91
}
