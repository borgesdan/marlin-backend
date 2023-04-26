using System.ComponentModel.DataAnnotations;

namespace MarlinAPI.Domain.Entities
{
    public class ClassEntity : BaseEntity
    {        
        [Required]
        public int Number { get; set; }
        
        [Required]
        [StringLength(6)]
        public string? Year { get; set; }

        [Required]
        [StringLength(32)]
        public string? Registry { get; set; }

        public ClassLevel Level { get; set; }

        public virtual ICollection<StudentEntity> Students { get; set; } = new HashSet<StudentEntity>();
    }
}