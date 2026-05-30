using Microsoft.ML.Data;

namespace ApiInteligenteWeb.ML
{
    // Clase para los datos de entrada (lo que recibe el endpoint)
    public class SentimentData
    {
        public string SentimentText { get; set; } = string.Empty;
    }

    // Clase para los datos de entrenamiento (carga del CSV)
    public class SentimentTrainingData
    {
        [LoadColumn(0)]
        public string SentimentText { get; set; } = string.Empty;

        [LoadColumn(1)]
        public bool Sentiment { get; set; }
    }
}