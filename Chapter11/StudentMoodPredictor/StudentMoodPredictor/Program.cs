// See https://aka.ms/new-console-template for more information

using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using Spectre.Console;

IAnsiConsole console = AnsiConsole.Console;

// Display a header
console.MarkupLine("[bold blue]Student Mood Predictor[/]");
console.MarkupLine("[bold green]Data Preparation[/]");

MLContext context = new();

// Load data and split into training and test sets
IDataView data = context.Data.LoadFromTextFile<StudentData>("DataSet.csv", separatorChar: ',', hasHeader: true);
DataOperationsCatalog.TrainTestData trainTest = context.Data.TrainTestSplit(data, testFraction: 0.2f);

// Train a regression model
uint maxSeconds = 20;
RegressionExperiment experiment = context.Auto().CreateRegressionExperiment(maxSeconds);

ExperimentResult<RegressionMetrics>? result = null;

console.Status()
    .Start($"Training regression model for {maxSeconds} seconds...", ctx =>
    {
        result = experiment.Execute(trainTest.TrainSet, trainTest.TestSet);
    });

// Display metrics on our best model
console.MarkupLineInterpolated($"Best model found: [bold yellow]{result.BestRun.TrainerName}[/]");

RegressionMetrics metrics = result.BestRun.ValidationMetrics;
Table metricsTable = new Table()
    .AddColumns("Metric","Value")
    .Expand();
metricsTable.AddRow("R-Squared (R2)", metrics.RSquared.ToString("P2"));
metricsTable.AddRow("Mean Absolute Error (MAE)", metrics.MeanAbsoluteError.ToString("F2"));
metricsTable.AddRow("Mean Squared Error (MSE)", metrics.MeanSquaredError.ToString("F2"));
metricsTable.AddRow("Root Mean Squared Error (RMSE)", metrics.RootMeanSquaredError.ToString("F2"));
console.Write(metricsTable);

// Build the final model using the best pipeline
ITransformer finalModel = result.BestRun.Model;
// Save the model to a file
context.Model.Save(finalModel, data.Schema, "Model.zip");

// Load the model from the file (not really necessary here, but good for completeness)
finalModel = context.Model.Load("Model.zip", out _);

// Build prediction engine
PredictionEngine<StudentData, StudentPrediction> predictionEngine = 
    context.Model.CreatePredictionEngine<StudentData, StudentPrediction>(finalModel);
// Example prediction
StudentData sampleData = new()
{
    LocationId = 1,
    TemperatureCelsius = 22.5f,
    HumidityPercent = 45.0f,
    AirQualityIndex = 50.0f,
    NoiseLevelDb = 30.0f,
    LightingLux = 300.0f,
    CrowdDensity = 0.2f,
    StressLevel = 3.0f,
    SleepHours = 7.0f
};
StudentPrediction prediction = predictionEngine.Predict(sampleData);
console.MarkupLineInterpolated($"Predicted Mood Score: [bold cyan]{prediction.PredictedValue}[/]");