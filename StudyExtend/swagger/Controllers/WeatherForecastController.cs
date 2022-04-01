using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPC;
using swagger.Interfaces;
using swagger.Services;
using CPC.WebCore;
using swaggerOne.Interfaces;
using swagger.DTOs;
using swagger.Entities;
using swaggerOne.DTOs;
using swaggerOne.Entities;
using CPC.Logging;

namespace swagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : CommonApiController
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

        [HttpGet("Get")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("TestDepends")]
        public string TestDepends()
        {
            var empService = GetService<IEmploy>();
            empService.Name("wangfeng");
            return "ok";
        }
        [HttpGet("TestCompany")]
        public string TestCompany(string name)
        {
            var companyService = GetService<ICompany>();
            companyService.CompanyName(name);
            return "ok";
        }
        [HttpGet("TestSchool")]
        public string TestSchool(string name)
        {
            var schoolService = GetService<ISchool>();
            return name;
        }
        [HttpGet("TestMapper")]
        public string TestMapper([FromQuery]EmployDTO employDTO)
        {
            var employ = employDTO.Map<Employ>();
            var companyDto2 = employ.Map<CompanyDTO>();
            var employDto = employ.Map<EmployDTO>();
            var company = employDTO.Map<swagger.Entities.Company>();
            var company1 = employDTO.Map<swagger.Company>();
            return company.Name;
        }
        [HttpGet("TestSchoolMapper")]
        public string TestSchoolMapper([FromQuery]SchoolDTO schoolDTO)
        {
            var school = schoolDTO.Map<School>();
            //LogUtility.Info("TestSchoolMapper");
            //throw new ArgumentNullException();
            return school.Name;
        }
    }
}
