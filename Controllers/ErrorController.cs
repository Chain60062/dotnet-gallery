using Microsoft.AspNetCore.Mvc;

namespace Gallery.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        [Route("404")]
        public async Task<IActionResult> NotFoundPage()
        {
            return View();
        }
    }
}