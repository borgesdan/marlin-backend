using MarlinAPI.Domain.Entities;
using static MarlinAPI.Domain.Contracts.ClassGetResponse;

namespace MarlinAPI.Domain.Contracts
{
    public class ClassGetAllResponse
    {
        public int Number { get; set; }
        public string? Year { get; set; }
        public string? Level { get; set; }
        public string? Registry { get; set; }

        public ClassGetAllResponse() { }

        public ClassGetAllResponse(ClassEntity entity)
        {
            Number = entity.Number;
            Year = entity.Year;
            Registry = entity.Registry;

            switch (entity.Level)
            {
                case ClassLevel.Begginer:
                    Level = "Iniciante";
                    break;
                case ClassLevel.Intermediate:
                    Level = "Intermediário";
                    break;
                case ClassLevel.Expert:
                    Level = "Avançado";
                    break;
            }
        }
    }
}
