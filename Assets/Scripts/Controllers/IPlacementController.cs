/*======================================================*
 |  Author: Yifan Song
 |  Creation Date: 16/08/2021
 |  Latest Modified Date: 16/08/2021
 |  Description: To place object on the grid/grond
 |  Bugs: N/A
 *=======================================================*/
using System.Collections.Generic;
using UnityEngine;

public interface IPlacementController
{
    GameObject CreateGhostObject(List<Vector3> objectPositions, GameObject objectPrefab);
    void DestroyObjects(IEnumerable<GameObject> objectCollection);
    void DestroySingleObject(GameObject obj);
    void PlaceObjectsOnTheMap(IEnumerable<GameObject> objectCollection);
    void ResetObjectMaterial(GameObject obj);
    void SetObjectForSale(GameObject objectToSell);
}