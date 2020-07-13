using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using ACIGConsumer.Models;
using Microsoft.Extensions.Options;

namespace ACIGConsumer.Controllers
{
    [Route("api")]    
    public class ApiController : Controller
    {
        
    }
}
