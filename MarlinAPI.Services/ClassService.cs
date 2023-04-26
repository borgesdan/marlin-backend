using MarlinAPI.Domain.Contracts;
using MarlinAPI.Domain.Entities;
using MarlinAPI.Repository;
using System.Net;

namespace MarlinAPI.Service
{
    public class ClassService
    {
        private readonly ClassRepository classRepository;
        private readonly StudentRepository studentRepository;

        public ClassService(ClassRepository classRepository, StudentRepository studentRepository)
        {
            this.classRepository = classRepository;
            this.studentRepository = studentRepository;
        }

        /// <summary>
        /// Cadastra uma nova turma.
        /// </summary>
        public async Task<IServiceResultData> CreateAsync(ClassCreateRequest request)
        {
            var validationResult = RequestValidation(request);

            if (validationResult != null)
                return validationResult;

            var entity = new ClassEntity
            {
                Number = request.Number,
                Year = request.Year,
                Registry = Helpers.CreateClassRegistry(),
                Level = request.Level,
            };

            try
            {
                var currentClass = await classRepository.GetAsync(c => c.Year == entity.Year && c.Number == entity.Number && c.Level == entity.Level);

                if(currentClass != null)
                    return ServiceResultData.Error($"Já existe uma turma cadastrada com o número {entity.Number} e ano {entity.Year}.");

                await classRepository.CreateAsync(entity);
                return new ServiceResultData(true, $"A turma foi criada com sucesso. Registro {entity.Registry}", HttpStatusCode.OK);
            }
            catch
            {
                return ServiceResultData.Error("Ocorreu um erro interno ao criar a turma no banco de dados.");
            }
        }

        /// <summary>
        /// Atualiza os dados de uma turma.
        /// </summary>
        public async Task<IServiceResultData> UpdateAsync(string? registry, ClassUpdateRequest request)
        {
            var validationResult = RequestValidation(registry, request);

            if (validationResult != null)
                return validationResult;

            registry = registry.Trim().ToUpper();

            try
            {
                var currentClass = await classRepository.GetAsync(c => c.Registry == registry);

                if (currentClass == null)
                    return ServiceResultData.Error($"Não foi encontrada uma turma com esse registro.");

                currentClass.Number = request.Number;
                currentClass.Year = request.Year;
                currentClass.Level = request.Level;

                await classRepository.UpdateAsync(currentClass);
                return new ServiceResultData(true, $"A turma foi atualizada com sucesso.", HttpStatusCode.OK);
            }
            catch
            {
                return ServiceResultData.Error("Ocorreu um erro interno ao atualizar a turma no banco de dados.");
            }
        }

        /// <summary>
        /// Deleta uma turma somente se ela não tiver alunos matrículados.
        /// </summary>
        public async Task<IServiceResultData> DeleteAsync(string? registry)
        {
            if (string.IsNullOrWhiteSpace(registry))
                return ServiceResultData.Error("O registro da turma não pode ser nulo.");

            registry = registry.Trim().ToUpper();

            try
            {
                var currentClass = await classRepository.GetAsync(c => c.Registry == registry, true);

                if (currentClass == null)
                    return ServiceResultData.Error($"Não foi encontrada uma turma com esse registro.");                

                if(currentClass.Students.ToList().Count > 0)
                    return ServiceResultData.Error($"A turma não pode ser excluída pois ainda contém alunos.");

                await classRepository.DeleteAsync(currentClass);

                return new ServiceResultData(true, $"A turma foi exclu[ida com sucesso.", HttpStatusCode.OK);
            }
            catch
            {
                return ServiceResultData.Error("Ocorreu um erro interno ao deletar a turma no banco de dados.");
            }
        }

        /// <summary>
        /// Obtém os dados de uma turma.
        /// </summary>
        public async Task<IServiceResultData> GetAsync(string? registry)
        {
            if (string.IsNullOrWhiteSpace(registry))
                return ServiceResultData.Error("O registro da turma não pode ser nula.");

            registry = registry.Trim().ToUpper();

            try
            {
                var currentClass = await classRepository.GetAsync(c => c.Registry == registry, true);

                if (currentClass == null)
                    return ServiceResultData.Error($"Não foi encontrada uma turma com esse registro.");

                return ServiceResultData.Ok(new ClassGetResponse(currentClass));
            }
            catch
            {
                return ServiceResultData.Error("Ocorreu um erro interno ao obter a turma no banco de dados.");
            }
        }

        /// <summary>
        /// Obtém todas as turmas ativas.
        /// </summary>
        public async Task<IServiceResultData> GetAllAsync()
        {
            try
            {
                var classes = await classRepository.GetAllAsync();

                return ServiceResultData.Ok(classes.Select(c => new ClassGetAllResponse(c)));
            }
            catch
            {
                return ServiceResultData.Error("Ocorreu um erro interno ao obter a turma no banco de dados.");
            }
        }

        /// <summary>
        /// Remove todos os estudantes matrículados da turma.
        /// </summary>
        public async Task<IServiceResultData> RemoveAllStudents(string? registry) 
        {
            if (string.IsNullOrWhiteSpace(registry))
                return ServiceResultData.Error("O registro da turma não pode ser nula.");

            registry = registry.Trim().ToUpper();

            try
            {
                var currentClass = await classRepository.GetAsync(c => c.Registry == registry, true);

                if (currentClass == null)
                    return ServiceResultData.Error($"Não foi encontrada uma turma com esse registro.");

                if (currentClass.Students.Count == 0)
                    return new ServiceResultData(true, "Não existe alunos matriculados na turma.", HttpStatusCode.OK);

                currentClass.Students.Clear();

                await classRepository.UpdateAsync(currentClass);
                return new ServiceResultData(true, $"Todos os alunos foram retirados da turma.", HttpStatusCode.OK);
            }
            catch
            {
                return ServiceResultData.Error("Ocorreu um erro interno ao atualizar a turma no banco de dados.");
            }
        }

