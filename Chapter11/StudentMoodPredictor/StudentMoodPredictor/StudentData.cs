using Microsoft.ML.Data;

public class StudentData
{
    [LoadColumn(1)]
    public float LocationId { get; set; }

    [LoadColumn(2)]
    public float TemperatureCelsius { get; set; }

    [LoadColumn(3)]
    public float HumidityPercent { get; set; }

    [LoadColumn(4)]
    public float AirQualityIndex { get; set; }

    [LoadColumn(5)]
    public float NoiseLevelDb { get; set; }

    [LoadColumn(6)]
    public float LightingLux { get; set; }

    [LoadColumn(7)]
    public float CrowdDensity { get; set; }

    [LoadColumn(8)]
    public float StressLevel { get; set; }

    [LoadColumn(9)]
    public float SleepHours { get; set; }

    [LoadColumn(10), ColumnName("Label")]
    public float MoodScore { get; set; }
}