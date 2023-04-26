using MarlinAPI.Domain.Entities;

namespace MarlinAPI.Domain.Contracts
{
    public class StudentCreateRequest
    {
        public string? FullName { get; set; }
        public string? CPF { get; set; }
        public string? Email { get; set; }
        public List<ClassStudentContract>? Classes { get; set; }
    }
}
