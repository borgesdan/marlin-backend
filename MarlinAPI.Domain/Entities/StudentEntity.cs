using System.ComponentModel.DataAnnotations;

namespace MarlinAPI.Domain.Entities
{
    public class StudentEntity : BaseEntity
    {   
        [Required]
        [StringLength(256)]
        public string? FullName { get; set; }
        
        [Required]
        [StringLength(14)]
        public string? CPF { get; set; }

        [Required]
        [StringLength(256)]
        public string? Email { get; set; }

        [Required]
        [StringLength(32)]
        public string? Registry { get; set; }        
    }
}
