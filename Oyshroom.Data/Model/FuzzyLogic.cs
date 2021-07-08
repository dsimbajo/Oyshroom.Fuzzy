using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oyshroom.Data.Model
{
    [Table("FuzzyLogics")]
    public class FuzzyLogic
    {
        [ExplicitKey]
        public int Id { get; set; }

        public string Temperature { get; set; }

        public string Humidity { get; set; }

        public string Light { get; set; }

        public string CO2 { get; set; }

        public string Misting { get; set; }

        public string Exhaust { get; set; }

        public string LED { get; set; }
    }
}
