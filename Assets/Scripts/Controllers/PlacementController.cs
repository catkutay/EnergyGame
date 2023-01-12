/*======================================================*
 |  Author: Yifan Song
 |  Creation Date: 16/08/2021
 |  Latest Modified Date: 16/08/2021
 |  Description: To place object on the grid/grond
 |  Bugs: N/A
 *=======================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour, IPlacementController
{
    public Transform ground; // Make ground posistion univiersal
    public Material transparentMaterial;
    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>();

    public GameObject CreateGhostObject(List<Vector3> objectPositions, GameObject objectPrefab)
    {
        GameObject newObject = Instantiate(objectPrefab, ground.position + objectPositions[0], objectPrefab.transform.rotation);
        Color colorToSet = Color.green;
        colorToSet.a = 0.5f;
        ModifyObjectPrefabColor(newObject, colorToSet);
        return newObject;
    }

    private void ModifyObjectPrefabColor(GameObject newObject, Color colorToSet)
    {
        //Debug.Log(newObject.transform.childCount);
        foreach (Transform child in newObject.transform)
        {
            var renderer = child.GetComponent<MeshRenderer>();
            //Debug.Log(renderer);
            if (originalMaterials.ContainsKey(child.gameObject) == false)
            {
                originalMaterials.Add(child.gameObject, renderer.materials);
                //Debug.Log(originalMaterials.ContainsKey(child.gameObject));
            }

            Material[] materialsToSet = new Material[renderer.materials.Length];
            for (int i = 0; i < materialsToSet.Length; i++)
            {
                materialsToSet[i] = transparentMaterial;
                materialsToSet[i].color = colorToSet;
            }
            renderer.materials = materialsToSet;
        }
    }

    public void PlaceObjectsOnTheMap(IEnumerable<GameObject> objectCollection)
    {
        foreach (var obj in objectCollection)
        {
            ResetObjectMaterial(obj);
        }
        originalMaterials.Clear();
    }

    public void ResetObjectMaterial(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            //Debug.Log(child.gameObject);
            var renderer = child.GetComponent<MeshRenderer>();
            //Debug.Log(originalMaterials.ContainsKey(child.gameObject));
            //Debug.Log(child.gameObject);``
            if (originalMaterials.ContainsKey(child.gameObject))
            {
                renderer.materials = originalMaterials[child.gameObject];
            }
        }
    }

    public void DestroyObjects(IEnumerable<GameObject> objectCollection)
    {
        foreach (var obj in objectCollection)
        {
            DestroySingleObject(obj);
        }
        originalMaterials.Clear();
    }

    public void DestroySingleObject(GameObject obj)
    {
        Destroy(obj);
    }


    public void SetObjectForSale(GameObject objectToSell)
    {
        Color colorToSet = Color.red;
        colorToSet.a = 0.5f;
        ModifyObjectPrefabColor(objectToSell, colorToSet);
    }
}
