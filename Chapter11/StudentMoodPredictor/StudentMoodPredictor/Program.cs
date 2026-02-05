using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using Spectre.Console;

// Display a header
IAnsiConsole console = AnsiConsole.Console;
console.MarkupLine("[bold blue]Student Mood Predictor[/]");
console.MarkupLine("[bold green]Data Preparation[/]");

// Load data and split into training and test sets
MLContext context = new();
IDataView data = context.Data.LoadFromTextFile<StudentData>("DataSet.csv", separatorChar: ',', hasHeader: true);
DataOperationsCatalog.TrainTestData trainTest = context.Data.TrainTestSplit(data, testFraction: 0.2f);

// Train a regression model
RegressionExperimentSettings settings = new()
{
    MaxModels = 5,
    MaxExperimentTimeInSeconds = 15,
    OptimizingMetric = RegressionMetric.RSquared
};
RegressionExperiment experiment = context.Auto().CreateRegressionExperiment(settings);

ExperimentResult<RegressionMetrics>? result = null;
console.Status().Start($"Training regression model for {settings.MaxExperimentTimeInSeconds} seconds...", _ =>
    {
        result = experiment.Execute(trainTest.TrainSet, trainTest.TestSet);
    });

// Display metrics for each model run
Table runsTable = new Table()
    .AddColumns("Trainer", "R-Squared (R2)", "MAE", "MSE", "RMSE")
    .Expand();
foreach (var run in result!.RunDetails)
{
    // Runs may not have validation metrics if they failed or were skipped
    if (run.ValidationMetrics is null) continue;
    
    runsTable.AddRow(run.TrainerName,
        run.ValidationMetrics.RSquared.ToString("P2"),
        run.ValidationMetrics.MeanAbsoluteError.ToString("F2"),
        run.ValidationMetrics.MeanSquaredError.ToString("F2"),
        run.ValidationMetrics.RootMeanSquaredError.ToString("F2"));
}
console.Write(runsTable);
console.MarkupLineInterpolated($"[bold green]Best Model:[/] {result.BestRun.TrainerName}");

// Display detailed metrics on the best model
RegressionMetrics metrics = result.BestRun.ValidationMetrics;
Table table = new Table()
    .AddColumns("Metric","Value")
    .Expand()
    .AddRow("R-Squared (R2)", metrics.RSquared.ToString("P2"))
    .AddRow("Mean Absolute Error (MAE)", metrics.MeanAbsoluteError.ToString("F2"))
    .AddRow("Mean Squared Error (MSE)", metrics.MeanSquaredError.ToString("F2"))
    .AddRow("Root Mean Squared Error (RMSE)", metrics.RootMeanSquaredError.ToString("F2"));
console.Write(table);

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
float mood = prediction.PredictedValue;
console.MarkupLineInterpolated($"Predicted Mood Score: [bold cyan]{mood}[/]");

BarChart bar = new BarChart()
    .Label("Mood Prediction by Lighting Lux")
    .Width(60)
    .CenterLabel();

float[] luxValues = [100, 200, 300, 400, 500];
foreach (float lux in luxValues)
{
    prediction = predictionEngine.Predict(new StudentData { LightingLux = lux });
    mood = prediction.PredictedValue;
    bar.AddItem($"{lux} Lux", Math.Round(mood, 2), Color.Yellow);
}
console.Write(bar);