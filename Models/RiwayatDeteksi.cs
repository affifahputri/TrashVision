public class RiwayatDeteksi
{
    public int Id { get; set; }
    public string? NamaFile { get; set; }
    public string? LabelHasil { get; set; }
    public float SkorKeyakinan { get; set; }

    // Ini yang akan otomatis mengisi tanggal di sisi C#
    public DateTime WaktuUpload { get; set; } = DateTime.Now;
}