using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oyshroom.Data.Model
{
    [Table("StableTemperatures")]
    public class LatestTemperature
    {
        public int Id { get; set; }

        public double Temperature { get; set; }
    }
}
