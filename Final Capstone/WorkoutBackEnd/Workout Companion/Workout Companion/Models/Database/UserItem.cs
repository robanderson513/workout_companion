using System;
using WorkoutCompanion.Utility;
using System.ComponentModel.DataAnnotations;

namespace WorkoutCompanion.Models.Database
{
    [Serializable]
    public class UserItem
    {
        //  [JsonIgnore]
        private const string defaultPhotoURL = "https://www.sackettwaconia.com/wp-content/uploads/default-profile.png";
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public int RoleId { get; set; } = 1;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Url]
        public string PhotoURL { get; set; } = defaultPhotoURL;
        public string WorkoutGoals { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public string ExperienceLevel { get; set; } = "na";
        public string WeeklyExercise { get; set; } = "na";

        public UserItem Clone()
        {
            return ObjectCopier.Clone(this);
        }
    }
}
