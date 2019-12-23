using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutAPI.Models.DTO
{
    public class HistoryDTO
    {
        public int VisitId { get; set; }
        public int EquipmentId { get; set; }
        public int? Reps { get; set; }
        public int? Weight { get; set; }
        public int? Duration { get; set; }
    }
}
