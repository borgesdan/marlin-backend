using MarlinAPI.Domain.Entities;

namespace MarlinAPI.Domain.Contracts
{
    public class StudentGetResponse
    {
        public string? FullName { get; set; }
        public string? CPF { get; set; }        
        public string? Email { get; set; }
        public string? Registry { get; set; }

        public StudentGetResponse() { }

        public StudentGetResponse(StudentEntity entity)
        {
            FullName = entity.FullName;
            CPF = entity.CPF;
            Email = entity.Email;
            Registry = entity.Registry;
        }
    }
}
