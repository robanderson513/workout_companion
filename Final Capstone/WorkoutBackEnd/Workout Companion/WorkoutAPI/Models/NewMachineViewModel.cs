using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutAPI
{
    public class NewMachineViewModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public string Category { get; set; }
        
        [Required]
        public string ImageURL { get; set; }

        [Required]
        public string VideoURL { get; set; }

        [Required]
        public int Number { get; set; }
    }
}
