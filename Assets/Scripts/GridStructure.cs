/*======================================================*
 |  Author: Yifan Song
 |  Creation Date: 16/08/2021
 |  Latest Modified Date: 23/08/2021
 |  Description: To convert mouse position into grid position
 |  Bugs: N/A
 *=======================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridStructure
{
    private int cellSize;  // Cellsize = 10 : If the mouse position is (11,0,3), then the grid position is (1, 0, 0)
    Cell[,,] grid;     // Cell[x, y, z] = [0, 1, 2]
    private int width, height, length;
    public List<List<Vector3>> existedObjectsPositions = new List<List<Vector3>>();
    //public List<ApplianceBaseSO> installedAppliances;

    public GridStructure(int cellSize, int width, int height, int length)
    {
        this.cellSize = cellSize;
        this.width = width;
        this.height = height;
        this.length = length;
        grid = new Cell[this.width, this.height, this.length];        // Grid uses an array to store cells - initialize the array

        // Troubleshooting: if we want to create a 300x300x300 map, the cell size is 3, then we need to set width, height, length to 100x100x100
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int z = 0; z < grid.GetLength(2); z++)
                {
                    grid[x, y, z] = new Cell();
                }
            }
        }
    }

    public ApplianceBaseSO GetApplianceDataFromTheGrid(Vector3 gridPosition)
    {
        Vector3Int cellIndex = CalculateGridIndex(gridPosition);
        return grid[cellIndex.x, cellIndex.y, cellIndex.z].GetApplianceData();
    }

    public IEnumerable<EnergySystemGeneratorBaseSO> GetAllObjects()
    {
        List<EnergySystemGeneratorBaseSO> objectDataList = new List<EnergySystemGeneratorBaseSO>();
        foreach (var list in existedObjectsPositions)
        {
            Vector3 position = list[0];
            var data = grid[(int)position.x, (int)position.y, (int)position.z].GetEnergySystemData();
            if (data != null)
            {
                objectDataList.Add(data);
            }
        }
        return objectDataList;
    }

    public List<EnergySystemGeneratorBaseSO> GetListOfAllObjects()
    {
        List<EnergySystemGeneratorBaseSO> objectDataList = new List<EnergySystemGeneratorBaseSO>();
        foreach (var list in existedObjectsPositions)
        {
            Vector3 position = list[0];
            var data = grid[(int)position.x, (int)position.y, (int)position.z].GetEnergySystemData();
            if (data != null)
            {
                objectDataList.Add(data);
            }
        }
        return objectDataList;
    }

    public IEnumerable<ApplianceBaseSO> GetAllAppliances()
    {
        List<ApplianceBaseSO> applianceDataList = new List<ApplianceBaseSO>();
        foreach (var list in existedObjectsPositions)
        {
            Vector3 position = list[0];
            var data = grid[(int)position.x, (int)position.y, (int)position.z].GetApplianceData();
            if (data != null)
            {
                applianceDataList.Add(data);
            }
        }
        return applianceDataList;
    }

    public List<ApplianceBaseSO> GetListOfAllAppliances(List<ApplianceBaseSO> applianceData)
    {
        List<ApplianceBaseSO> applianceDataList = new List<ApplianceBaseSO>();
        GameObject[] appliances = GameObject.FindGameObjectsWithTag("Appliance");
        foreach (var item in applianceData)
        {
            foreach (var obj in appliances)
            {
                if (obj.name.Split('(')[0].Equals(String.Concat(item.name.Where(c => !Char.IsWhiteSpace(c)))))
                {
                    applianceDataList.Add(item);
                }
            }
        }
        
        /*foreach (var list in existedObjectsPositions)
        {
            Vector3 position = list[0];
            var data = grid[(int)position.x, (int)position.y, (int)position.z].GetApplianceData();
            if (data != null)
            {
                applianceDataList.Add(data);
            }
        }
        */
        return applianceDataList;
    }
    #region InputPosition=>GridPosition=>GridIndex
    // Convert the mouse position (can be float) into grid position (must be int) and get grid position
    public Vector3 CalculateGridPosition(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt((float)inputPosition.x / cellSize);
        int y = Mathf.FloorToInt((float)inputPosition.y / cellSize);
        int z = Mathf.FloorToInt((float)inputPosition.z / cellSize);
        return new Vector3(x * cellSize, y * cellSize, z * cellSize);
    }

    // Convert grid position into grid index 
    public Vector3Int CalculateGridIndex(Vector3 gridPosition)
    {
        return new Vector3Int((int)(gridPosition.x / cellSize), (int)(gridPosition.y / cellSize), (int)(gridPosition.z / cellSize));
    }
    #endregion

    #region Get/Place/RemoveObject
    // To read an object from the cell
    public GameObject GetObjectFromTheGrid(Vector3 gridPosition)
    {
        // Convert grid index into cell index
        Vector3Int cellIndex = CalculateGridIndex(gridPosition);
        return grid[cellIndex.x, cellIndex.y, cellIndex.z].GetObject();

    }

    // To place an object on the empty cell
    public void PlaceObjectOnTheGrid(GameObject objectModel, List<Vector3> gridPositionList, EnergySystemGeneratorBaseSO energySystemData, ApplianceBaseSO applianceData)
    {
        foreach (Vector3 p in gridPositionList)
        {
            Vector3Int cellIndex = CalculateGridIndex(p);

            if (CheckGridIndexInRange(cellIndex))
            {
                grid[cellIndex.x, cellIndex.y, cellIndex.z].SetObject(objectModel, energySystemData, applianceData);
            }
            else
            {
                throw new IndexOutOfRangeException("No Index" + cellIndex + "in grid");
            }
        }
        AddPositionListToExistingObjectPositionList(gridPositionList);
    }

    public void RemoveObjectFromTheGrid(List<Vector3> gridPositionList)
    {
        // Convert grid index into cell index
        foreach (var position in gridPositionList)
        {
            Vector3Int cellIndex = CalculateGridIndex(position);
            grid[cellIndex.x, cellIndex.y, cellIndex.z].RemoveObject();
        }
        RemovePositionListFromExistingObjectPositionList(gridPositionList);
    }
    #endregion

    //TODO:Editmode Test
    public EnergySystemGeneratorBaseSO GetEnergySystemDataFromTheGrid(Vector3 gridPosition)
    {
        Vector3Int cellIndex = CalculateGridIndex(gridPosition);
        return grid[cellIndex.x, cellIndex.y, cellIndex.z].GetEnergySystemData();
    }


    // To check if the cell index is in the range of gird(ground map)
    public bool CheckGridIndexInRange(Vector3Int cellIndex)
    {
        if (cellIndex.x >= 0 && cellIndex.x <= grid.GetLength(0) && cellIndex.y >= 0 && cellIndex.y <= grid.GetLength(1) && cellIndex.z >= 0 && cellIndex.z <= grid.GetLength(2))
        {
            return true;
        }
        return false;
    }

    // To check if the cell has been taken by an object
    public bool IsCellTaken(List<Vector3> gridPositionList)
    {
        foreach (Vector3 p in gridPositionList)
        {
            Vector3Int cellIndex = CalculateGridIndex(p);

            if (CheckGridIndexInRange(cellIndex))
            {
                //Debug.Log(grid[cellIndex.x, cellIndex.y, cellIndex.z]);
                if (grid[cellIndex.x, cellIndex.y, cellIndex.z].IsTaken == true)
                    // return the result of if the cell has been taken or not
                    return true;
            }
            else
            {
                //throw new IndexOutOfRangeException("No Index" + cellIndex + "in grid");
            }

        }
        return false;
    }

    public List<Vector3> GetObjectPositionListFromTheGrid(Vector3 gridPosition)
    {
        foreach (var list in existedObjectsPositions)
        {
            if (list.Contains(gridPosition))
                return list;
        }
        return null;

    }

    private void AddPositionListToExistingObjectPositionList(List<Vector3> objectVolumn)
    {
        existedObjectsPositions.Add(objectVolumn);
    }

    private void RemovePositionListFromExistingObjectPositionList(List<Vector3> gridPositionList)
    {
        existedObjectsPositions.Remove(gridPositionList);
    }
}
