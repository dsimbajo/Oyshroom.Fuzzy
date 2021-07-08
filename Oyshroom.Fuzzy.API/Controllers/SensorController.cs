using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Oyshroom.Data;
using Oyshroom.Domain;
using Oyshroom.Domain.Model;
using Oyshroom.Fuzzy.API.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Oyshroom.Fuzzy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly SensorService _sensorService;
        private readonly FuzzyLogicService _fuzzyLogicService;
        private readonly LoggerService _loggerService;
        private readonly RpiService _rpiService;

        public SensorController(SensorService sensorService, RpiService rpiService, FuzzyLogicService fuzzyLogicService, LoggerService loggerService)
        {
            try
            {
                if (sensorService == null || rpiService == null || fuzzyLogicService == null || loggerService == null)
                {
                    throw new Exception("Dependency Injection Error some dependencies are null");
                }

                _sensorService = sensorService;
                _rpiService = rpiService;
                _fuzzyLogicService = fuzzyLogicService;
                _loggerService = loggerService;
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, sensorService,rpiService,fuzzyLogicService,loggerService);
                throw ex;
            }
        
        }

        [HttpPost]
        [Route("sensors")]
        public IActionResult PostSensorReadings(decimal lux, decimal temperature, decimal humidity, decimal co2)
        {
            var sensorReadings = new SensorEntity((double)lux, (double)temperature, (double)humidity, (double)co2);
            SensorEntity sensorResult = null;

            try
            {

                if (_loggerService == null)
                {
                    throw new Exception("LoggerService is null");
                }

                if (_fuzzyLogicService == null)
                {
                    throw new Exception("FuzzyLogicService is null");
                }

                if (_sensorService == null)
                {
                    throw new Exception("SensorService is null");
                }


                if (_rpiService == null)
                {
                    throw new Exception("RpiService is null");
                }

                sensorResult = _fuzzyLogicService.GetActuators(sensorReadings);

                _sensorService.RecordSensors(sensorResult);

                _rpiService.Trigger(sensorResult);

                var rpiResult = _rpiService.ActuatorStatus;

                return Ok(new SensorResponse()
                {
                    Status = "OK",
                    Message = $" Sensor Readings: {JsonConvert.SerializeObject(sensorResult)}/r/n Actuators - {JsonConvert.SerializeObject(rpiResult)}"

                });
            }
            catch (Exception ex)
            {
                var sensorJsonValues = JsonConvert.SerializeObject(sensorResult);
                
                var errorMessage = $"Error encountered on /api/sensor/sensors with values {sensorJsonValues} - Exception: {ex.Message} - {ex.InnerException?.Message}";

                _loggerService.Error(errorMessage);
                return Problem(detail: $"{ex.Message} - {ex.InnerException?.Message} - {ex.StackTrace}" , title: "RPI Internal Error");
            }
          
        }

        [HttpPost]
        [Route("turnoffpin")]
        public IActionResult TurnOffPin(int pin)
        {
            try
            {
                if (_loggerService == null)
                {
                    throw new Exception("LoggerService is null");
                }

                if (_rpiService == null)
                {
                    throw new Exception("RpiService is null");
                }

                _rpiService.TurnOff(pin);

                return Ok($"Pin {pin} was turned off");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error encountered on /api/sensor/turnoff with values {pin} - Exception: {ex.Message} - {ex.InnerException?.Message}";
                _loggerService.Error(errorMessage);
                return Problem(detail: $"{ex.Message} - {ex.InnerException?.Message}", title: "RPI Internal Error");
            }

        }

        [HttpPost]
        [Route("turnonpin")]
        public IActionResult TurnOnPin(int pin)
        {
            try
            {
                if (_loggerService == null)
                {
                    throw new Exception("LoggerService is null");
                }

                if (_rpiService == null)
                {
                    throw new Exception("RpiService is null");
                }

                _rpiService.TurnOn(pin);

                return Ok($"Pin {pin} was turned on");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error encountered on /api/sensor/turnon with values {pin} - Exception: {ex.Message} - {ex.InnerException?.Message}";
                _loggerService.Error(errorMessage);
                return Problem(detail: $"{ex.Message} - {ex.InnerException?.Message}", title: "RPI Internal Error");
            }

        }

        [HttpGet]
        [Route("extract")]
        public IActionResult ExtractData(string dateFrom, string dateTo)
        {

            try
            {
                if (_loggerService == null)
                {
                    throw new Exception("LoggerService is null");
                }

                if (_sensorService == null)
                {
                    throw new Exception("SensorService is null");
                }

                var stream = _sensorService.GetAllRecords(dateFrom, dateTo);

                string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return new FileContentResult(stream.ToArray(), mimeType)
                {
                    FileDownloadName = "Sensors.xlsx"
                };

            }
            catch (Exception ex)
            {
                var errorMessage = $"Error encountered on /api/sensor/extract with values dateFrom - {dateFrom} and dateTo - {dateTo} - Exception: {ex.Message} - {ex.InnerException?.Message}";
                _loggerService.Error(errorMessage);
                return Problem(detail: ex.Message, title: "Data Extraction Internal Error");
            }
               
        }

        [HttpGet]
        [Route("pinstatus")]
        public IActionResult GetPinStatus()
        {
            try
            {
                if (_loggerService == null)
                {
                    throw new Exception("LoggerService is null");
                }

                if (_rpiService == null)
                {
                    throw new Exception("RpiService is null");
                }

                var status = _rpiService.GetPinStatus();

                return Ok(status);

            }
            catch (Exception ex)
            {

                _loggerService.Error($"Error encountered on /api/sensor/pinstatus - Exception: {ex.Message} - {ex.InnerException?.Message}");
                return Problem(detail: $"{ex.Message} - {ex.InnerException?.Message}", title: "RPI Internal Error");
            }
        }

        [HttpGet]
        [Route("resetstabletemp")]
        public IActionResult ResetStableTemperature()
        {
            try
            {
                if (_loggerService == null)
                {
                    throw new Exception("LoggerService is null");
                }

                if (_sensorService == null)
                {
                    throw new Exception("SensorService is null");
                }

                _sensorService.ResetStableTemperature(); 

                return Ok("Stable Temperature was set");

            }
            catch (Exception ex)
            {

                _loggerService.Error($"Error encountered on /api/sensor/resetstabletemp - Exception: {ex.Message} - {ex.InnerException?.Message}");
                return Problem(detail: $"{ex.Message} - {ex.InnerException?.Message}", title: "RPI Internal Error");
            }
        }

        [HttpGet]
        [Route("turnoffall")]
        public IActionResult TurnOffRpiPins()
        {
            try
            {
                if (_loggerService == null)
                {
                    throw new Exception("LoggerService is null");
                }

                if (_rpiService == null)
                {
                    throw new Exception("RpiService is null");
                }

                _rpiService.TurnOffAllPins();

                return Ok($"Successfully Turned Off All Pins");

            }
            catch (Exception ex)
            {

                _loggerService.Error($"Error encountered on /api/sensor/turnoffall - Exception: {ex.Message} - {ex.InnerException?.Message}");
                return Problem(detail: $"{ex.Message} - {ex.InnerException?.Message}", title: "RPI Internal Error");
            }
        }
    }
}
