using System;

namespace ProjekTrashVision.Models
{
    public class DeteksiSampah
    {
        public int Id { get; set; }
        public string NamaFile { get; set; }
        public string JenisSampah { get; set; }
        public DateTime Tanggal { get; set; }
    }
}
