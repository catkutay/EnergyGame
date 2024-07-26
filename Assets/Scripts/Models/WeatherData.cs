using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class WeatherData
    {
        public float SolarIrradiance;
        public float WindSpeed;
        public float Temperature;

        /// <summary>
        /// Checks the <c>WeatherData</c> values and populates any unassigned values with the defaut.
        /// </summary>
        public void RandomisedDataVariation()
        {
            //Todo Change SolarIrradiance randomly by a small amount

            //Todo Change WindSpeed randomly by a small amount
        }

        public void LogData()
        {
            Debug.Log($"\nData: Solar Irradiance: {SolarIrradiance} Wind Speed: {WindSpeed}");
        }
    }
}
