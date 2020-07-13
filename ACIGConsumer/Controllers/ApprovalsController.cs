using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ACIGConsumer.Controllers
{
    public class ApprovalsController : Controller
    {
        private readonly ILogger<ApprovalsController> _logger;

        public ApprovalsController(ILogger<ApprovalsController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult test()
        {
            return View();
        }
    }
}
