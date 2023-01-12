using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplianceObjectController
{

    GridStructure grid;
    IPlacementController placementController;
    ObjectRepository objectRepository;
    ApplianceRepository applianceRepository;
    ObjectModificationFactory objectModificationFactory;
    ObjectModificationHelper objectModificationHelper;
    ObjectUpdateHelper objectUpdateHelper;
    //CameraMovement cameraMovement;

    public ApplianceObjectController(int cellSize, int width, int height, int length, IPlacementController placementController, ObjectRepository objectRepository, ApplianceRepository applianceRepository, IResourceController resourceController)
    {
        grid = new GridStructure(cellSize, width, height, length);
        this.objectRepository = objectRepository;
        this.applianceRepository = applianceRepository;
        this.placementController = placementController;
        objectModificationFactory = new ObjectModificationFactory(grid, placementController, objectRepository, applianceRepository, resourceController);
        objectUpdateHelper = new ObjectUpdateHelper();
    }

    public ApplianceBaseSO GetApplianceDataFromPosition(Vector3 inputPosition)
    {
        List<Vector3> PositionList = new List<Vector3>();
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        PositionList.Add(gridPosition);
        if (grid.IsCellTaken(PositionList))
        {
            return grid.GetApplianceDataFromTheGrid(PositionList[0]);
        }
        return null;
    }
    public IEnumerable<ApplianceBaseSO> GetAllAppliances()
    {
        return grid.GetAllAppliances();
    }

    public List<ApplianceBaseSO> GetListOfAllAppliances()
    {
        return grid.GetListOfAllAppliances(applianceRepository.GetApplianceObjects());
    }

    public void CancelModification()
    {
        objectModificationHelper.CancelModifications("Appliance");
    }

    public void PreparePurchasingApplianceController(Type classType)
    {
        //Debug.Log(classType);
        objectModificationHelper = objectModificationFactory.GetHelper(classType);
    }

    public void PrepareApplianceForSellingAt(Vector3 inputPosition, string objectName, string applianceName)
    {
        //try
        //{
            objectModificationHelper.PrepareObjectForModification(inputPosition, objectName, applianceName, "Appliance");
        //}
        //catch
        //{
            //throw new Exception("No component installed at this position.");
        //}
    }

    public void UpdateSystemAttributesToApplianceData()
    {
        objectUpdateHelper.GetSystemData(grid.GetListOfAllObjects(), grid.GetListOfAllAppliances(applianceRepository.GetApplianceObjects()), grid);
        objectUpdateHelper.UpdateSystemObjectAttributes();
    }

    public void ConfirmModification()
    {
        objectModificationHelper.ConfirmModifications("Appliance");
    }

    public void PrepareApplianceForModification(Vector3 inputPosition, string objectName, string applianceName, GameObject fanLightPrefab)
    {
        //try
        //{
            objectModificationHelper.FanLightPrefab = fanLightPrefab;
            objectModificationHelper.PrepareObjectForModification(inputPosition, objectName, applianceName, "Appliance");
            //Debug.Log(fanLightPrefab);
            
        //}
        //catch
        //{
            //throw new Exception("No such appliance type." + objectName);
        //}
    }

    public ApplianceBaseSO GetExistingFan(string applianceName)
    {
        foreach (var appliance in GetListOfAllAppliances())
        {
            if (appliance.name.Split(' ')[1].Split('(')[0].Equals(applianceName.Split(' ')[1]) && !appliance.name.Split(' ')[0].Equals("Light"))
            {
                return appliance;
            }
        }
        return null;
    }

    public bool LightExists(string applianceName)
    {
        foreach (var appliance in GetListOfAllAppliances())
        {
            if (appliance.name.Split(' ')[1].Equals(applianceName.Split(' ')[1]) && !appliance.name.Split(' ')[0].Equals("Fan"))
            {
                return true;
            }
        }
        return false;
    }
}
