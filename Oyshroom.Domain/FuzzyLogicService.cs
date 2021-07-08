using Oyshroom.Data;
using Oyshroom.Data.Model;
using Oyshroom.Domain.Entity;
using Oyshroom.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oyshroom.Domain
{
    public class FuzzyLogicService 
    {
        private readonly FuzzyLogicRepository _fuzzyLogicRepository;
        private readonly SensorRepository _sensorRepository;

        public FuzzyLogicService(FuzzyLogicRepository fuzzyLogicRepository, SensorRepository sensorRepository)
        {
            _fuzzyLogicRepository = fuzzyLogicRepository;
            _sensorRepository = sensorRepository;
        }


        public SensorEntity GetActuators(SensorEntity sensorEntity)
        {
            var temperatureValue = GetTemperatureLinguisticValue(sensorEntity.Temperature);
            var humidityValue = GetHumidityLinguisticValue(sensorEntity.Humidity);
            var lightValue = GetLightLinguisticValue(sensorEntity.Lux);
            var co2Value = GetCo2LinguisticValue(sensorEntity.CO2);

            var fuzzyLogics = _fuzzyLogicRepository.Get();

            var result = fuzzyLogics.Where(f => f.Temperature == temperatureValue
               && f.Humidity == humidityValue
               && f.Light == lightValue
               && f.CO2 == co2Value).FirstOrDefault();

            var soundStatus = EvaluateTemperature(sensorEntity);

            sensorEntity.Misting = result.Misting.ToBit();
            sensorEntity.Exhaust = result.Exhaust.Trim() == "EXT" ? 1 : 0;
            sensorEntity.LED = result.LED.ToBit();
            sensorEntity.Sound = soundStatus.ToBit();
            sensorEntity.Intake = result.Exhaust.Trim() == "INT" ? 1 : 0;


            return sensorEntity;
        }


        private string EvaluateTemperature(SensorEntity sensors)
        {
            var result = _sensorRepository.GetLatestTemperature();
            double stableTemperature = _sensorRepository.GetLastStableTemperature();

            var temperaturePercentage = (sensors.Temperature / result) * 100;

            if (sensors.Humidity > 80 && (Math.Abs(temperaturePercentage) >= 60 && Math.Abs(temperaturePercentage) <= 80))
            {
                _sensorRepository.UpdateLatestStableTemperature(sensors.Temperature);
                return "ON";
            }

            if (sensors.Humidity < 80 && sensors.Temperature == stableTemperature)
            {
                _sensorRepository.UpdateLatestStableTemperature(0);
                return "OFF";
            }

            return stableTemperature == 0 ? "OFF" : "ON";

        }


        private string GetCo2LinguisticValue(double co2)
        {
            if (co2 >= 0 && co2 <= 400)
            {
                return "LM";
            }

            if (co2 >= 401 && co2 <= 600)
            {
                return "I";
            }

            if (co2 > 600)
            {
                return "H";
            }

            return "I";
        }

        private string GetLightLinguisticValue(double light)
        {
            if (light >= 0 && light <= 499)
            {
                return "DD";
            }

            if (light >= 500 && light <= 1000)
            {
                return "I";
            }

            if (light > 1000)
            {
                return "B";
            }

            return "I";

        }

        private string GetHumidityLinguisticValue(double humidity)
        {
            if (humidity <= 69)
            {
                return "DM";
            }

            if (humidity <= 70 & humidity <= 90)
            {
                return "I";
            }

            if (humidity >= 90)
            {
                return "M";
            }

            return "I";
        }

        private string GetTemperatureLinguisticValue(double temperature)
        {
            if (temperature <= 21)
            {
                return "C";
            }

            if (temperature >= 22 && temperature <= 28)
            {
                return "N";
            }

            if (temperature >= 29)
            {
                return "H";
            }

            return "N";
        }


    }

}
