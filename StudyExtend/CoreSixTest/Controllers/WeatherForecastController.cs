using CoreSixTest.DTOs;
using CoreSixTest.Entities;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CoreSixTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : BaseController
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("Mapster")]
        public string Mapster([FromQuery] EmpDTO empDTO)
        {
    //        TypeAdapterConfig<EmpDTO, Emp>
    //.NewConfig()
    ////.Ignore(dest => dest.Name)
    //.Map(dest => dest.Id,
    //    src => string.Format("{0} {1}", src.Id, src.Desc));
    //        TypeAdapterConfig.GlobalSettings.When((EmpDTO, Emp, MapType) => EmpDTO != Emp).Ignore("Name");
            var emp = empDTO.Adapt<Emp>();
            
            EmpValidator validationRules= new EmpValidator();
            var validate = validationRules.Validate(emp);
            var errors = string.Join(Environment.NewLine,validate.Errors.Select(t=>t.ErrorMessage).ToArray());
            var res = 123.Adapt<string>();
            //return JsonConvert.SerializeObject(emp);
            return errors;
        }
    }
}