using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyHelper
{
    private int money;

    public MoneyHelper(int startMoneyAmount)
    {
        this.money = startMoneyAmount; // Initialize startMoneyAmount
    }

    // Exploit Money to other classes
    public int Money {
        get => money;
        private set
        {
            if(value < 0)
            {
                money = 0;
                throw new MoneyException("Not enough money");
            }
            money = value;
        }
    }

    public void ReduceMoney(int amount)
    {
        Money -= amount;
    }

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public void CalculateMoney(IEnumerable<EnergySystemGeneratorBaseSO> objects)
    {
        CollectIncome(objects);
        ReduceUpkeep(objects);
    }

    // Todo: While players are clicking on a button to maintanence object, then reduce it.
    private void ReduceUpkeep(IEnumerable<EnergySystemGeneratorBaseSO> objects)
    {
        foreach (var obj in objects)
        {
            Money -= obj.upkeepCost;
        }
    }

    //Todo: Based on the output generated
    private void CollectIncome(IEnumerable<EnergySystemGeneratorBaseSO> objects)
    {
        foreach (var obj in objects)
        {
            Money += obj.GetIncomeRate();
        }
    }
}

