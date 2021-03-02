using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Words.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppInfoController : ControllerBase
    {
        [HttpGet] 
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(AppInfo.Singleton);
        }
        private readonly ILogger<AppInfoController> _logger;
        public AppInfoController(ILogger<AppInfoController> logger)
        {
            _logger = logger;
        }    
    }
    public class AppInfo
    {
        public string DotNetCoreVersion { get; set; } = System.Environment.Version.ToString();
        public string RuntimeVersion { get; set; } = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;

        public static AppInfo Singleton = new AppInfo();
    }

    
}