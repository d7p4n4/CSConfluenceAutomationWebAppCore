using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSConfluenceServiceFW;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CSConfluenceAutomationWebAppCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfluenceController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ConfluenceController> _logger;

        public ConfluenceController(ILogger<ConfluenceController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
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

        [HttpPost]
        public string Post(UpdateOrAddPageCompositeRequest value)
        {
            UpdateOrAddPageCompositeResponse updateOrAddPageCompositeResponse =
                new ConfluenceServices().UpdateOrAddPageComposite(new CSConfluenceServiceFW.UpdateOrAddPageCompositeRequest()
                {
                    PageTitle = value.PageTitle
                    ,
                    Password = value.Password
                    ,
                    Content = value.Content
                    ,
                    URL = value.URL
                    ,
                    Username = value.Username
                    ,
                    ParentPageTitle = value.ParentPageTitle
                    ,
                    SpaceKey = value.SpaceKey
                });

            if (updateOrAddPageCompositeResponse.Result.Success())
            {
                return "A feltöltés sikeres!";
            }
            else
            {
                return updateOrAddPageCompositeResponse.Result.Message;
            }
        }
    }
}
