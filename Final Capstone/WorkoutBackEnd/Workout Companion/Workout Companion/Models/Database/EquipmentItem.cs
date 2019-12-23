using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WorkoutCompanion.Models.Database
{
    public class EquipmentItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public string Description { get; set; }
        [Url]
        public string ImageURL { get; set; }
        [Url]
        public string VideoLink { get; set; }
        public int MachineNumber { get; set; }
    }
}
