using System;

namespace WorkoutCompanion.Models
{
    public class UserClassModel
    {
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public string UserName { get; set; }
        public string ClassName { get; set; }
        public DateTime ClassDate { get; set; }
    }
}