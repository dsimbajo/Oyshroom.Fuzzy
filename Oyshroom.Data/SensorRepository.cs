using Dapper;
using Dapper.Contrib.Extensions;
using Oyshroom.Data.Model;
using System.Collections.Generic;
using System.Linq;


namespace Oyshroom.Data
{
    public class SensorRepository : Repository<Sensor>
    {
        public SensorRepository(Database database) : base(database)
        {

        }

        public List<Sensor> GetByDate(string fromDate, string toDate)
        {
            string sql = $"SELECT ROUND(Lux,2) Lux,ROUND(Temperature,2) Temperature, ROUND(Humidity,2) Humidity, ROUND(CO2,2) CO2, Intake, Exhaust, Misting, LED, Sound, DateCreated FROM Sensors WHERE DateCreated > DATE('{fromDate}') AND DateCreated < DATE('{toDate}') ORDER BY DateCreated ASC";

            using (var connection = this._database.Connection())
            {
               return connection.Query<Sensor>(sql).ToList();  

            }
        }

        public double GetLatestTemperature()
        {
            string sql = "SELECT ROUND(Temperature,2) Temperature FROM Sensors ORDER BY DateCreated DESC LIMIT 1";

            using (var connection = this._database.Connection())
            {
                var data = connection.Query(sql).FirstOrDefault();

                if (data == null)
                {
                    return 0.00;
                }

                return (double)data.Temperature;
            }
        }

        public double GetLastStableTemperature()
        {
            string sql = "SELECT ROUND(Temperature,2) Temperature FROM StableTemperatures ORDER BY Id DESC LIMIT 1";

            using (var connection = this._database.Connection())
            {
                var data = connection.Query(sql).FirstOrDefault();

                if (data == null)
                {
                    return 0;
                }

                return (double)data.Temperature;
            }
        }


        public long UpdateLatestStableTemperature(double temperature)
        {
            using (var connection = this._database.Connection())
            {
                var newLatest = new LatestTemperature() { Temperature = temperature };

                return connection.Insert<LatestTemperature>(newLatest);
            }
        }
    }
}
