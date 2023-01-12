using System;
using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PurchasingObjectControllerTests
{
    EnergySystemObjectController purchasingObjectController;
    Material materialTransparent;

    [SetUp]
    public void Init()
    {
        PlacementController placementController = Substitute.For<PlacementController>();

        IResourceController resourceController = Substitute.For<IResourceController>();
        resourceController.CanIBuyIt(default).Returns(true);

        materialTransparent = new Material(Shader.Find("Standard"));
        placementController.transparentMaterial = materialTransparent;

        GameObject ground = new GameObject();
        ground.transform.position = Vector3.zero;
        placementController.ground = ground.transform;

        ObjectRepository objectRepository = Substitute.For<ObjectRepository>();
        ScriptableObjectCollection collection = new ScriptableObjectCollection();
        SolarPanelSO solarPanel = new SolarPanelSO();
        solarPanel.name = "Solar Panel";
        solarPanel.objectWidth = 1;
        solarPanel.objectHeight = 1;
        solarPanel.objectLength = 1;
        GameObject solarPanelChild = new GameObject("Solar Panel", typeof(MeshRenderer));
        solarPanelChild.GetComponent<MeshRenderer>().material.color = Color.blue;
        GameObject solarPanelPrefab = new GameObject("Solar Panel");
        solarPanelChild.transform.SetParent(solarPanelPrefab.transform);
        solarPanel.objectPrefab = solarPanelPrefab;
        collection.solarPanelSO = solarPanel;
        objectRepository.scriptableObjectCollection = collection;

        //purchasingObjectController = new EnergySystemObjectController(1, 10, 10, 10, placementController, objectRepository, applianceRepository, resourceController);
    }

    private Material AccessMaterial(Vector3 inputPosition, Func<GameObject> accessMethod)
    {
        var solarPanelObject = accessMethod();
        Material material = solarPanelObject.GetComponentInChildren<MeshRenderer>().material;
        return material;
    }

    private void PrepareSelling(Vector3 inputPosition)
    {
        purchasingObjectController.ConfirmModification();
        purchasingObjectController.PreparePurchasingObjectController(typeof(PlayerSellingObjectState));
        purchasingObjectController.PrepareObjectForSellingAt(inputPosition, "Solar Panel");
    }

    private Vector3 PreparePlacement()
    {
        Vector3 inputPosition = new Vector3(1, 0, 1);
        string objectName = "Solar Panel";
        purchasingObjectController.PreparePurchasingObjectController(typeof(PlayerPurchasingSolarPanelState));
        purchasingObjectController.PrepareObjectForModification(inputPosition, objectName);
        return inputPosition;
    }

    #region PlacementActionTests
    [UnityTest]
    public IEnumerator PurchasingObjectControllerPlacementCancellationTestPasses()
    {
        Vector3 inputPosition = PreparePlacement();
        purchasingObjectController.CancelModification();
        yield return new WaitForEndOfFrame();
        Assert.IsNull(purchasingObjectController.CheckForStructureInGrid(inputPosition));
    }

    [UnityTest]
    public IEnumerator PurchasingObjectControllerPlacementConfirmationTestPasses()
    {
        Vector3 inputPosition = PreparePlacement();
        purchasingObjectController.ConfirmModification();
        yield return new WaitForEndOfFrame();
        Assert.IsNotNull(purchasingObjectController.CheckForStructureInGrid(inputPosition));
    }

    [UnityTest]
    public IEnumerator PurchasingObjectControllerPlacementWithoutConfirmationTestPasses()
    {
        Vector3 inputPosition = PreparePlacement();
        yield return new WaitForEndOfFrame();
        Assert.IsNull(purchasingObjectController.CheckForStructureInGrid(inputPosition)); ;
    }
    #endregion

    #region MaterialTests
    //Todo
    [UnityTest]
    public IEnumerator PurchasingObjectControllerMaterialChangePlacementPreparationTestPasses()
    {
        Vector3 inputPosition = PreparePlacement();
        Material material = AccessMaterial(inputPosition, () => purchasingObjectController.CheckForOnjectToModifyDictionary(inputPosition));
        yield return new WaitForEndOfFrame();
        Color color = Color.green;
        color.a = 0.5f;
        Assert.AreEqual(material.color, color);
    }

    [UnityTest]
    public IEnumerator PurchasingObjectControllerMaterialChangePlacementConfirmationTestPasses()
    {
        Vector3 inputPosition = PreparePlacement();
        purchasingObjectController.ConfirmModification();
        Material material = AccessMaterial(inputPosition, () => purchasingObjectController.CheckForStructureInGrid(inputPosition));
        yield return new WaitForEndOfFrame();
        Assert.AreEqual(material.color, Color.blue);
    }

    [UnityTest]
    public IEnumerator PurchasingObjectControllerMaterialChangeRemovePreparationTestPasses()
    {
        Vector3 inputPosition = PreparePlacement();
        PrepareSelling(inputPosition);
        purchasingObjectController.PreparePurchasingObjectController(typeof(PlayerSellingObjectState));
        Material material = AccessMaterial(inputPosition, () => purchasingObjectController.CheckForOnjectToModifyDictionary(inputPosition));
        yield return new WaitForEndOfFrame();
        Color color = Color.red;
        color.a = 0.5f;
        Assert.AreEqual(material.color, color);
    }

    [UnityTest]
    public IEnumerator PurchasingObjectControllerMaterialChangeRemoveCancellationTestPasses()
    {
        Vector3 inputPosition = PreparePlacement();
        PrepareSelling(inputPosition);
        purchasingObjectController.CancelModification();
        Material material = AccessMaterial(inputPosition, () => purchasingObjectController.CheckForStructureInGrid(inputPosition));
        yield return new WaitForEndOfFrame();
        Assert.AreEqual(material.color, Color.blue);
    }
    #endregion

    #region RemoveActionTests
    [UnityTest]
    public IEnumerator PurchasingObjectControllerRemoveCancellationTestPasses()
    {
        Vector3 inputPosition = PreparePlacement();
        PrepareSelling(inputPosition);
        purchasingObjectController.PreparePurchasingObjectController(typeof(PlayerSellingObjectState));
        purchasingObjectController.CancelModification();
        yield return new WaitForEndOfFrame();
        Assert.IsNotNull(purchasingObjectController.CheckForStructureInGrid(inputPosition));
    }

    [UnityTest]
    public IEnumerator PurchasingObjectControllerRemoveConfirmationTestPasses()
    {
        Vector3 inputPosition = PreparePlacement();
        PrepareSelling(inputPosition);
        purchasingObjectController.PreparePurchasingObjectController(typeof(PlayerSellingObjectState));
        purchasingObjectController.ConfirmModification();
        yield return new WaitForEndOfFrame();
        Assert.IsNull(purchasingObjectController.CheckForStructureInGrid(inputPosition));
    }

    [UnityTest]
    public IEnumerator PurchasingObjectControllerRemoveWithoutConfirmationTestPasses()
    {
        Vector3 inputPosition = PreparePlacement();
        PrepareSelling(inputPosition);
        purchasingObjectController.PreparePurchasingObjectController(typeof(PlayerSellingObjectState));
        yield return new WaitForEndOfFrame();
        Assert.IsNotNull(purchasingObjectController.CheckForStructureInGrid(inputPosition));
    }
    #endregion
}
