using Microsoft.AspNetCore.Mvc;

namespace JTWAuthServer.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
        }

        public IActionResult Test() {
            return View();
        }

    }
}
