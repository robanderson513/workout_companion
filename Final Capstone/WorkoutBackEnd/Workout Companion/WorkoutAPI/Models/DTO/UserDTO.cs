using System;
using System.ComponentModel.DataAnnotations;

namespace WorkoutAPI.Models.DTO
{
    public class UserDTO
    {
        [Required]
        [StringLength(24)]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public int RoleId { get; set; }

        [StringLength(24)]
        public string FirstName { get; set; }

        [StringLength(24)]
        public string LastName { get; set; }

        [Url]
        public string PhotoURL { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }
        [StringLength(2000)]
        public string WorkoutGoals { get; set; }
        [StringLength(24)]
        public string ExperienceLevel { get; set; }
        [StringLength(24)]
        public string WeeklyExercise { get; set; }
    }
}