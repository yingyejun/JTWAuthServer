using JTWAuthServer.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace JTWAuthServer.Controllers {

    [JWTAuth]
    [Route("/api/[controller]")]
    public class TestApiController : Controller {

        [HttpGet]
        public IActionResult Index() {
            return Json(new {
                successed = true
            });
        }
    }
}
