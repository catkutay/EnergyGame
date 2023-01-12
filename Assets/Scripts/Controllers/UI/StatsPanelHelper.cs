using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class StatsPanelHelper : MonoBehaviour
{
    public UIController uiController;

    public GameObject energySystemsPanel;
    public GameObject renewablesPanel;
    public GameObject statsPanelPrefab;
    public TextMeshProUGUI noNonRenewablesInstalledText, noRenewablesInstalledText, totalEmissionsText, totalOffsetText;

    private TextMeshProUGUI dieselCo2ProducedText, gridPowerCo2ProducedText, solarPanelCo2OffsetText, windTurbineCo2OffsetText;
    private int dieselIndex, gridPowerIndex, solarPanelIndex, windTurbineIndex = -1;

    private float gridEmissions = 0f;
    private float dieselEmissions = 0f;
    private float solarPanelsOffset = 0f;
    private float windTurbineOffset = 0f;

    

    List<EnergySystemGeneratorBaseSO> energySystemData = new List<EnergySystemGeneratorBaseSO>();
    List<EnergySystemGeneratorBaseSO> renewablesData = new List<EnergySystemGeneratorBaseSO>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateAmountProduced", 0.001f, 1.0f);
    }

    private void UpdateAmountProduced()
    {
        if (energySystemData != null)
        {
            if (dieselCo2ProducedText != null)
            {
                dieselCo2ProducedText.text = Math.Round(energySystemData[dieselIndex].emissionGeneratedAmount, 2) + " kg";
                dieselEmissions = float.Parse(dieselCo2ProducedText.text.Split(' ')[0]);
            }
            if (gridPowerCo2ProducedText != null)
            {
                gridPowerCo2ProducedText.text = Math.Round(energySystemData[gridPowerIndex].emissionGeneratedAmount, 2) + " kg";
                gridEmissions = float.Parse(gridPowerCo2ProducedText.text.Split(' ')[0]);
            }
            totalEmissionsText.text = "Total Emissions: " + Math.Round(gridEmissions + dieselEmissions, 2) + " kg";
            /*if (dieselIndex != -1 && gridPowerIndex != -1)
            {
                //totalEmissionsText.text = "Total Emissions: " + (Math.Round(energySystemData[dieselIndex].emissionGeneratedAmount, 2) + Math.Round(energySystemData[gridPowerIndex].emissionGeneratedAmount, 2)) + " kg";
            }*/
        }

        if (renewablesData != null)
        {
            if (solarPanelCo2OffsetText != null)
            {
                solarPanelCo2OffsetText.text = Math.Round(renewablesData[solarPanelIndex].emissionGeneratedAmount) + " %";
                solarPanelsOffset = float.Parse(solarPanelCo2OffsetText.text.Split(' ')[0]);
            }
            if (windTurbineCo2OffsetText != null)
            {
                windTurbineCo2OffsetText.text = Math.Round(renewablesData[windTurbineIndex].emissionGeneratedAmount) + " %";
                windTurbineOffset = float.Parse(windTurbineCo2OffsetText.text.Split(' ')[0]);
            }
            totalOffsetText.text = "Total Offset: " + Math.Round(solarPanelsOffset + windTurbineOffset) + " %";
        }
    }

    private float GetCo2Offset()
    {
        if (energySystemData.Count > 0 /*&& !energySystemData[dieselIndex].isRunning*/)
        {
            return energySystemData[dieselIndex].emissionGeneratedAmount;
        }
        return 0.0f;
    }

    public void CreateEnergySystemsInStatsMenu(List<EnergySystemGeneratorBaseSO> data)
    {
        AddEnergySystemData(data);
        UpdateData();

        if (energySystemData == null || energySystemData.Count < 1)
        {
            //Debug.Log("No non-renewables installed!");
            noNonRenewablesInstalledText.gameObject.SetActive(true);
            ClearPanel(energySystemsPanel.transform);
        }
        else
        {
            /*foreach (var item in energySystemData)
            {
                Debug.Log("energySystemData: " + item.objectName);
            }*/
            noNonRenewablesInstalledText.gameObject.SetActive(false);
            UpdateEnergySystemsInStatsPanel(energySystemsPanel.transform, energySystemData);
        }
    }

    public void CreateRenewablesInStatsMenu(List<EnergySystemGeneratorBaseSO> data)
    {
        AddRenewablesData(data);
        UpdateRenewablesData();

        if (renewablesData == null || renewablesData.Count < 1)
        {
            //Debug.Log("No renewables installed!");
            noRenewablesInstalledText.gameObject.SetActive(true);
            ClearPanel(renewablesPanel.transform);
        }
        else
        {
            noRenewablesInstalledText.gameObject.SetActive(false);
            UpdateEnergySystemsInStatsPanel(renewablesPanel.transform, renewablesData);
        }
    }

    private void ClearPanel(Transform panelTransform)
    {
        for (int i = 0; i < panelTransform.childCount; i++)
        {
            var child = panelTransform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    private void UpdateEnergySystemsInStatsPanel(Transform panelTransform, List<EnergySystemGeneratorBaseSO> data)
    {
        //UpdateData();
        //ClearApplianceSwitches();
        //AddApplianceSwitches();
        //Debug.Log(data.Count + "-" + panelTransform.childCount);
        if (data.Count > panelTransform.childCount)
        {
            int quantityDifference = data.Count - panelTransform.childCount;
            for (int index3 = 0; index3 < quantityDifference; index3++)
            {
                Instantiate(statsPanelPrefab, panelTransform);
            }

            for (int index1 = 0; index1 < panelTransform.childCount; index1++)
            {
                var child = panelTransform.GetChild(index1);

                if (child != null)
                {
                    child.GetComponentsInChildren<TextMeshProUGUI>()[0].text = data[index1].objectName;
                    child.GetComponentsInChildren<Image>()[1].sprite = data[index1].objectIcon;
                    switch (data[index1].objectName)
                    {
                        case ("Diesel Generator"):
                            dieselCo2ProducedText = child.GetComponentsInChildren<TextMeshProUGUI>()[1];
                            dieselIndex = index1;
                            break;
                        case ("On-Grid Power"):
                            gridPowerCo2ProducedText = child.GetComponentsInChildren<TextMeshProUGUI>()[1];
                            gridPowerIndex = index1;
                            break;
                        case ("Solar Panel"):
                            solarPanelCo2OffsetText = child.GetComponentsInChildren<TextMeshProUGUI>()[1];
                            child.GetComponentsInChildren<TextMeshProUGUI>()[0].text = data[index1].objectName + "s";
                            solarPanelIndex = index1;
                            break;
                        case ("Wind Turbine"):
                            windTurbineCo2OffsetText = child.GetComponentsInChildren<TextMeshProUGUI>()[1];
                            windTurbineIndex = index1;
                            break;
                    }
                    /*if (data[index1].GetType() == typeof(DieselGeneratorSO))
                    {
                        dieselCo2ProducedText = child.GetComponentsInChildren<TextMeshProUGUI>()[1];
                        dieselIndex = index1;
                    }
                    else
                    {
                        gridPowerIndex = index1;
                        gridPowerCo2ProducedText = child.GetComponentsInChildren<TextMeshProUGUI>()[1];
                    }*/

                    //objectName = newData[index1].objectName;
                    //InvokeRepeating("UpdateCo2Text", 1.0f, 1.0f);
                }
            }
        }
        else if (data.Count < panelTransform.childCount)
        {
            //Debug.Log(energySystemData.Count + "-" + panelTransform.childCount);
            for (int index2 = 0; index2 < panelTransform.childCount; index2++)
            {
                var child = panelTransform.GetChild(index2);
                foreach (var energySystem in data)
                {
                    //Debug.Log(energySystem.objectName + " = " + child.GetComponentsInChildren<TextMeshProUGUI>()[0].text);
                    if (child.GetComponentsInChildren<TextMeshProUGUI>()[0].text.Equals(energySystem.objectName) == false)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }
    }

    private void AddEnergySystemData(List<EnergySystemGeneratorBaseSO> data)
    {
        if (data != null)
        {
            energySystemData = new List<EnergySystemGeneratorBaseSO>(data);
        }
    }

    private void AddRenewablesData(List<EnergySystemGeneratorBaseSO> data)
    {
        if (data != null)
        {
            foreach (var energySystem in data)
            {
                if (!renewablesData.Find(item => item.objectName == energySystem.objectName))
                {
                    renewablesData.Add(energySystem);
                }
            }
        }
    }

    private void UpdateData()
    {
        if (energySystemData != null)
        {
            int i = energySystemData.Count - 1;
            while ( i >= 0 )
            {
                if (energySystemData[i].GetType() != typeof(DieselGeneratorSO) && energySystemData[i].GetType() != typeof(PowerLinesSO))
                {
                    energySystemData.Remove(energySystemData[i]);
                    
                }
                --i;
            }
        }
    }

    private void UpdateRenewablesData()
    {
        if (renewablesData != null)
        {
            int i = renewablesData.Count - 1;
            while (i >= 0)
            {
                if (renewablesData[i].GetType() != typeof(SolarPanelSO) && renewablesData[i].GetType() != typeof(WindTurbineSO))
                {
                    renewablesData.Remove(renewablesData[i]);

                }
                --i;
            }
        }
    }
}