using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class ObjectModificationHelperTests
{
    GameObject tempObject = null;
    GridStructure grid;
    string solarPanelName = "Solar Panel";
    string windTurbineName = "Wind Turbine";
    string chargeControllerName = "Charge Controller";
    string invertorName = "Invertor";
    string dieselGeneratorName = "Diesel Generator";
    string batteryName = "Battery";
    Vector3 gridPosition1 = Vector3.zero;
    Vector3 gridPosition2 = new Vector3(1, 0, 1);
    Vector3 gridPosition3 = new Vector3(1, 0, 2);
    Vector3 gridPosition4 = new Vector3(1, 0, 3);
    Vector3 gridPosition5 = new Vector3(1, 0, 4);
    Vector3 gridPosition6 = new Vector3(1, 0, 5);

    ObjectModificationHelper helper;

    [SetUp]
    public void Init()
    {
        ObjectRepository objectRepository = TestHelper.CreateObjectRepositoryContainingSolarPanel();
        IPlacementController placementController = Substitute.For<IPlacementController>();
        IResourceController resourceController = Substitute.For<IResourceController>();
        resourceController.CanIBuyIt(default).Returns(true);

        tempObject = new GameObject();
        placementController.CreateGhostObject(default, default).ReturnsForAnyArgs(tempObject);
        grid = new GridStructure(1, 10, 10, 10);
        //helper = new ObjectPlacementHelper(grid, placementController, objectRepository, applianceRepository, resourceController);

    }

    [Test]
    public void ObjectModificationHelperAddPositionTestPasses()
    {
        helper.PrepareObjectForModification(gridPosition1, solarPanelName, "", "Energy");
        GameObject objectInDictionary1 = helper.AccessStructureInDictionary(gridPosition1);
        Assert.AreEqual(tempObject, objectInDictionary1);

        helper.PrepareObjectForModification(gridPosition2, windTurbineName, "", "Energy");
        GameObject objectInDictionary2 = helper.AccessStructureInDictionary(gridPosition2);
        Assert.AreEqual(tempObject, objectInDictionary2);

        helper.PrepareObjectForModification(gridPosition3, chargeControllerName, "", "Energy");
        GameObject objectInDictionary3 = helper.AccessStructureInDictionary(gridPosition3);
        Assert.AreEqual(tempObject, objectInDictionary3);

        helper.PrepareObjectForModification(gridPosition4, invertorName, "", "Energy");
        GameObject objectInDictionary4 = helper.AccessStructureInDictionary(gridPosition4);
        Assert.AreEqual(tempObject, objectInDictionary4);

        helper.PrepareObjectForModification(gridPosition5, batteryName, "", "Energy");
        GameObject objectInDictionary5 = helper.AccessStructureInDictionary(gridPosition5);
        Assert.AreEqual(tempObject, objectInDictionary5);

        helper.PrepareObjectForModification(gridPosition6, dieselGeneratorName, "", "Energy");
        GameObject objectInDictionary6 = helper.AccessStructureInDictionary(gridPosition6);
        Assert.AreEqual(tempObject, objectInDictionary6);
    }

    [Test]
    public void ObjectModificationHelperRemoveFromPositionTestPasses()
    {
        helper.PrepareObjectForModification(gridPosition1, solarPanelName, "", "Energy");
        helper.PrepareObjectForModification(gridPosition1, solarPanelName, "", "Energy");
        GameObject objectInDictionary = helper.AccessStructureInDictionary(gridPosition1);
        Assert.IsNull(objectInDictionary);
    }

    [Test]
    public void ObjectModificationHelperAddMultiplePositionsTestPasses()
    {
        helper.PrepareObjectForModification(gridPosition1, solarPanelName, "", "Energy");
        helper.PrepareObjectForModification(gridPosition2, solarPanelName, "", "Energy");
        GameObject objectInDictionary1 = helper.AccessStructureInDictionary(gridPosition1);
        GameObject objectInDictionary2 = helper.AccessStructureInDictionary(gridPosition2);
        Assert.AreEqual(tempObject, objectInDictionary1);
        Assert.AreEqual(tempObject, objectInDictionary2);
    }

    [Test]
    public void ObjectModificationHelperRemoveFromMultiplePositionsTestPasses()
    {
        helper.PrepareObjectForModification(gridPosition1, solarPanelName, "", "Energy");
        helper.PrepareObjectForModification(gridPosition2, solarPanelName, "", "Energy");
        helper.CancelModifications("Energy");
        GameObject objectInDictionary1 = helper.AccessStructureInDictionary(gridPosition1);
        GameObject objectInDictionary2 = helper.AccessStructureInDictionary(gridPosition2);
        Assert.IsNull(objectInDictionary1);
        Assert.IsNull(objectInDictionary2);
    }

    [Test]
    public void ObjectModificationHelperAddToGridTestPasses()
    {
        helper.PrepareObjectForModification(gridPosition1, solarPanelName, "", "Energy");
        helper.PrepareObjectForModification(gridPosition2, solarPanelName, "", "Energy");
        helper.ConfirmModifications("Energy");
        List<Vector3> gridPositionList1 = new List<Vector3>();
        gridPositionList1.Add(gridPosition1);
        List<Vector3> gridPositionList2 = new List<Vector3>();
        gridPositionList2.Add(gridPosition2);
        Assert.IsTrue(grid.IsCellTaken(gridPositionList1));
        Assert.IsTrue(grid.IsCellTaken(gridPositionList2));
    }


}
