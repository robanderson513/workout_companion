using System;

namespace WorkoutAPI.Models.DTO
{
    public class UserClassDTO
    {
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public string UserName { get; set; }
        public string ClassName { get; set; }
        public DateTime ClassDate { get; set; }
    }
}
