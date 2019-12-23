using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutAPI.Models.DTO
{
    public class EquipmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public string VideoLink { get; set; }
        public int MachineNumber { get; set; }
    }
}
