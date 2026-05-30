using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using ApiInteligenteWeb.ML;

namespace ApiInteligenteWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MLController : ControllerBase
{
    private readonly MLContext _mlContext;
    private readonly ITransformer _model;

    public MLController()
    {
        try
        {
            _mlContext = new MLContext();
            _model = SentimentModelBuilder.GetOrCreateModel(_mlContext);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inicializando ML Controller: {ex.Message}");
        }
    }

    // POST: api/ml/sentimiento
    [HttpPost("sentimiento")]
    public IActionResult AnalizarSentimiento([FromBody] SentimentData request)
    {
        try
        {
            // Validación básica
            if (request == null || string.IsNullOrWhiteSpace(request.SentimentText))
            {
                return BadRequest(new { error = "El comentario no puede estar vacío" });
            }

            // Crear engine de predicción
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(_model);
            
            // Realizar predicción
            var prediction = predictionEngine.Predict(request);
            
            // Determinar sentimiento
            var sentimiento = prediction.PredictedLabel ? "Positivo" : "Negativo";
            
            return Ok(new
            {
                comentario = request.SentimentText,
                sentimiento = sentimiento,
                probabilidad = Math.Round(prediction.Probability, 2)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Error al analizar sentimiento: {ex.Message}" });
        }
    }
}