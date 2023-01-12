using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class PlacementControllerTests
{
    Material materialTransparent;
    PlacementController placementController;
    GameObject testGameObject;

    List<Vector3> gridPosition1 = new List<Vector3>() { Vector3.zero };

    [SetUp]
    public void Init()
    {
        GameObject ground = new GameObject();
        ground.transform.position = Vector3.zero;
        testGameObject = TestHelper.GetSolarPanelGameObjectWithMaterial();
        materialTransparent = new Material(Shader.Find("Standard"));

        placementController = Substitute.For<PlacementController>();
        placementController.ground = ground.transform;
        placementController.transparentMaterial = materialTransparent;
    }


    [UnityTest]
    public IEnumerator PlacementControllerCreateGhostStructureTestPasses()
    {
        GameObject ghostObject = placementController.CreateGhostObject(gridPosition1, testGameObject);
        yield return new WaitForEndOfFrame();
        Color color = Color.green;
        color.a = 0.5f;
        foreach (var renderer in ghostObject.GetComponentsInChildren<MeshRenderer>())
        {
            Assert.AreEqual(renderer.material.color, color);
        }
    }

    [UnityTest]
    public IEnumerator PlacementControllerPlaceStructureOnTheMapMaterialTestPasses()
    {
        GameObject ghostObject = placementController.CreateGhostObject(gridPosition1, testGameObject);
        placementController.PlaceObjectsOnTheMap(new List<GameObject>() { ghostObject });
        yield return new WaitForEndOfFrame();
        foreach (var renderer in ghostObject.GetComponentsInChildren<MeshRenderer>())
        {
            Assert.AreEqual(renderer.material.color, Color.blue);
        }
    }

    [UnityTest]
    public IEnumerator PlacementControllerDestroyStructureTestPasses()
    {
        placementController.SetObjectForSale(testGameObject);
        yield return new WaitForEndOfFrame();
        Color color = Color.red;
        color.a = 0.5f;
        foreach (var renderer in testGameObject.GetComponentsInChildren<MeshRenderer>())
        {
            Assert.AreEqual(renderer.material.color, color);
        }
    }
}
