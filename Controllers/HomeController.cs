using Microsoft.AspNetCore.Mvc;
using ProjekTrashVision.Data;
using ProjekTrashVision.Models;
using System.Linq;

namespace ProjekTrashVision.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // PERBAIKAN 1: Kirim objek kosong ke View saat pertama kali buka
        public IActionResult Index()
        {
            return View(new UploadViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile fotoSampah) // Sesuaikan nama 'fotoSampah' dengan di HTML
        {
            var viewModel = new UploadViewModel();

            if (fotoSampah == null || fotoSampah.Length == 0)
            {
                ViewBag.Error = "Silakan pilih foto terlebih dahulu.";
                return View("Index", viewModel);
            }

            // 1. Baca gambar ke dalam Byte Array
            using var ms = new MemoryStream();
            await fotoSampah.CopyToAsync(ms);
            byte[] imageBytes = ms.ToArray();

            // 2. Jalankan Prediksi AI
            var input = new ModelSampah.ModelInput()
            {
                ImageSource = imageBytes
            };

            var result = ModelSampah.Predict(input);

            // 3. Ambil Label dan Skor
            string labelHasil = result.PredictedLabel;
            float skor = result.Score.Max() * 100;

            // 4. Masukkan data ke ViewModel untuk ditampilkan di Web
            viewModel.HasilPrediksi = labelHasil;
            viewModel.SkorKepastian = skor;
            viewModel.GambarBase64 = Convert.ToBase64String(imageBytes);

            // 5. SIMPAN KE DATABASE (LARAGON)
            var dataBaru = new RiwayatDeteksi
            {
                NamaFile = fotoSampah.FileName,
                LabelHasil = labelHasil,
                SkorKeyakinan = (float)Math.Round(skor, 2),
                WaktuUpload = DateTime.Now
            };

            _context.RiwayatDeteksis.Add(dataBaru);
            await _context.SaveChangesAsync();

            // PERBAIKAN 2: Kirim viewModel kembali ke View
            return View("Index", viewModel);
        }
    }
}