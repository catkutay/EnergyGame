using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class WeatherApiResponseModel
    {
        public string Time { get; set; }
        public float? GHI {get; set; }
        public float? POA_IRRAD { get; set; }
        public float? DNI { get; set; }
        public float? WDIR { get; set; }
        public float? WSPD { get; set; }
        public float? WV { get; set; }
        public float? TEMP { get; set; }
        public float? HUMIDITY { get; set; }
        public float? RAIN { get; set; }

        public WeatherData ExtractWeatherData()
        {
            var data = new WeatherData();

            data.SolarIrradiance = POA_IRRAD ?? default;
            data.WindSpeed = WSPD ?? default;
            data.Temperature = TEMP ?? default;

            return data;
        }

        public void LogData()
        {
            Debug.Log(JsonConvert.SerializeObject(this));
        }
    }

}
