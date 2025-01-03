using CNPM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CNPM.Utilities;

namespace CNPM.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
            if (!Function.IsLogin())
                return RedirectToAction("Index", "Login");
            return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
            Function._AccountId = 0;
            Function._Username = string.Empty;
            Function._Message = string.Empty;
            return RedirectToAction("Index", "Login");
        }

    }
}