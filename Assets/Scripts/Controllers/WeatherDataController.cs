using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WeatherDataController : MonoBehaviour
{
    public UIController uIController;
    float poa;

    public float Poa { get => poa; set => poa = value; }

    public void GetData(int i) => StartCoroutine(GetData_Coroutine(i));

    IEnumerator GetData_Coroutine(int i)
    {
        string api = "http://eri.teachingforchange.edu.au/weatherapi/data";
        //string api = "http://127.0.0.1:8000/weatherapi/data";
        string apiWithDataId = api + "/" + i;
        using (UnityWebRequest request = UnityWebRequest.Get(apiWithDataId))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
               // Debug.Log(request.error);
            }
            else
            {
                ProcessJsonData(request);
            }
        }
    }

    private void ProcessJsonData(UnityWebRequest request)
    {
        Weather weather = JsonUtility.FromJson<Weather>(request.downloadHandler.text);
        uIController.SetWeatherValue("Sunny", weather.TEMP_AVG, weather.POA_AVG, weather.WSPD_AVG);
        poa = weather.POA_AVG;
        
    }
}

public class Weather
{
    public int DataId;
    public string Time;
    public string Timezone;
    public float POA_AVG;
    public float WDIR_AVG;
    public float WSPD_AVG;
    public float TEMP_AVG;

    public Weather(int DataId, string Time, string Timezone, float POA_AVG, float WDIR_AVG, float WSPD_AVG, float TEMP_AVG)
    {
        this.DataId = DataId;
        this.Time = Time;
        this.Timezone = Timezone;
        this.POA_AVG = POA_AVG;
        this.WDIR_AVG = WDIR_AVG;
        this.WSPD_AVG = WSPD_AVG;
        this.TEMP_AVG = TEMP_AVG;
    }
}