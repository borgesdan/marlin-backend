using System.ComponentModel.DataAnnotations;

namespace MarlinAPI.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
