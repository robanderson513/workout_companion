using System;
using System.ComponentModel.DataAnnotations;

namespace WorkoutCompanion.Models.Database
{
    public class VisitItem
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime CheckIn { get; set; } = DateTime.UtcNow;
        public DateTime? CheckOut { get; set; } = null;

    }
}
