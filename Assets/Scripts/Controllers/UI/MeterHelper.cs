using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeterHelper : MonoBehaviour
{
    // Content
    public float currentValue;

    [Range(0, 100)] public int speed;

    public float maxValue = 50;

    public float targetPercent;

    // Resources
    public Image powerBar;
    public Image loadBar;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI loadText;
    public TextMeshProUGUI helpText;


    private float powerRate = 0f;
    private float loadRate = 0;


    private float targetPowerRate;
    private float targetLoadRate;

    public float TargetLoadRate { get => targetLoadRate; set => targetLoadRate = value; }
    public float LoadRate { get => loadRate; set => loadRate = value; }
    public float TargetPowerRate { get => targetPowerRate; set => targetPowerRate = value; }

    void Start()
    {
        UpdateMeterUI();
        speed = 5;

    }

    void Update()
    {
        //Debug.Log(TargetLoadRate);
        if (LoadRate < targetLoadRate)
        {
            float diff1 = speed * Time.deltaTime;
            float diff2 = targetLoadRate - loadRate;
            if (diff1 < diff2)
                LoadRate += speed * Time.deltaTime;
            else
                loadRate = targetLoadRate;
            
        }
        else if (LoadRate > targetLoadRate)
        {

            float diff1 = speed * Time.deltaTime;
            float diff2 = loadRate - targetLoadRate;
            if (diff1 < diff2)
                LoadRate -= speed * Time.deltaTime;
            else
                loadRate = targetLoadRate;
        }

        if (powerRate < targetPowerRate )
        {
            float diff1 = speed * Time.deltaTime;
            float diff2 = targetPowerRate - powerRate;
            if (diff1 < diff2)
                powerRate += speed * Time.deltaTime;
            else
                powerRate = targetPowerRate;

        }
        else if (powerRate > targetPowerRate)
        {

            float diff1 = speed * Time.deltaTime;
            float diff2 = powerRate - targetPowerRate;
            if (diff1 < diff2)
                powerRate -= speed * Time.deltaTime;
            else
                powerRate = targetPowerRate;
        }

        UpdateMeterUI();

    }

    public void UpdateMeterUI()
    {
        powerBar.fillAmount = powerRate / maxValue;
        powerText.text = ((float)powerRate).ToString("F0")+" kwh";
  

        loadBar.fillAmount = LoadRate / maxValue;
        loadText.text = ((float)LoadRate).ToString("F0") + " kwh";

        if(powerRate<loadRate)
        {
            //helpText.text = "Error: Overload, please set the load to a lower number or buy more energy system components. Otherwise it will be set to Zero automatically once all power has been used. ";
            helpText.text = "Your system is Overload. Please Take Action ASAP.";
        }
        else
        {
            helpText.text = "Your system is healthy.";
        }    
    }


}
