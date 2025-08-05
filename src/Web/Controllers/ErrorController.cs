using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;
public class ErrorController : Controller
{
    [Route("error/{statusCode?}")]
    public IActionResult Index(int? statusCode = null) {
        ViewData["StatusCode"] = statusCode;
        return View("Error");
    }
}