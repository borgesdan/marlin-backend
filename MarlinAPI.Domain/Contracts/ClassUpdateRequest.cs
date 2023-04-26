namespace MarlinAPI.Domain.Contracts
{
    public class ClassUpdateRequest
    {
        public int Number { get; set; }
        public string? Year { get; set; }
        public ClassLevel Level { get; set; }
    }
}
