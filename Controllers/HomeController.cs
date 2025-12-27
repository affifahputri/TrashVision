using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(IFormFile imageFile)
    {
        if (imageFile != null)
        {
            // sementara dummy dulu
            ViewBag.Result = "Organik";
        }

        return View();
    }
}
