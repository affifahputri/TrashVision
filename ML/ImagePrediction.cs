using Microsoft.ML.Data;

public class ImagePrediction
{
    [ColumnName("PredictedLabel")]
    public string PredictedLabel { get; set; }
}
