using System.Threading.Tasks;
using JTWAuthServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace JTWAuthServer.Controllers {
    public class ClientController : Controller {
        private readonly IJWTClientService _clientService;

        public ClientController(IJWTClientService clientService) {
            _clientService = clientService;
        }
        public IActionResult Index() {
            return View();
        }

        public IActionResult Edit() {
            return View();
        }


        public IActionResult Test() {
            return View();
        }

        [HttpGet("/client/paged")]
        public async Task<IActionResult> Paged(int pageSize, int pageIndex) {
            var paged = await _clientService.GetPagedClientAsync(pageSize, pageIndex);
            return Json(new {
                total = paged.TotalCount,
                data = paged
            });
        }
    }
}
