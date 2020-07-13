using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ACIGConsumer.Controllers
{
    public class PolicyController : Controller
    {
        private readonly ILogger<PolicyController> _logger;

        public PolicyController(ILogger<PolicyController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(int? id)
        {
            return PartialView("_policyDetails");
        }

        public IActionResult Details(int id)
        {
            return View();
        }
        
    }
}
