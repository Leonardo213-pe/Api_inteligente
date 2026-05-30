using Microsoft.ML.Data;

namespace ApiInteligenteWeb.ML
{
    // Clase para la predicción (lo que devuelve el modelo)
    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }

        public float Probability { get; set; }
    }
}