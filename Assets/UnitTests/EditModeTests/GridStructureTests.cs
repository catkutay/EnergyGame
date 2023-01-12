using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class GridStructureTests
{
    GridStructure grid;

    [SetUp]
    public void Init()
    {
        // initialize map=(100x100x100) and cell size=(1)
        grid = new GridStructure(1, 100, 100, 100);
    }

    #region CalculateGridPositionTests
    [Test]
    public void CalculateGridPositionZeroTestPasses()
    {
        Vector3 position = new Vector3(0, 0, 0);
        Vector3 returnPosition = grid.CalculateGridPosition(position);
        Assert.AreEqual(Vector3.zero, returnPosition);
    }

    [Test]
    public void CalculateGridPositionFloatTestPasses()
    {
        Vector3 position = new Vector3(0.9f, 0, 0.9f);
        Vector3 returnPosition = grid.CalculateGridPosition(position);
        Assert.AreEqual(Vector3.zero, returnPosition);
    }

    [Test]
    public void CalculateGridPositionFloatTestFails()
    {
        Vector3 position = new Vector3(1.1f, 0, 0);
        Vector3 returnPosition = grid.CalculateGridPosition(position);
        Assert.AreNotEqual(Vector3.zero, returnPosition);
    }

    [Test]
    public void CalculateGridPositionIntegerTestFails()
    {
        Vector3 position = new Vector3(1, 0, 0);
        Vector3 returnPosition = grid.CalculateGridPosition(position);
        Assert.AreNotEqual((1,0,0), returnPosition);
    }
    #endregion

    #region CalculateGridIndexTests
    [Test]
    public void CalculateGridIndexZeroTestPasses()
    {
        Vector3 position = new Vector3(0, 0, 0);
        Vector3 returnPosition = grid.CalculateGridPosition(position);
        Vector3Int indexInsideGrid = grid.CalculateGridIndex(returnPosition);
        Assert.AreEqual(Vector3Int.zero, indexInsideGrid);
    }

    [Test]
    public void CalculateGridIndexNoneZeroTestPasses()
    {
        Vector3 position = new Vector3(1, 0, 1);
        Vector3 returnPosition = grid.CalculateGridPosition(position);
        Vector3Int indexInsideGrid = grid.CalculateGridIndex(returnPosition);
        Assert.AreEqual(new Vector3Int(1, 0, 1), indexInsideGrid);
    }

    [Test]
    public void CalculateGridIndexNoneZeroTestFails()
    {
        Vector3 position = new Vector3(10, 0, 5);
        Vector3 returnPosition = grid.CalculateGridPosition(position);
        Vector3Int indexInsideGrid = grid.CalculateGridIndex(returnPosition);
        Assert.AreNotEqual(new Vector3Int(1, 0, 1), indexInsideGrid);
    }
    #endregion

    #region CheckGridIndexInRangeTests
    [Test]
    public void CheckGridIndexInRangeTestFails()
    {
        // = CheckGridIndexOutOfRangePasses
        Vector3Int gridIndex = new Vector3Int(101, 0, 0);
        Assert.IsFalse(grid.CheckGridIndexInRange(gridIndex));
    }

    [Test]
    public void CheckGridIndexInRangeTestPasses()
    {
        Vector3Int gridIndex = new Vector3Int(100, 100, 100);
        Assert.IsTrue(grid.CheckGridIndexInRange(gridIndex));
    }
    #endregion

    #region PlaceObjectOnGridPositionTests
    [Test]
    public void PlaceObejctAt10_0_10AndCheckIsTakenTestPasses()
    {
        List<Vector3> position = new List<Vector3>() { new Vector3(10, 0, 10) };
        List<Vector3> returnPosition = new List<Vector3>();
        foreach (var p in position)
        {
            returnPosition.Add(grid.CalculateGridPosition(p));
        }
        GameObject testObject = new GameObject("TestObject");
        grid.PlaceObjectOnTheGrid(testObject, returnPosition, null, null);
        //Assert
        Assert.IsTrue(grid.IsCellTaken(position));
    }

    [Test]
    public void PlaceObjectAtMinConditionAndCheckIsTakenTestPasses()
    {
        List<Vector3> position = new List<Vector3>() { new Vector3(0, 0, 0) };
        List<Vector3> returnPosition = new List<Vector3>();
        foreach (var p in position)
        {
            returnPosition.Add(grid.CalculateGridPosition(p));
        }
        GameObject testObject = new GameObject("TestObject");
        grid.PlaceObjectOnTheGrid(testObject, returnPosition, null, null);
        //Assert
        Assert.IsTrue(grid.IsCellTaken(position));
    }

    [Test]
    public void PlaceObjectAtMaxConditionAndCheckIsTakenTestPasses()
    {
        List<Vector3> position = new List<Vector3>() { new Vector3(99, 99, 99) };
        List<Vector3> returnPosition = new List<Vector3>();
        foreach (var p in position)
        {
            returnPosition.Add(grid.CalculateGridPosition(p));
        }
        GameObject testObject = new GameObject("TestObject");
        grid.PlaceObjectOnTheGrid(testObject, returnPosition, null, null);
        //Assert
        Assert.IsTrue(grid.IsCellTaken(position));
    }

    [Test]
    public void PlaceObjectAt20_0_10AndCheckIsTakenNullObjectTestFails()
    {
        List<Vector3> position = new List<Vector3>() { new Vector3(20, 0, 10) };
        List<Vector3> returnPosition = new List<Vector3>();
        foreach (var p in position)
        {
            returnPosition.Add(grid.CalculateGridPosition(p));
        }
        GameObject testObject = null;
        grid.PlaceObjectOnTheGrid(testObject, returnPosition, null, null);
        //Assert
        Assert.IsFalse(grid.IsCellTaken(position));
    }

    [Test]
    public void PlaceObjectAndCheckIsTakenIndexOutOfRangeTestFail()
    {
        List<Vector3> position = new List<Vector3>() { new Vector3(1010, 1010, 1010) };
        //Act
        //Assert
        Assert.Throws<IndexOutOfRangeException>(() => grid.IsCellTaken(position));
    }
    #endregion

    #region GetObjectFromTheGridPositionTests
    [Test]
    public void RetreiveObjectFromCellTestPasses()
    {
        List<Vector3> position = new List<Vector3>() { new Vector3(99, 99, 99) };
        List<Vector3> returnPosition = new List<Vector3>();
        foreach (var p in position)
        {
            returnPosition.Add(grid.CalculateGridPosition(p));
        }
        GameObject testGameObject = new GameObject("TestGameObject");
        grid.PlaceObjectOnTheGrid(testGameObject, position, null, null);
        GameObject retreivedGameObject = grid.GetObjectFromTheGrid(returnPosition[0]);
        Assert.AreEqual(testGameObject, retreivedGameObject);
        Assert.AreEqual(returnPosition, grid.existedObjectsPositions[0]);
    }
    #endregion

    #region RemoveObjectFromTheGridPositionTests
    [Test]
    public void RetreiveObjectFromCellNullTestPasses()
    {
        List<Vector3> position = new List<Vector3>() { new Vector3(99, 99, 99) };
        List<Vector3> returnPosition = new List<Vector3>();
        foreach (var p in position)
        {
            returnPosition.Add(grid.CalculateGridPosition(p));
        }
        grid.RemoveObjectFromTheGrid(position);
        GameObject retreivedGameObject = grid.GetObjectFromTheGrid(returnPosition[0]);
        Assert.AreEqual(null, retreivedGameObject);
        List<Vector3> emptyList = new List<Vector3>();
        Assert.AreEqual(grid.existedObjectsPositions, emptyList);
    }
    #endregion

    #region GetObjectPositionListFromTheGridTest
    [Test]
    public void GetExistingObjectPositionListFromTheGridTestPasses()
    {
        List<Vector3> position = new List<Vector3>() { new Vector3(99, 99, 99) };
        List<Vector3> returnPosition = new List<Vector3>();
        foreach (var p in position)
        {
            returnPosition.Add(grid.CalculateGridPosition(p));
        }
        GameObject testObject = new GameObject("TestObject");
        grid.PlaceObjectOnTheGrid(testObject, returnPosition, null, null);
        Assert.IsNotNull(grid.GetObjectPositionListFromTheGrid(returnPosition[0]));
    }

    [Test]
    public void GetNoneExistedObjectPositionListFromTheGridTestPasses()
    {
        List<Vector3> position = new List<Vector3>() { new Vector3(99, 99, 99) };
        List<Vector3> returnPosition = new List<Vector3>();
        foreach (var p in position)
        {
            returnPosition.Add(grid.CalculateGridPosition(p));
        }
        GameObject testObject = new GameObject("TestObject");
        grid.PlaceObjectOnTheGrid(testObject, returnPosition, null, null);
        Assert.IsNull(grid.GetObjectPositionListFromTheGrid(Vector3.zero));
    }
    #endregion

    [Test]
    public void GetDataStructureTestPasses()
    {
        SolarPanelSO solarPanel = ScriptableObject.CreateInstance<SolarPanelSO>();
        DieselGeneratorSO dieselGenerator = ScriptableObject.CreateInstance< DieselGeneratorSO>();
        BatterySO battery = ScriptableObject.CreateInstance< BatterySO>();
        InvertorSO invertor = ScriptableObject.CreateInstance< InvertorSO>();
        HybirdChargeControllerSO chargeController = ScriptableObject.CreateInstance< HybirdChargeControllerSO>();
        WindTurbineSO windTurbineSO = ScriptableObject.CreateInstance< WindTurbineSO>();

        GameObject gameObject = new GameObject();
        List<Vector3> positionList1 = new List<Vector3>() { new Vector3(0, 0, 0) };
        List<Vector3> positionList2 = new List<Vector3>() { new Vector3(99, 0, 0) };
        List<Vector3> positionList3 = new List<Vector3>() { new Vector3(99, 0, 99) };
        List<Vector3> positionList4 = new List<Vector3>() { new Vector3(0, 0, 99) };
        List<Vector3> positionList5 = new List<Vector3>() { new Vector3(0, 99, 0) };
        List<Vector3> positionList6 = new List<Vector3>() { new Vector3(99, 99, 0) };
        List<Vector3> positionList7 = new List<Vector3>() { new Vector3(99, 99, 99) };
        List<Vector3> positionList8 = new List<Vector3>() { new Vector3(0, 99, 99) };
        grid.PlaceObjectOnTheGrid(gameObject, positionList1, solarPanel, null);
        grid.PlaceObjectOnTheGrid(gameObject, positionList2, dieselGenerator, null);
        grid.PlaceObjectOnTheGrid(gameObject, positionList3, battery, null);
        grid.PlaceObjectOnTheGrid(gameObject, positionList4, invertor, null);
        grid.PlaceObjectOnTheGrid(gameObject, positionList5, chargeController, null);
        grid.PlaceObjectOnTheGrid(gameObject, positionList6, windTurbineSO, null);
        grid.PlaceObjectOnTheGrid(gameObject, positionList7, chargeController, null);
        grid.PlaceObjectOnTheGrid(gameObject, positionList8, windTurbineSO, null);

        var list = grid.GetAllObjects().ToList();
        Assert.IsTrue(list.Count == 8);
    }
}