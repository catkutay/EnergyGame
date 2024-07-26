using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

[CreateAssetMenu(fileName = "New Wind Turbine", menuName = "EnergySystemSimulation/ObjectData/WindTurbine")]
public class WindTurbineSO : EnergySystemGeneratorBaseSO
{
    public float cutInSpeed;
    public float cutOutSpeed;
    public float turbineEfficiency;
    public float turbineCrossSection;

    internal void RemoveSystem(EnergySystemGeneratorBaseSO energySystemGeneratorBaseSO)
    {
        throw new NotImplementedException();
    }

    override public float GetPowerOutput(WeatherData weatherData)
    {
        if (weatherData.WindSpeed < cutInSpeed)
        {
            return 0f;
        }

        if (weatherData.WindSpeed > cutOutSpeed)
        {
            return 0f;
        }

        return CurrentPowerOutput(weatherData);
    }

    internal float CurrentPowerOutput(WeatherData weatherData)
    {
        var result = turbineEfficiency * EnergyInWind(turbineCrossSection, weatherData.WindSpeed);
        //Debug.Log($"Wind Turbine Output: {result}");
        return result;
    }

    internal float EnergyInWind(float turbineCrossSection, float windSpeed)
    {
        var airDensity = 1.225f; //Average air density at sea level

        var energyWatt = (float) 0.5f * airDensity * turbineCrossSection * (windSpeed * windSpeed * windSpeed);
        //Debug.Log($"Wind Turbine Input: Air Density: {airDensity}, Tubine Cross Section: {turbineCrossSection}, Turbine Efficency: {turbineEfficiency}, Wind Speed: {windSpeed}.");

        return energyWatt;
    }
}
