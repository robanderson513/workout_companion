using System.ComponentModel.DataAnnotations;

namespace WorkoutAPI.Models
{
    public class RegisterAccountViewModel
    {
        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// The user's email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The user's username.
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]

        public string Password { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        public string ExperienceLevel { get; set; } = "na'";

        /// <summary>
        /// The user's weekly exercise frequency.
        /// </summary>
        [Required]
        public string WeeklyExercise { get; set; } = "na";

        /// <summary>
        /// The user's goal.
        /// </summary>
        [Required]
        public string Goals { get; set; }








    }
}
