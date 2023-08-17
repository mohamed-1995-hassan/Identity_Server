using JavaScriptClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JavaScriptClient.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}