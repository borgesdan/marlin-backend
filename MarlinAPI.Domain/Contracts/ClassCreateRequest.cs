namespace MarlinAPI.Domain.Contracts
{
    public class ClassCreateRequest
    {
        public int Number { get; set; }
        public string? Year { get; set; }
        public ClassLevel Level { get; set; }
    }
}
