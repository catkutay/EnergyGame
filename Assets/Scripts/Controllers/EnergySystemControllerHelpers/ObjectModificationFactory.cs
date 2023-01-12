using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectModificationFactory
{
    private readonly ObjectModificationHelper objectPlacementHelper;
    private readonly ObjectModificationHelper objectRemoveHelper;

    public ObjectModificationFactory(GridStructure grid, IPlacementController placementController, ObjectRepository objectRepository, ApplianceRepository applianceRepository, IResourceController resourceController)
    {
        objectPlacementHelper = new ObjectPlacementHelper(grid, placementController, objectRepository, applianceRepository, resourceController);
        objectRemoveHelper = new ObjectRemoveHelper(grid, placementController, objectRepository, applianceRepository, resourceController);
    }

    public ObjectModificationHelper GetHelper(Type classType)
    {
        
        if (classType == typeof(PlayerSellingObjectState))
        {
            //Debug.Log(classType);
            return objectRemoveHelper;
        }
        else
        {
            return objectPlacementHelper;
        }
    }
}
