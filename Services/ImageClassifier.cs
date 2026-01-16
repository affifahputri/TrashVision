using Microsoft.ML;
using Microsoft.AspNetCore.Hosting;
using System.IO;

public class PredictionResult
{
    public string Label { get; set; }
    public float Score { get; set; }
}

public class ImageClassifier
{
    private readonly MLContext _ml;
    private readonly ITransformer _model;

    public ImageClassifier(IWebHostEnvironment env)
    {
        _ml = new MLContext();

        var modelPath = Path.Combine(env.ContentRootPath, "MLModel", "model.zip");
        if (File.Exists(modelPath))
        {
            using var stream = File.OpenRead(modelPath);
            _model = _ml.Model.Load(stream, out var schema);
        }
        else
        {
            _model = null;
        }
    }

    public PredictionResult Predict(string imagePath)
    {
        if (_model == null)
        {
            return new PredictionResult { Label = "Model not found", Score = 0f };
        }

        // Create a fresh PredictionEngine per call (PredictionEngine is NOT thread-safe).
        var engine = _ml.Model.CreatePredictionEngine<ImageInput, ImagePrediction>(_model);

        var input = new ImageInput { ImagePath = imagePath };
        var pred = engine.Predict(input);

        // If your model provides probabilities/scores, map them here; otherwise return the label.
        return new PredictionResult
        {
            Label = pred.PredictedLabel ?? "Unknown",
            Score = 0f
        };
    }
}