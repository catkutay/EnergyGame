using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TimeController : MonoBehaviour
{
    public WeatherDataController weatherDataController;
    public UIController uIController;
    private float secPerMin = 1f;
    private int minChange = 1;
    private float pausedSecPerMin;
    private int pausedMinChange;
    private string _time, _date;
    int hr, min, day, month, year;
    int maxHr = 24;
    int maxMin = 60;
    private int dataId;
    List<int> maxMonth30Days = new List<int>() { 4, 6, 9, 11 };
    List<int> maxMonth31Days = new List<int>() { 1, 3, 5, 7, 8, 10, 12 };
    List<int> maxMonth29Days = new List<int>() { 2020, 2024, 2028, 2032, 2036, 2040, 2044, 2048 };
    int maxMonth = 12;
    float timer = 0;

    private float solarRadiation = 1f;

    public float SolarRadiation { get => solarRadiation;  }

    public void PrepareTimeController()
    {
        PrepareTimeStartingPoint();
        PrepareUI();
    }

    private void PrepareTimeStartingPoint()
    {
        hr = 7;
        day = 22;
        month = 2;
        year = 2020;
        dataId = 8;
    }

    private void PrepareUI()
    {
        uIController.resumeGameButton.gameObject.SetActive(false);
        uIController.pauseGameButton.gameObject.SetActive(true);
        uIController.pausePlane.gameObject.SetActive(false);
        uIController.increaseGameSpeedButton.onClick.AddListener(IncreaseSpeed);
        uIController.decreaseGameSpeedButton.onClick.AddListener(DecreaseSpeed);
        uIController.pauseGameButton.onClick.AddListener(PauseGame);
        uIController.resumeGameButton.onClick.AddListener(ResumeGame);
    }


    public void UpdateTimeDateString()
    {
        weatherDataController.GetData(dataId);
        solarRadiation = weatherDataController.Poa;
        if (timer >= secPerMin)
        {
            min+=minChange;
            if (min >= maxMin)
            {
                min = 0;
                hr++;
                dataId++;
                weatherDataController.GetData(dataId);
                solarRadiation = weatherDataController.Poa;
                if (hr >= maxHr)
                {
                    hr = 0;
                    day++;
                    UpdateDate();
                }
            }
            SetTimeDateString();
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void UpdateDate()
    {
        if (maxMonth30Days.Contains(month) && day > 30)
        {
            UpdateYear();
        }
        else if(maxMonth31Days.Contains(month) && day > 31)
        {
            UpdateYear();
        }
        else
        {
            if(maxMonth29Days.Contains(year) && day > 29)
            {
                UpdateYear();
            }
            if(!maxMonth29Days.Contains(year) && day > 28)
            {
                UpdateYear();
            }
        }
    }

    private void UpdateYear()
    {
        day = 1;
        month++;
        if (month >= maxMonth)
        {
            month = 1;
            year++;

        }
    }

    void SetTimeDateString()
    {
        if (hr <= 9)
        {
            _time = "0" + hr + ":";
        }
        else
        {
            _time = hr + ":";
        }

        if (min <= 9)
        {
            _time += "0" + min;
        }
        else
        {
            _time +=  min;
        }

        if (month <= 9)
        {
            _date = year + "-" + "0" + month + "-";
        }
        else
        {
            _date += year + month + "-";
        }


        if (day <= 9)
        {
            _date += "0" + day;
        }
        else
        {
            _date += day + "-";
        }
        uIController.timeValue.text = _time;
        uIController.dateValue.text = _date;
    }



    void IncreaseSpeed()
    {
        if (minChange != 0)
        {
            if (minChange <= 55)
            {
                minChange += 5;
            }
            else
            {
                minChange = 60;
            }
        }

        
    }

    void DecreaseSpeed()
    {
        if (minChange != 0)
        {
            if (minChange >= 6)
            {
                minChange -= 5;
            }
            else
            {
                minChange = 1;
            }
        }


    }

    void PauseGame()
    {
        pausedMinChange = minChange;
        minChange = 0;
        pausedSecPerMin = secPerMin;
        secPerMin = 0;
        uIController.resumeGameButton.gameObject.SetActive(true);
        uIController.pausePlane.gameObject.SetActive(true);

    }

    void ResumeGame()
    {
        minChange = pausedMinChange;
        secPerMin = pausedSecPerMin;
        uIController.resumeGameButton.gameObject.SetActive(false);
        uIController.pausePlane.gameObject.SetActive(false);
    }


    public string GetTime()
    {
        return hr + "_" + min;
    }


}
