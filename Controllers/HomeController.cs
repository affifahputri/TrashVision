using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProjekTrashVision.Models;
using System;
using System.IO;

namespace ProjekTrashVision.Controllers
{
    public class HomeController : Controller
    {
        // ================= HOME =================
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // ================= UPLOAD PAGE =================
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        // ================= UPLOAD POST =================
        [HttpPost]
        public IActionResult Upload(IFormFile gambar, string gambarBase64)
        {
            string fileName = "";

            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads"
            );

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // ===== DARI KAMERA =====
            if (!string.IsNullOrEmpty(gambarBase64))
            {
                var base64Data = gambarBase64.Split(',')[1];
                var bytes = Convert.FromBase64String(base64Data);

                fileName = Guid.NewGuid().ToString() + ".png";
                var filePath = Path.Combine(folderPath, fileName);

                System.IO.File.WriteAllBytes(filePath, bytes);
            }
            // ===== DARI FILE =====
            else if (gambar != null && gambar.Length > 0)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(gambar.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    gambar.CopyTo(stream);
                }
            }
            else
            {
                return RedirectToAction("Upload");
            }

            // ===== SIMULASI DETEKSI =====
            string jenisSampah;

            if (fileName.ToLower().Contains("daun") ||
                fileName.ToLower().Contains("makanan"))
            {
                jenisSampah = "Organik";
            }
            else
            {
                jenisSampah = "Anorganik";
            }

            // ===== ISI MODEL =====
            var hasil = new DeteksiSampah
            {
                NamaFile = fileName,
                JenisSampah = jenisSampah,
                Tanggal = DateTime.Now
            };

            return View("Hasil", hasil);
        }
    }
}
