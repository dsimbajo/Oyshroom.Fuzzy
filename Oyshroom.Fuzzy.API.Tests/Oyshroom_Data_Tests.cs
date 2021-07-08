using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Oyshroom.Data;
using Oyshroom.Data.Model;
using ServiceStack.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace Oyshroom.Fuzzy.API.Tests
{
    [TestClass]
    public class Oyshroom_Data_Tests
    {
        [TestMethod]
        public void FuzzyLogic_Repository_Get_Test()
        {
            var dbFilePath = $"{ Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))}\\Data\\oyshroom.db";

            var database = new Database(dbFilePath);

            var fuzzyLogicRepository = new FuzzyLogicRepository(database);

            var result = fuzzyLogicRepository.Get();

            Assert.IsTrue(result.Count > 0);

        }

        [TestMethod]
        public void Sensor_Sensor_Repository_Insert_Test()
        {
            var dbFilePath = $"{ Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))}\\Data\\oyshroom.db";

            var database = new Database(dbFilePath);

            var sensorRepository = new SensorRepository(database);

            Random random = new Random();
            var sensor = new Sensor()
            {
                Lux = random.Next(10,100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Humidity = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            var id = sensorRepository.Add(sensor);

            var addedData = sensorRepository.GetById((int)id);

            Assert.AreEqual(addedData.Lux, sensor.Lux);
            Assert.AreEqual(addedData.Temperature, sensor.Temperature);
            Assert.AreEqual(addedData.CO2, sensor.CO2);
            Assert.AreEqual(addedData.Humidity, sensor.Humidity);
            Assert.AreEqual(addedData.DateCreated, sensor.DateCreated);

        }
    }
}
