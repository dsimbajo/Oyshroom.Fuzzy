using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oyshroom.Data.Model
{
    [Table("Sensors")]
    public class Sensor : IData
    {
        public int Id { get; set; }

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
