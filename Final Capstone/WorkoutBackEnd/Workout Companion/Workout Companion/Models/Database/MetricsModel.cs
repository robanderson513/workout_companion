namespace WorkoutCompanion.Models.Database
{
    public class MetricsModel
    {
        public int AvgVisitDuration { get; set; }
        public TopUsageItemModel[] TopFiveWorkouts { get; set; }
        public VisitMetricsItem[] WeeklyWorkouts { get; set; }
    }
}
