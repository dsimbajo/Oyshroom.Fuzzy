using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oyshroom.Domain.Model
{
    public class SensorEntity
    {
        public SensorEntity()
        {

        }

        public SensorEntity(double lux, double temperature, double humidity, double co2)
        {
            Lux = lux;
            Temperature = temperature;
            Humidity = humidity;
            CO2 = co2;
            DateCreated = DateTime.Now.ToString("s");
        }

        public double Lux { get; set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public double CO2 { get; set; }

        public int Intake { get; set; }

        public int Exhaust { get; set; }

        public int Misting { get; set; }

        public int LED { get; set; }

        public int Sound { get; set; }

        public string DateCreated { get; set; }


      
    }
}
