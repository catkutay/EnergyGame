using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ObjectRepositoryTests
{
    ObjectRepository objectRepository;
    GameObject testSolarPanelPrefab;
    GameObject testDieselGeneratorPrefab;
    GameObject testWindTurbinePrefab;
    GameObject testInvertorPrefab;
    GameObject testChargeControllerPrefab;
    GameObject testBatteryPrefab;

    //GameObject testSingleStructure;
    //GameObject testZone;
    [OneTimeSetUp]
    public void Init()
    {
        objectRepository = Substitute.For<ObjectRepository>();
        ScriptableObjectCollection collection = new ScriptableObjectCollection();

        testSolarPanelPrefab = new GameObject();
        testDieselGeneratorPrefab = new GameObject();
        testWindTurbinePrefab = new GameObject();
        testInvertorPrefab = new GameObject();
        testChargeControllerPrefab = new GameObject();
        testBatteryPrefab = new GameObject();


        var SolarPanel = new SolarPanelSO();
        SolarPanel.name = "Solar Panel";
        SolarPanel.objectPrefab = testSolarPanelPrefab;
        SolarPanel.objectWidth = 1;
        SolarPanel.objectHeight = 1;
        SolarPanel.objectLength = 1;

        var WindTurbine = new WindTurbineSO();
        WindTurbine.name = "Wind Turbine";
        WindTurbine.objectPrefab = testWindTurbinePrefab;
        WindTurbine.objectWidth = 2;
        WindTurbine.objectHeight = 2;
        WindTurbine.objectLength = 2;

        var DieselGenerator = new DieselGeneratorSO();
        DieselGenerator.name = "Diesel Generator";
        DieselGenerator.objectPrefab = testDieselGeneratorPrefab;
        DieselGenerator.objectWidth = 3;
        DieselGenerator.objectHeight = 3;
        DieselGenerator.objectLength = 3;

        var Invertor = new InvertorSO();
        Invertor.name = "Invertor";
        Invertor.objectPrefab = testInvertorPrefab;
        Invertor.objectWidth = 4;
        Invertor.objectHeight = 4;
        Invertor.objectLength = 4;

        var ChargeController = new HybirdChargeControllerSO();
        ChargeController.name = "Charge Controller";
        ChargeController.objectPrefab = testChargeControllerPrefab;
        ChargeController.objectWidth = 5;
        ChargeController.objectHeight = 5;
        ChargeController.objectLength = 5;

        var Battery = new BatterySO();
        Battery.name = "Battery";
        Battery.objectPrefab = testBatteryPrefab;
        Battery.objectWidth = 6;
        Battery.objectHeight = 6;
        Battery.objectLength = 6;


        collection.solarPanelSO = SolarPanel;
        collection.dieselGeneratorSO = DieselGenerator;
        collection.windTurbineSO = WindTurbine;
        collection.batterySO = Battery;
        collection.invertorSO = Invertor;
        collection.hybirdChargeControllerSO = ChargeController;

        objectRepository.scriptableObjectCollection = collection;
    }


    [Test]
    public void StructureRepositoryEditModeGetSolarPanelPrefabTestPasses()
    {
        GameObject returnObject = objectRepository.GetObjectPrefabByName("Solar Panel");
        Assert.AreEqual(testSolarPanelPrefab, returnObject);
    }

    [Test]
    public void StructureRepositoryEditModeGetDieselGeneratorPrefabTestPasses()
    {
        GameObject returnObject = objectRepository.GetObjectPrefabByName("Diesel Generator");
        Assert.AreEqual(testDieselGeneratorPrefab, returnObject);
    }

    [Test]
    public void StructureRepositoryEditModeGetBatteryPrefabTestPasses()
    {
        GameObject returnObject = objectRepository.GetObjectPrefabByName("Battery");
        Assert.AreEqual(testBatteryPrefab, returnObject);
    }

    [Test]
    public void StructureRepositoryEditModeGetWindTurbinePrefabTestPasses()
    {
        GameObject returnObject = objectRepository.GetObjectPrefabByName("Wind Turbine");
        Assert.AreEqual(testWindTurbinePrefab, returnObject);
    }

    [Test]
    public void StructureRepositoryEditModeGetChargeControllerPrefabTestPasses()
    {
        GameObject returnObject = objectRepository.GetObjectPrefabByName("Charge Controller");
        Assert.AreEqual(testChargeControllerPrefab, returnObject);
    }

    [Test]
    public void StructureRepositoryEditModeGetInvertorPrefabTestPasses()
    {
        GameObject returnObject = objectRepository.GetObjectPrefabByName("Invertor");
        Assert.AreEqual(testInvertorPrefab, returnObject);
    }


    [Test]
    public void StructureRepositoryEditModeGetSolarPanelSizeTestPasses()
    {
        List<float> returnList = objectRepository.GetObjectSize("Solar Panel");
        List<float> testList = new List<float>() { 1, 1, 1 };
        Assert.AreEqual(testList, returnList);
    }


    [Test]
    public void StructureRepositoryEditModeGetWindTurbineSizeTestPasses()
    {
        List<float> returnList = objectRepository.GetObjectSize("Wind Turbine");
        List<float> testList = new List<float>() { 2, 2, 2 };
        Assert.AreEqual(testList, returnList);
    }

    [Test]
    public void StructureRepositoryEditModeGetDieselGeneratorSizeTestPasses()
    {
        List<float> returnList = objectRepository.GetObjectSize("Diesel Generator");
        List<float> testList = new List<float>() { 3, 3, 3 };
        Assert.AreEqual(testList, returnList);
    }

    [Test]
    public void StructureRepositoryEditModeGetInvertorSizeTestPasses()
    {
        List<float> returnList = objectRepository.GetObjectSize("Invertor");
        List<float> testList = new List<float>() { 4, 4, 4 };
        Assert.AreEqual(testList, returnList);
    }

    [Test]
    public void StructureRepositoryEditModeGetChargeControllerSizeTestPasses()
    {
        List<float> returnList = objectRepository.GetObjectSize("Charge Controller");
        List<float> testList = new List<float>() { 5,5, 5 };
        Assert.AreEqual(testList, returnList);
    }

    [Test]
    public void StructureRepositoryEditModeGetBatterySizeTestPasses()
    {
        List<float> returnList = objectRepository.GetObjectSize("Battery");
        List<float> testList = new List<float>() { 6, 6, 6 };
        Assert.AreEqual(testList, returnList);
    }

}
