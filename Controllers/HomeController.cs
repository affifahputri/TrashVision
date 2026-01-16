using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly ImageClassifier _classifier;

    public HomeController(IWebHostEnvironment env, ImageClassifier classifier)
    {
        _env = env;
        _classifier = classifier;
    }

    public IActionResult Index()
    {
        return View();
    }

    // GET handler supaya /Home/Upload (GET) tidak menghasilkan 405
    [HttpGet]
    public IActionResult Upload()
    {
        return View("UploadGambar");
    }

    public IActionResult UploadGambar()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(IFormFile gambar)
    {
        // existing non-AJAX form submit (file input)
        if (gambar == null || gambar.Length == 0)
        {
            ModelState.AddModelError("", "Pilih file gambar.");
            return View("UploadGambar");
        }

        var uploads = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploads);

        var fileName = Path.GetRandomFileName() + Path.GetExtension(gambar.FileName);
        var filePath = Path.Combine(uploads, fileName);

        using (var stream = System.IO.File.Create(filePath))
        {
            await gambar.CopyToAsync(stream);
        }

        var prediction = _classifier != null ? _classifier.Predict(filePath) : null;

        ViewBag.Result = prediction?.Label ?? "Model not loaded";
        ViewBag.Score = prediction?.Score;
        ViewBag.ImageUrl = "/uploads/" + fileName;

        return View("UploadGambar");
    }

    // Untuk AJAX dari kamera: abaikan antiforgery token agar fetch() sederhana berhasil.
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> UploadAjax(IFormFile gambar)
    {
        if (gambar == null || gambar.Length == 0)
            return BadRequest("Tidak ada file gambar.");

        var uploads = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploads);

        var fileName = Path.GetRandomFileName() + Path.GetExtension(gambar.FileName);
        var filePath = Path.Combine(uploads, fileName);

        using (var stream = System.IO.File.Create(filePath))
        {
            await gambar.CopyToAsync(stream);
        }

        var prediction = _classifier != null ? _classifier.Predict(filePath) : null;

        var result = new
        {
            label = prediction?.Label ?? "Model not loaded",
            score = prediction?.Score,
            imageUrl = "/uploads/" + fileName
        };

        return Json(result);
    }
}