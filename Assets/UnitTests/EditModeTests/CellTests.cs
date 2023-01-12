using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CellTests
{
    [Test]
    public void CellSetGameObjectTestPasses()
    {
        Cell cell = new Cell();
        cell.SetObject(new GameObject(), null, null);
        Assert.IsTrue(cell.IsTaken);
    }

    [Test]
    public void CellSetGameObjectNullTestFails()
    {
        Cell cell = new Cell();
        cell.SetObject(null, null, null);
        Assert.IsFalse(cell.IsTaken);
    }

    [Test]
    public void CellRemoveGameObjectTestFails()
    {
        Cell cell = new Cell();
        cell.SetObject(new GameObject(), null, null);
        cell.RemoveObject();
        Assert.IsFalse(cell.IsTaken);
    }

    [Test]
    public void CellGetEnergySystemDataTestPasses()
    {
        Cell cell = new Cell();
        SolarPanelSO solarPanel = new SolarPanelSO();
        cell.SetObject(new GameObject(), solarPanel, null);
        Assert.AreEqual(solarPanel, cell.GetEnergySystemData());
    }

}
