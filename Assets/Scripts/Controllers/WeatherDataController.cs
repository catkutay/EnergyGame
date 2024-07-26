using Assets.Scripts.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

public class WeatherDataController : MonoBehaviour
{
    public UIController uIController;
    WeatherData data;

    public WeatherData Data { get => data; set => data = value; }

    public void GetData(int i) => StartCoroutine(GetData_Coroutine(i));

    IEnumerator GetData_Coroutine(int i)
    {
        //Debug.Log($"Weather Api Call for Index: {i}");
        string api = "http://eri.teachingforchange.edu.au/weather/api";
        //string api = "http://127.0.0.1:8000/weatherapi/data";
        string apiWithDataId = api + $"?format=json&id={i}";
        using (UnityWebRequest request = UnityWebRequest.Get(apiWithDataId))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError || request.downloadHandler.text.Equals("[]"))
            {
                if (data is null)
                {
                    data = new WeatherData();
                }

                data.RandomisedDataVariation();
            }
            else
            {
                ProcessJsonData(request);
            }
        }
    }

    private void ProcessJsonData(UnityWebRequest request)
    {
        var apiResponse = JsonConvert.DeserializeObject<WeatherApiResponseModel>(request.downloadHandler.text);

        data = apiResponse.ExtractWeatherData();
        uIController.SetWeatherValue("Sunny", data.Temperature, data.SolarIrradiance, data.WindSpeed);
    }
}