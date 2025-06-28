using Microsoft.ML.Data;

public class StudentPrediction
{
    [ColumnName("Score")]
    public float PredictedValue { get; set; }
}