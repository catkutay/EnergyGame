using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPlacementHelper: ObjectModificationHelper
{
    public ObjectPlacementHelper(GridStructure grid, IPlacementController placementController, ObjectRepository objectRepository, ApplianceRepository applianceRepository, IResourceController resourceController) : base(grid, placementController, objectRepository, applianceRepository, resourceController)
    {
    }

    public override void PrepareObjectForModification(Vector3 inputPosition, string objectName, string applianceName, string type)
    {
        base.PrepareObjectForModification(inputPosition, objectName, applianceName, type);

        //Debug.Log(applianceData);
        //Debug.Log(FanLightPrefab);
        GameObject objectPrefab = null;
        if (FanLightPrefab != null)
        {
            objectPrefab = FanLightPrefab;
        } else
        {
            objectPrefab = GetObjectType(type);
        }

        List<Vector3> positionList = GetPositionListByName(inputPosition, objectName, applianceName, type); // Get and update object positions List

        if (!grid.IsCellTaken(positionList) || objectName.Equals("Light")) // If the cells are not taken 
        {
            List<Vector3> currentPositionList = CheckExisting(positionList);
            if (currentPositionList != null)
            {
                if (currentPositionList.Contains(positionList[0]))
                {
                    if (type.Equals("Energy"))
                    {
                        resourceController.AddMoney(energySystemData.purchaseCost);
                        RevokeObjectFromBeingPlaced(currentPositionList);
                    } else
                    {
                        resourceController.AddMoney(applianceData.purchaseCost);
                        RevokeObjectFromBeingPlaced(currentPositionList);
                    }
                }
                else
                {
                    Debug.Log("Cell has been taken by the existing ghost object");
                }
            }
            else if (resourceController.CanIBuyIt(energySystemData.purchaseCost))
            {
                if (type.Equals("Energy"))
                {
                    AddObjectForPlacement(objectPrefab, positionList);
                        
                    resourceController.SpendMoney(energySystemData.purchaseCost);
                }
                else
                {
                    if (!base.ApplianceExists(applianceName))
                    {
                        AddObjectForPlacement(objectPrefab, positionList);
                        resourceController.SpendMoney(applianceData.purchaseCost);
                    } else
                    {
                        Debug.Log("Cell has been taken!!!");
                    }
                }
            }
        }
        else
        {
            //Todo: Create a notification here (Only one item can be generated)
            //foreach(Vector3 p in positionList)
            //{
            //    //Debug.Log(p);
            //}
            Debug.Log("Cell has been taken!!!");
        }
        //}
    }

    /*private bool ApplianceExists(string applianceName)
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
    }*/

    private GameObject GetObjectType(string type)
    {
        GameObject objectPrefab = null;
        if (type.Equals("Energy"))
        {
            objectPrefab = energySystemData.objectPrefab; // Get object prefab
        }
        else
        {
            objectPrefab = applianceData.objectPrefab;
        }
        return objectPrefab;
    }

    private void AddObjectForPlacement(GameObject objectPrefab, List<Vector3> positionList)
    {
        //Debug.Log(positionList.Count);
        objectToBeModified.Add(positionList, placementController.CreateGhostObject(positionList, objectPrefab));
    }

    private void RevokeObjectFromBeingPlaced(List<Vector3> positionList)
    {
        var obj = objectToBeModified[positionList];
        placementController.DestroySingleObject(obj);
        objectToBeModified.Remove(positionList);
    }

    // return positionlist
    private List<Vector3> GetPositionListByName(Vector3 inputPosition, string objectName, string applianceName, string type)
    {
        List<Vector3> positionList = new List<Vector3>();
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition); // Convert mouse position into grid position
        Vector3 p; // temp position
        List<float> objectSize = new List<float>() { 0f, 0f, 0f };
        if (type.Equals("Energy"))
        {
            objectSize = this.objectRepository.GetObjectSize(objectName);
        } else
        {
            for (int i = 0; i < objectSize.Count; i++)
            {
                objectSize[i] = applianceData.objectPrefab.gameObject.transform.localScale[i];
            }
        }
        
        // Adding current mouse/grid position with object volume
        for (int x = (int)gridPosition.x; x < gridPosition.x + objectSize[0]; x++)
        {
            for (int y = (int)gridPosition.y; y < gridPosition.y + objectSize[1]; y++)
            {
                for (int z = (int)gridPosition.z; z < gridPosition.z + objectSize[2]; z++)
                {
                    p.x = x;
                    p.y = y;
                    p.z = z;
                    positionList.Add(p);
                    // ------ Debug ------//
                    //Debug.Log(p);
                }
            }
        }
        return positionList;
    }

    private List<Vector3> CheckExisting(List<Vector3> positionList)
    {
        foreach (var list in objectToBeModified.Keys)
        {
            foreach (var position in positionList)
            {
                if (list.Contains(position))
                    return list;
            }
        }
        return null;
    }

    public override void CancelModifications(string type)
    {
        if (type.Equals("Energy"))
        {
            resourceController.AddMoney(objectToBeModified.Count * energySystemData.purchaseCost);
        } 
        else
        {
            resourceController.AddMoney(objectToBeModified.Count * applianceData.purchaseCost);
        }
        base.CancelModifications(type);
    }
    public override void ConfirmModifications(string type)
    {
        if (type.Equals("Energy"))
        {
            resourceController.AddExperience(objectToBeModified.Count * energySystemData.purchaseExperience);
        } else
        {
            resourceController.AddExperience(objectToBeModified.Count * applianceData.purchaseExperience);
            //resourceController.UpdateLoad(applianceData.powerNeededRate);
        }
        
        base.ConfirmModifications(type);   
    }
}
