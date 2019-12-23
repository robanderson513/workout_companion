using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutCompanion.Models.Database;

namespace WorkoutAPI.Models
{
    public class MetricsViewModel
    {
        public decimal AvgDuration { get; set; }
        public IList<TopUsageItemModel> TopFiveWorkouts { get; set; }
        public IList<VisitMetricsItem> WeeklyWorkouts { get; set; } = new List<VisitMetricsItem>();
    }
}
