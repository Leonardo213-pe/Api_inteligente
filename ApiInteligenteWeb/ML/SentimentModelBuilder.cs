using Microsoft.ML;
using ApiInteligenteWeb.ML;

namespace ApiInteligenteWeb.ML
{
    public static class SentimentModelBuilder
    {
        private static ITransformer? _model;
        private static MLContext? _mlContext;

        public static ITransformer GetOrCreateModel(MLContext mlContext)
        {
            if (_model == null)
            {
                try
                {
                    var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "sentiment_data.csv");
                    
                    // Verificar si el archivo existe
                    if (!File.Exists(dataPath))
                    {
                        throw new FileNotFoundException($"No se encontró el archivo: {dataPath}");
                    }
                    
                    // Cargar datos
                    var data = mlContext.Data.LoadFromTextFile<SentimentTrainingData>(
                        path: dataPath,
                        separatorChar: ',',
                        hasHeader: true);

                    // Crear pipeline de entrenamiento
                    var pipeline = mlContext.Transforms.Text.FeaturizeText(
                            outputColumnName: "Features",
                            inputColumnName: nameof(SentimentTrainingData.SentimentText))
                        .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                            labelColumnName: nameof(SentimentTrainingData.Sentiment),
                            featureColumnName: "Features"));

                    // Entrenar el modelo
                    _model = pipeline.Fit(data);
                    _mlContext = mlContext;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al crear el modelo ML: {ex.Message}");
                }
            }
            
            return _model;
        }
    }
}