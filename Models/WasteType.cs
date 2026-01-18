using System.ComponentModel.DataAnnotations;

public class WasteType
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty; // contoh: "Plastic", "Glass"

    public string? Description { get; set; }

    // contoh referensi ke gambar contoh
    public string? ExampleImageUrl { get; set; }

    // opsional: id dari dataset Kaggle jika ada
    public string? KaggleId { get; set; }
}