using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ResourceControllerTests : MonoBehaviour, IResourceController
{
    public int StartMoneyAmount {get;}

    public float MoneyCalculationInterval { get; }

    public int removeCost {get;}

    public void AddExperience(int amount)
    {
        
    }

    public void AddMoney(int amount)
    {
        
    }

    public void CalculatePropertyIncome()
    {
       
    }

    public bool CanIBuyIt(int amount)
    {
        return true;
    }

    public void PrepareResourceController(EnergySystemObjectController purchasingObjectController, ApplianceObjectController applianceObjectController)
    {
        
    }

    public bool SpendMoney(int amount)
    {
        return true;
    }

    public void UpdateLoad(float powerNeededRate)
    {
        throw new System.NotImplementedException();
    }
}
