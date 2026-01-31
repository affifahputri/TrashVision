namespace ProjekTrashVision.Models
{
    public class UploadViewModel
    {
        public string? HasilPrediksi { get; set; }
        public float SkorKepastian { get; set; }
        public string? GambarBase64 { get; set; } // Untuk menampilkan kembali gambar yang diupload
    }
}