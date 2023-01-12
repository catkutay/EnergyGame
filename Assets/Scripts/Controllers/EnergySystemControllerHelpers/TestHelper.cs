using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using UnityEngine;

public static class TestHelper 
{
    public static ObjectRepository CreateObjectRepositoryContainingSolarPanel()
    {
        ObjectRepository objectRepository = Substitute.For<ObjectRepository>();
        ScriptableObjectCollection collection = new ScriptableObjectCollection();

        SolarPanelSO solarPanel = new SolarPanelSO();
        solarPanel.name = "Solar Panel";
        solarPanel.objectWidth = 1;
        solarPanel.objectHeight = 1;
        solarPanel.objectLength = 1;
        solarPanel.objectPrefab = GetSolarPanelGameObjectWithMaterial();
        collection.solarPanelSO = solarPanel;

        WindTurbineSO windTurbine = new WindTurbineSO();
        windTurbine.name = "Wind Turbine";
        windTurbine.objectWidth = 1;
        windTurbine.objectHeight = 1;
        windTurbine.objectLength = 1;
        windTurbine.objectPrefab = GetWindTurbineGameObjectWithMaterial();
        collection.windTurbineSO = windTurbine;

        BatterySO battery = new BatterySO();
        battery.name = "Battery";
        battery.objectWidth = 1;
        battery.objectHeight = 1;
        battery.objectLength = 1;
        battery.objectPrefab = GetBatteryGameObjectWithMaterial();
        collection.batterySO = battery;

        HybirdChargeControllerSO chargeController = new HybirdChargeControllerSO();
        chargeController.name = "Charge Controller";
        chargeController.objectWidth = 1;
        chargeController.objectHeight = 1;
        chargeController.objectLength = 1;
        chargeController.objectPrefab = GetChargeControllerGameObjectWithMaterial();
        collection.hybirdChargeControllerSO = chargeController;

        DieselGeneratorSO dieselGenerator = new DieselGeneratorSO();
        dieselGenerator.name = "Diesel Generator";
        dieselGenerator.objectWidth = 1;
        dieselGenerator.objectHeight = 1;
        dieselGenerator.objectLength = 1;
        dieselGenerator.objectPrefab = GetDieselGeneratorGameObjectWithMaterial();
        collection.dieselGeneratorSO = dieselGenerator;

        InvertorSO invertor = new InvertorSO();
        invertor.name = "Invertor";
        invertor.objectWidth = 1;
        invertor.objectHeight = 1;
        invertor.objectLength = 1;
        invertor.objectPrefab = GetInvertorGameObjectWithMaterial();
        collection.invertorSO = invertor;


        objectRepository.scriptableObjectCollection = collection;
        return objectRepository;
    }

    public static GameObject GetWindTurbineGameObjectWithMaterial()
    {
        var materialBlue = Resources.Load("BlueMaterial", typeof(Material)) as Material;
        GameObject windTurbineChild = new GameObject("Wind Turbine", typeof(MeshRenderer));
        var rendererWindTurbine = windTurbineChild.GetComponent<MeshRenderer>();
        rendererWindTurbine.material = materialBlue;
        GameObject windTurbinePrefab = new GameObject("Wind Turbine");
        windTurbineChild.transform.SetParent(windTurbinePrefab.transform);
        return windTurbinePrefab;
    }

    public static GameObject GetDieselGeneratorGameObjectWithMaterial()
    {
        var materialBlue = Resources.Load("BlueMaterial", typeof(Material)) as Material;
        GameObject dieselGeneratorChild = new GameObject("Diesel Generator", typeof(MeshRenderer));
        var rendererDieselGenerator = dieselGeneratorChild.GetComponent<MeshRenderer>();
        rendererDieselGenerator.material = materialBlue;
        GameObject dieselGeneratorPrefab = new GameObject("Diesel Generator");
        dieselGeneratorChild.transform.SetParent(dieselGeneratorPrefab.transform);
        return dieselGeneratorPrefab;
    }

    public static GameObject GetInvertorGameObjectWithMaterial()
    {
        var materialBlue = Resources.Load("BlueMaterial", typeof(Material)) as Material;
        GameObject invertorChild = new GameObject("Invertor", typeof(MeshRenderer));
        var rendererInvertor = invertorChild.GetComponent<MeshRenderer>();
        rendererInvertor.material = materialBlue;
        GameObject invertorPrefab = new GameObject("Invertor");
        invertorChild.transform.SetParent(invertorPrefab.transform);
        return invertorPrefab;
    }

    public static GameObject GetChargeControllerGameObjectWithMaterial()
    {
        var materialBlue = Resources.Load("BlueMaterial", typeof(Material)) as Material;
        GameObject chargeControllerChild = new GameObject("Charge Controller", typeof(MeshRenderer));
        var rendererChargeController = chargeControllerChild.GetComponent<MeshRenderer>();
        rendererChargeController.material = materialBlue;
        GameObject chargeControllerPrefab = new GameObject("Charge Controller");
        chargeControllerChild.transform.SetParent(chargeControllerPrefab.transform);
        return chargeControllerPrefab;
    }

    public static GameObject GetBatteryGameObjectWithMaterial()
    {
        var materialBlue = Resources.Load("BlueMaterial", typeof(Material)) as Material;
        GameObject batteryChild = new GameObject("Battery", typeof(MeshRenderer));
        var rendererBattery = batteryChild.GetComponent<MeshRenderer>();
        rendererBattery.material = materialBlue;
        GameObject batteryPrefab = new GameObject("Battery");
        batteryChild.transform.SetParent(batteryPrefab.transform);
        return batteryPrefab;
    }

    public static GameObject GetSolarPanelGameObjectWithMaterial()
    {
        var materialBlue = Resources.Load("BlueMaterial", typeof(Material)) as Material;
        GameObject solarPanelChild = new GameObject("Solar Panel", typeof(MeshRenderer));
        var rendererSolarPanel = solarPanelChild.GetComponent<MeshRenderer>();
        rendererSolarPanel.material = materialBlue;
        GameObject solarPanelPrefab = new GameObject("Solar Panel");
        solarPanelChild.transform.SetParent(solarPanelPrefab.transform);
        return solarPanelPrefab;

    }
}
