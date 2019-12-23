using WorkoutCompanion.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace WorkoutCompanion.Models.Database
{
    [Serializable]
    public class RoleItem
    {
        [Range(1, 3)]
        public int Id { get; set; }
        public string Name { get; set; }

        public RoleItem Clone()
        {
            return ObjectCopier.Clone(this);
        }
    }
}
