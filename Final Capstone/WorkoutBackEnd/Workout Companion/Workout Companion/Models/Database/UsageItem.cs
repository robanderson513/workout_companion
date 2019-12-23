using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutCompanion.Models.Database
{
    public class UsageItem
    {
        public int VisitId { get; set; }
        public int EquipmentId { get; set; }
        public int? Reps { get; set; }
        public int? Weight { get; set; }
        public int? Duration { get; set; }
    }
}
