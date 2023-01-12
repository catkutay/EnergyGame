using System;
public class NullObjectSO:EnergySystemGeneratorBaseSO
{
    private void OnEnable()
    {
        objectName = "nullable object";
        objectPrefab = null;
        purchaseCost = 0;
        upkeepCost = 0;
        incomeRate = 0;


    }
}
