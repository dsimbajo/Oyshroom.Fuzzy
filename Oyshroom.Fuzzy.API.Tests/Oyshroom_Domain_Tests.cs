using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oyshroom.Data;
using Oyshroom.Domain;
using Oyshroom.Domain.Entity;
using Oyshroom.Domain.Model;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.IO;
using System.Reflection;
using System.Text;

namespace Oyshroom.Fuzzy.API.Tests
{
    [TestClass]
    public class Oyshroom_Domain_Tests
    {
        [TestMethod]
        public void RecordSensors_Tests()
        {
            var dbFilePath = $"{ Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))}\\Data\\oyshroom.db";

            var database = new Database(dbFilePath);

            var repository = new SensorRepository(database);
            var stableTempRepository = new StableTemperatureRepository(database);

            var sensorService = new SensorService(repository, stableTempRepository);

            Random random = new Random();
            var sensorEntity = new SensorEntity()
            {
                Lux = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Humidity = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            int id = sensorService.RecordSensors(sensorEntity);

            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void FuzzyLogicService_Sound_On_Test()
        {
            var dbFilePath = $"{ Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))}\\Data\\oyshroom.db";

            var database = new Database(dbFilePath);

            var fuzzyLogicRepository = new FuzzyLogicRepository(database);
            var sensorRepository = new SensorRepository(database);

            var fuzzyLogicService = new FuzzyLogicService(fuzzyLogicRepository, sensorRepository);

            var sensorEntity = new SensorEntity()
            {
                Lux = 10.00,
                Temperature = 32.56,
                Humidity = 50.00,
                CO2 = 61.24,
                DateCreated = DateTime.Now.ToString("s")
            };

            sensorRepository.Add(sensorEntity.ToSensor());


            Random random = new Random();
            var newSensorEntity = new SensorEntity()
            {
                Lux = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = 25.29,
                Humidity = 83.00,
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            var result = fuzzyLogicService.GetActuators(newSensorEntity);

            newSensorEntity = new SensorEntity()
            {
                Lux = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = 25.29,
                Humidity = 83.00,
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            result = fuzzyLogicService.GetActuators(newSensorEntity);


            newSensorEntity = new SensorEntity()
            {
                Lux = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = 25.29,
                Humidity = 83.00,
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            result = fuzzyLogicService.GetActuators(newSensorEntity);


            Assert.IsTrue(result.Sound.Equals("ON"));

        }

        [TestMethod]
        public void FuzzyLogicService_Unstable_Temperature_Test_Expect_Sound_On_Test()
        {
            var dbFilePath = $"{ Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))}\\Data\\oyshroom.db";

            var database = new Database(dbFilePath);

            var fuzzyLogicRepository = new FuzzyLogicRepository(database);
            var sensorRepository = new SensorRepository(database);

            var fuzzyLogicService = new FuzzyLogicService(fuzzyLogicRepository, sensorRepository);

            var sensorEntity = new SensorEntity()
            {
                Lux = 10.00,
                Temperature = 32.56,
                Humidity = 50.00,
                CO2 = 61.24,
                DateCreated = DateTime.Now.ToString("s")
            };

            sensorRepository.Add(sensorEntity.ToSensor());


            Random random = new Random();
            var newSensorEntity = new SensorEntity()
            {
                Lux = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = 25.29,
                Humidity = 83.00,
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            var result = fuzzyLogicService.GetActuators(newSensorEntity);

            newSensorEntity = new SensorEntity()
            {
                Lux = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = 30,
                Humidity = 83.00,
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            result = fuzzyLogicService.GetActuators(newSensorEntity);


            newSensorEntity = new SensorEntity()
            {
                Lux = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = 25.29,
                Humidity = 83.00,
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            result = fuzzyLogicService.GetActuators(newSensorEntity);


            Assert.IsTrue(result.Sound.Equals("ON"));

        }

        [TestMethod]
        public void FuzzyLogicService_Unstable_Temperature_Test_Expect_Sound_Off_Test()
        {
            var dbFilePath = $"{ Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))}\\Data\\oyshroom.db";

            var database = new Database(dbFilePath);

            var fuzzyLogicRepository = new FuzzyLogicRepository(database);
            var sensorRepository = new SensorRepository(database);

            var fuzzyLogicService = new FuzzyLogicService(fuzzyLogicRepository, sensorRepository);

            var sensorEntity = new SensorEntity()
            {
                Lux = 10.00,
                Temperature = 32.56,
                Humidity = 50.00,
                CO2 = 61.24,
                DateCreated = DateTime.Now.ToString("s")
            };

            sensorRepository.Add(sensorEntity.ToSensor());


            Random random = new Random();
            var newSensorEntity = new SensorEntity()
            {
                Lux = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = 25.29,
                Humidity = 83.00,
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            var result = fuzzyLogicService.GetActuators(newSensorEntity);

            newSensorEntity = new SensorEntity()
            {
                Lux = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = 30,
                Humidity = 83.00,
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            result = fuzzyLogicService.GetActuators(newSensorEntity);


            newSensorEntity = new SensorEntity()
            {
                Lux = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = 25.29,
                Humidity = 70.00,
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            result = fuzzyLogicService.GetActuators(newSensorEntity);


            Assert.IsTrue(result.Sound.Equals("OFF"));

        }

        [TestMethod]
        public void FuzzyLogicService_Sound_Off_Test()
        {
            var dbFilePath = $"{ Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))}\\Data\\oyshroom.db";

            var database = new Database(dbFilePath);

            var fuzzyLogicRepository = new FuzzyLogicRepository(database);
            var sensorRepository = new SensorRepository(database);

            var fuzzyLogicService = new FuzzyLogicService(fuzzyLogicRepository, sensorRepository);

            var sensorEntity = new SensorEntity()
            {
                Lux = 10.00,
                Temperature = 25.29,
                Humidity = 50.00,
                CO2 = 61.24,
                DateCreated = DateTime.Now.ToString("s")
            };

            sensorRepository.Add(sensorEntity.ToSensor());

            sensorRepository.UpdateLatestStableTemperature(25.29);

            Random random = new Random();
            var newSensorEntity = new SensorEntity()
            {
                Lux = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = 25.29,
                Humidity = 70.00,
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            var result = fuzzyLogicService.GetActuators(newSensorEntity);

            newSensorEntity = new SensorEntity()
            {
                Lux = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = 25.29,
                Humidity = 70.00,
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            result = fuzzyLogicService.GetActuators(newSensorEntity);


            newSensorEntity = new SensorEntity()
            {
                Lux = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = 25.29,
                Humidity = 70.00,
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            result = fuzzyLogicService.GetActuators(newSensorEntity);

            Assert.IsTrue(result.Sound.Equals("OFF"));

        }

        [TestMethod]
        public void RpiService_Sound_On_Test()
        {
            var dbFilePath = $"{ Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))}\\Data\\oyshroom.db";

            var database = new Database(dbFilePath);

            var fuzzyLogicRepository = new FuzzyLogicRepository(database);
            var sensorRepository = new SensorRepository(database);

            var fuzzyLogicService = new FuzzyLogicService(fuzzyLogicRepository, sensorRepository);

            var sensorEntity = new SensorEntity()
            {
                Lux = 10.00,
                Temperature = 32.56,
                Humidity = 50.00,
                CO2 = 61.24,
                DateCreated = DateTime.Now.ToString("s")
            };

            sensorRepository.Add(sensorEntity.ToSensor());

            Random random = new Random();
            var newSensorEntity = new SensorEntity()
            {
                Lux = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                Temperature = 25.29,
                Humidity = 83.00,
                CO2 = random.Next(10, 100) + Math.Round((random.NextDouble() / 100), 2),
                DateCreated = DateTime.Now.ToString("s")
            };

            var result = fuzzyLogicService.GetActuators(newSensorEntity);

            var actuatorEvaluator = new ActuatorEvaluator();

            var rpiService = new RpiService(actuatorEvaluator);

            rpiService.Trigger(result);

            var evaluatorResult = rpiService.ActuatorStatus;

            Assert.IsTrue(evaluatorResult.Exhaust == PinValue.Low);
            Assert.AreEqual(evaluatorResult.Intake, PinValue.High);
            Assert.AreEqual(evaluatorResult.Sound, PinValue.High);
            Assert.AreEqual(evaluatorResult.LED, PinValue.High);
            Assert.AreEqual(evaluatorResult.Misting, PinValue.Low);


        }


    }
}
