namespace MarlinAPI.Domain.Contracts
{
    public class StudentUpdateRequest
    {
        public string? FullName { get; set; }
        public string? CPF { get; set; }
        public string? Email { get; set; }
    }
}
