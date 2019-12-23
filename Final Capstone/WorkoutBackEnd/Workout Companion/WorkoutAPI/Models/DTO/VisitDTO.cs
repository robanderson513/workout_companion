using System;

namespace WorkoutAPI.Models.DTO
{
    public class VisitDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; } = null;
    }
}
