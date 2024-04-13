using Microsoft.AspNetCore.Mvc;

namespace Gym_Project.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFound()
        {
            HttpContext.Response.StatusCode = 404;
            return View();
        }

        public IActionResult InternalError()
        {
            HttpContext.Response.StatusCode = 500;
            return View();
        }
    }
}
