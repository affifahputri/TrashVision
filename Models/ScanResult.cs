using System;
using System.ComponentModel.DataAnnotations;

public class ScanResult
{
    public int Id { get; set; }

    [Required]
    public string FileName { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public string? PredictedLabel { get; set; }

    public float? Score { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // jika mau relasi ke WasteType:
    public int? WasteTypeId { get; set; }
    public WasteType? WasteType { get; set; }

    // sumber: "camera" atau "upload"
    public string Source { get; set; } = "upload";
}