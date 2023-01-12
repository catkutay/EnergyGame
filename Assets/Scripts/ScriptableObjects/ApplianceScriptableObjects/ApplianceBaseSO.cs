using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class ApplianceBaseSO : ScriptableObject
{
    public Sprite objectIcon;
    public string objectName;
    public string objectDescription;
    public string objectType;
    public GameObject objectPrefab;

    public float emissionRate;
    public float emissionAmount;

    public int purchaseCost;
    public int upkeepCost;

    public int purchaseExperience;
    public int greenPointExperience;

    public float powerNeededRate;
    public float powerNeededAmount;

    public int happiness;

    public bool isTurnedOn;
}