        /// <summary>
        /// Remove um estudante matrículado na turma por seu registro único.
        /// </summary>
        public async Task<IServiceResultData> RemoveStudent(string? classRegistry, string? studentRegistry)
        {
            if (string.IsNullOrWhiteSpace(classRegistry))
                return ServiceResultData.Error("O registro da turma não pode ser nula.");

            if (string.IsNullOrWhiteSpace(studentRegistry))
                return ServiceResultData.Error("O registro do aluno não pode ser nula.");

            classRegistry = classRegistry.Trim().ToUpper();
            studentRegistry = studentRegistry.Trim().ToUpper();

            try
            {
                var currentClass = await classRepository.GetAsync(c => c.Registry == classRegistry, true);

                if (currentClass == null)
                    return ServiceResultData.Error($"Não foi encontrada uma turma com esse registro.");

                var currentStudent = currentClass.Students.FirstOrDefault(s => s.Registry == studentRegistry);

                if (currentStudent == null)
                    return ServiceResultData.Error($"O aluno não está matrículado nesta turma.");

                currentClass.Students.Remove(currentStudent);

                await classRepository.UpdateAsync(currentClass);
                return new ServiceResultData(true, $"O aluno foi removido da turma.", HttpStatusCode.OK);
            }
            catch
            {
                return ServiceResultData.Error("Ocorreu um erro interno ao remover o aluno da turma no banco de dados.");
            }
        }

        /// <summary>
        /// Matricula um aluno em uma turma.
        /// </summary>
        public async Task<IServiceResultData> RegisterStudent(string? classRegistry, string? studentRegistry)
        {
            if (string.IsNullOrWhiteSpace(classRegistry))
                return ServiceResultData.Error("O registro da turma não pode ser nula.");

            if (string.IsNullOrWhiteSpace(studentRegistry))
                return ServiceResultData.Error("O registro do aluno não pode ser nula.");

            classRegistry = classRegistry.Trim().ToUpper();
            studentRegistry = studentRegistry.Trim().ToUpper();

            try
            {
                var currentClass = await classRepository.GetAsync(c => c.Registry == classRegistry, true);

                if (currentClass == null)
                    return ServiceResultData.Error($"Não foi encontrada uma turma com esse registro.");

                var currentStudent = currentClass.Students.FirstOrDefault(s => s.Registry == studentRegistry);

                if (currentStudent != null)
                    return ServiceResultData.Error($"O aluno já está matrículado nesta turma.");

                currentStudent = await studentRepository.GetAsync(s => s.Registry == studentRegistry);

                if(currentClass.Students.Count == 5)
                    return ServiceResultData.Error($"A turma não pode exceder o número de 5 alunos matriculados.");

                currentClass.Students.Add(currentStudent);
                await classRepository.UpdateAsync(currentClass);

                return new ServiceResultData(true, $"O aluno foi matriculado na turma solicitada.", HttpStatusCode.OK);
            }
            catch
            {
                return ServiceResultData.Error("Ocorreu um erro interno ao acessar o banco de dados.");
            }
        }

        private static IServiceResultData? RequestValidation(string? registry, ClassUpdateRequest request)
        {
            if (request == null)
                return ServiceResultData.Error("A requisição no pode ser nula.");

            if (string.IsNullOrWhiteSpace(registry))
                return ServiceResultData.Error("O registro da turma não pode ser nula.");

            if (request.Number <= 0)
                return ServiceResultData.Error("A turma deve conter um número maior do que 0.");

            if (string.IsNullOrWhiteSpace(request.Year)
                || request.Year.Length > 6
                || !YearValidation(request.Year))
                return ServiceResultData.Error("A turma deve conter um ano letivo válido, com até 6 digítos e no formato YYYY ou YYYY.S, ex.: 2023, 2023.1 (equivale a 2023 semestre 1)");

            if ((int)request.Level <= 0 || (int)request.Level > 3)
                return ServiceResultData.Error("A turma deve conter um nível válido: 1 - Básico, 2 - Intermediário, 3 - Avançado");

            return null;
        }

        private static IServiceResultData? RequestValidation(ClassCreateRequest request)
        {
            if (request == null)
                return ServiceResultData.Error("A requisição no pode ser nula.");

            if (request.Number <= 0)
                return ServiceResultData.Error("A turma deve conter um número maior do que 0.");

            if (string.IsNullOrWhiteSpace(request.Year) 
                || request.Year.Length > 6
                || !YearValidation(request.Year))
                return ServiceResultData.Error("A turma deve conter um ano letivo válido, com até 6 digítos e no formato YYYY ou YYYY.S, ex.: 2023, 2023.1 (equivale a 2023 semestre 1)");

            if((int)request.Level <= 0 || (int)request.Level > 3)
                return ServiceResultData.Error("A turma deve conter um nível válido: 1 - Básico, 2 - Intermediário, 3 - Avançado");

            return null;
        }

        private static bool YearValidation(string year)
        {
            return decimal.TryParse(year, out _);            
        }
    }
}