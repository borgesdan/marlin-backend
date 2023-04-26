using MarlinAPI.Domain.Entities;

namespace MarlinAPI.Domain.Contracts
{
    public class ClassGetResponse
    {
        public int Number { get; set; }
        public string? Year { get; set; }
        public string? Level { get; set; }
        public string? Registry { get; set; }
        public List<StudentItemList>? Students { get; set; }

        public ClassGetResponse() { }

        public ClassGetResponse(ClassEntity entity) 
        {
            Number = entity.Number;
            Year = entity.Year;            
            Registry = entity.Registry;
            Students = entity.Students.Select(s => new StudentItemList(s)).ToList();

            switch(entity.Level)
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

        public class StudentItemList
        {
            public string? FullName { get; set; }
            public string? Registry { get; set; }

            public StudentItemList(StudentEntity entity)
            {
                FullName = entity.FullName;
                Registry = entity.Registry;
            }
        }
    }
}
