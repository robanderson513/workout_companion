using System;
using System.Collections.Generic;

namespace WorkoutCompanion.Models.Database
{
    public class VisitMetricsItem
    {
        public DateTime Date { get; set; }
        public int SumOfDuration { get; set; }
        public IList<WorkoutModel> Workouts { get; set; }
    }
}
