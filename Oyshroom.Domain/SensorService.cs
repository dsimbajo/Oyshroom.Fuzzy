using Oyshroom.Data;
using Oyshroom.Data.Model;
using Oyshroom.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Syncfusion.XlsIO;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Oyshroom.Domain
{
    public class SensorService
    {
        private readonly SensorRepository _sensorRepository;
        private readonly StableTemperatureRepository _stableTemperatureRepository;

        public SensorService(SensorRepository sensorRepository, StableTemperatureRepository stableTemperatureRepository)
        {
            _sensorRepository = sensorRepository;
            _stableTemperatureRepository = stableTemperatureRepository;
        }

        public int RecordSensors(SensorEntity sensorEntity)
        {
            int id = 0;

            try
            {
                var sensor = sensorEntity.ToSensor();

                id = (int)_sensorRepository.Add(sensor);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return id;
        }

        public MemoryStream GetAllRecords(string dateFrom, string dateTo)
        {
            var sensors = _sensorRepository.GetByDate(dateFrom, dateTo);

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;

                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];

                worksheet.ImportData(sensors, 1, 1, true);

                worksheet.UsedRange.AutofitColumns();

                MemoryStream outputStream = new MemoryStream();

                workbook.SaveAs(outputStream);

                return outputStream;

            }
        }

        public void ResetStableTemperature()
        {
            try
            {
                try
                {
                    var stableTemperature = new LatestTemperature { Temperature = 0.00 };

                    _ = (int)_stableTemperatureRepository.Add(stableTemperature);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
