using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class ObjectRemoveHelperTests
{
    GameObject tempObject = null;
    GridStructure grid;
    List<Vector3> gridPositionList1 = new List<Vector3>()
    {
        Vector3.zero
    };

    List<Vector3> gridPositionList2 = new List<Vector3>()
    {
        new Vector3(1, 0, 1)
    };

    ObjectModificationHelper helper;


    [SetUp]
    public void Init()
    {
        ObjectRepository objectRepository = TestHelper.CreateObjectRepositoryContainingSolarPanel();
        IPlacementController placementController = Substitute.For<IPlacementController>();
        tempObject = new GameObject();
        placementController.CreateGhostObject(default, default).ReturnsForAnyArgs(tempObject);
        grid = new GridStructure(1, 10, 10, 10);
        grid.PlaceObjectOnTheGrid(tempObject, gridPositionList1, null, null);
        grid.PlaceObjectOnTheGrid(tempObject, gridPositionList2, null, null);

        IResourceController resourceController = Substitute.For<IResourceController>();
        resourceController.CanIBuyIt(default).Returns(true);

        //helper = new ObjectRemoveHelper(grid, placementController, objectRepository, applianceRepository, resourceController);
    }

    [Test]
    public void ObjectRemoveHelperSelectForRemoveTestPasses()
    {
        helper.PrepareObjectForModification(gridPositionList1[0], "", "", "Energy");
        GameObject objectInDictionary1 = helper.AccessStructureInDictionary(gridPositionList1[0]);
        Assert.AreEqual(tempObject, objectInDictionary1);
    }

    [Test]
    public void ObjectRemoveHelperCancelRemoveTestPasses()
    {
        helper.PrepareObjectForModification(gridPositionList1[0], "", "", "Energy");
        helper.CancelModifications("Energy");
        Assert.IsTrue(grid.IsCellTaken( gridPositionList1));
    }

    [Test]
    public void ObjectRemoveHelperConfirmRemoveTestPasses()
    {
        helper.PrepareObjectForModification(gridPositionList1[0], "", "", "Energy");
        helper.ConfirmModifications("Energy");
        Assert.IsFalse(grid.IsCellTaken(gridPositionList1));
    }

}
