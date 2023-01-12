using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectModificationHelper 
{
    public GridStructure grid;
    protected readonly IPlacementController placementController;
    protected readonly ObjectRepository objectRepository;
    protected readonly ApplianceRepository applianceRepository;
    protected Dictionary<List<Vector3>, GameObject> objectToBeModified = new Dictionary<List<Vector3>, GameObject>();
    protected EnergySystemGeneratorBaseSO energySystemData;
    protected ApplianceBaseSO applianceData;
    protected IResourceController resourceController;

    private GameObject fanLightPrefab;

    public GameObject FanLightPrefab { get => fanLightPrefab; set => fanLightPrefab = value; }

    public ObjectModificationHelper(GridStructure grid, IPlacementController placementController, ObjectRepository objectRepository, ApplianceRepository applianceRepository, IResourceController resourceController)
    {
        this.grid = grid;
        this.placementController = placementController;
        this.objectRepository = objectRepository;
        this.applianceRepository = applianceRepository;
        this.resourceController = resourceController;
        energySystemData = ScriptableObject.CreateInstance<NullObjectSO>();
        applianceData = ScriptableObject.CreateInstance<NullApplianceSO>();
    }

    public GameObject AccessStructureInDictionary(Vector3 gridPosition)
    {
        foreach (var list in objectToBeModified.Keys)
        {
            if (list.Contains(gridPosition))
            {
                return objectToBeModified[list];
            }
        }
        return null;
    }

    public virtual void CancelModifications(string type)
    {
        placementController.DestroyObjects(objectToBeModified.Values);
        ResetHelperData();
    }

    public virtual void ConfirmModifications(string type)
    {
        placementController.PlaceObjectsOnTheMap(objectToBeModified.Values);
        foreach (var keyValuePair in objectToBeModified)
        {
            if (type.Equals("Energy"))
            {
                grid.PlaceObjectOnTheGrid(keyValuePair.Value, keyValuePair.Key, GameObject.Instantiate(energySystemData), GameObject.Instantiate(applianceData));
                
            } else
            {
                grid.PlaceObjectOnTheGrid(keyValuePair.Value, keyValuePair.Key, GameObject.Instantiate(energySystemData), GameObject.Instantiate(applianceData));
                //grid.installedAppliances.Add(applianceData);
            }
                //grid.PlaceObjectOnTheGrid(keyValuePair.Value, keyValuePair.Key, energySystemData);
        }
        ResetHelperData();
    }

    public virtual void PrepareObjectForModification(Vector3 inputPosition, string objectName, string applianceName, string type)
    {
        if (type.Equals("Energy"))
        {
            if (energySystemData.GetType() == typeof(NullObjectSO))
            {
                energySystemData = this.objectRepository.GetEnergySystemData(objectName);
            }
        } 
        else if (type.Equals("Appliance"))
        {
            if (applianceData.GetType() == typeof(NullApplianceSO))
            {
                applianceData = this.applianceRepository.GetApplianceData(objectName, applianceName);
            }
        }
        
    }

    public virtual bool ApplianceExists(string applianceName)
    {
        List<ApplianceBaseSO> applianceList = grid.GetListOfAllAppliances(applianceRepository.GetApplianceObjects());
        foreach (var appliance in applianceList)
        {
            if (appliance.name.Split('(')[0].Equals(applianceName))
            {
                return true;
            }
        }
        return false;
    }

    private void ResetHelperData()
    {
        objectToBeModified.Clear();
        energySystemData = ScriptableObject.CreateInstance<NullObjectSO>();
        applianceData = ScriptableObject.CreateInstance<NullApplianceSO>();
    }
}
