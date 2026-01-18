using Microsoft.EntityFrameworkCore;
using System.IO;
using ProjekTrashVision.Data; // Pastikan folder 'Data' sudah ada filenya

var builder = WebApplication.CreateBuilder(args);

// 1. Pastikan folder uploads tersedia untuk menampung gambar dari user
var contentRoot = builder.Environment.ContentRootPath;
Directory.CreateDirectory(Path.Combine(contentRoot, "wwwroot", "uploads"));

// 2. Registrasi Services
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews(); // Penting agar Controller/Views dikenali

// 3. Konfigurasi MySQL Laragon (Koreksi di bagian ServerVersion)
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(conn, ServerVersion.AutoDetect(conn)) // Laragon akan dideteksi otomatis
);

var app = builder.Build();

// 4. Konfigurasi Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Mengizinkan akses ke wwwroot (gambar, css, js)
app.UseRouting();
app.UseAuthorization();

// 5. Mapping Routes
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();