using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oyshroom.Data;
using Oyshroom.Data.Model;
using Oyshroom.Domain;
using Oyshroom.Domain.Entity;
using Oyshroom.Fuzzy.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oyshroom.Fuzzy.API
{
    public class Startup
    {
        private readonly LoggerService _loggerService;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var loggingFilePath = Configuration["LoggingFilePath"];
            _loggerService = new LoggerService(loggingFilePath);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           

            try
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Fuzzy API", Version = "v1" });
                });

                var database = new Database(Configuration["DatabasePath"]);

                var stableTemperatureRepository = new StableTemperatureRepository(database);
                var sensorRepository = new SensorRepository(database);
                var fuzzyLogicRepository = new FuzzyLogicRepository(database);
                var sensorService = new SensorService(sensorRepository, stableTemperatureRepository);
            
                var fuzzyLogicService = new FuzzyLogicService(fuzzyLogicRepository, sensorRepository);
                var actuatorEvaluator = new ActuatorEvaluator();
                var rpiService = new RpiService(actuatorEvaluator);

                _loggerService.Information($"Database Location: {database}");

                _loggerService.Information("Initializing Dependency Injection");

                services.AddSingleton<Database>(database);
                services.AddSingleton<SensorRepository>(sensorRepository);
                services.AddSingleton<StableTemperatureRepository>(stableTemperatureRepository);
                services.AddSingleton<LoggerService>(_loggerService);

                services.AddSingleton<FuzzyLogicRepository>(fuzzyLogicRepository);
                services.AddScoped<FuzzyLogicService>();
                services.AddScoped<SensorService>();
                services.AddSingleton<RpiService>(rpiService);


                _loggerService.Information("Dependency Injection Completed");

                services.AddControllers();
            }
            catch (Exception ex)
            {
                _loggerService.Error($"Error Encountered on ConfigureServices method - Exception: {ex.Message} - {ex.InnerException.Message}");
                throw ex;
            }

          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fuzzy API");
                });

                app.UseRouting();

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            }
            catch (Exception ex)
            {
                _loggerService.Error($"Error Encountered on ConfigureServices method - Exception: {ex.Message} - {ex.InnerException.Message}");
                throw ex;
            }

          
        }
    }
}
