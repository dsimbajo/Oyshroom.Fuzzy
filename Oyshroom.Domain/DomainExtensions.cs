using Oyshroom.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oyshroom.Data
{
    public static class DomainExtensions
    {

        public static Sensor ToSensor(this Domain.Model.SensorEntity sensorEntity)
        {
            var sensor = new Sensor()
            {
                Lux = sensorEntity.Lux,
                Temperature = sensorEntity.Temperature,
                Humidity = sensorEntity.Humidity,
                CO2 = sensorEntity.CO2,
                Misting = sensorEntity.Misting,
                Exhaust = sensorEntity.Exhaust,
                Intake = sensorEntity.Intake,
                LED = sensorEntity.LED,
                Sound = sensorEntity.Sound,
                DateCreated = sensorEntity.DateCreated
            };

            return sensor;

        }

        public static int ToBit(this string value)
        {
            return value.Equals("ON") ? 1 : 0; 
        }

    }
}
