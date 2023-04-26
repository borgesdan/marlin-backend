using MarlinAPI.Domain.Contracts;
using MarlinAPI.Domain.Entities;
using MarlinAPI.Repository;
using System.Net;
using System.Net.Mail;

namespace MarlinAPI.Service
{
    public class StudentService
    {
        private readonly StudentRepository studentRepository;
        private readonly ClassRepository classRepository;
        private readonly ClassService classService;

        public StudentService(
            StudentRepository studentRepository,
            ClassRepository classRepository,
            ClassService classService)
        {
            this.studentRepository = studentRepository;
            this.classRepository = classRepository;
            this.classService = classService;
        }

        /// <summary>
        /// Cadastra uma novo estudante com, no mínimo, uma turma para matrícula.
        /// </summary>
        public async Task<IServiceResultData> CreateAsync(StudentCreateRequest request)
        {
            var validationResult = RequestValidation(request);

            if (validationResult != null)
                return validationResult;

            var transactResult = await CreateTransactionAsync(request);

            if (!transactResult.Succeeded)
                return transactResult;

            var transactData = transactResult.GetData<StudentEntity>();

            return ServiceResultData.Ok(new StudentCreateResponse() { StudentRegistry = transactData.Registry });
        }

        /// <summary>
        /// Atualiza os dados de um aluno.
        /// </summary>
        public async Task<IServiceResultData> UpdateAsync(string? registry, StudentUpdateRequest request)
        {
            var validationResult = RequestValidation(request);

            if (validationResult != null)
                return validationResult;

            try
            {
                var currentStudent = await studentRepository.GetAsync(c => c.Registry == registry);

                if (currentStudent == null)
                    return ServiceResultData.Error($"Não foi encontrado um aluno com esse registro.");

                currentStudent.FullName = request.FullName;
                currentStudent.CPF = request.CPF;
                currentStudent.Email = request.Email;

                await studentRepository.UpdateAsync(currentStudent);
                return new ServiceResultData(true, $"O aluno foi atualizado com sucesso.", HttpStatusCode.OK);

            }
            catch
            {
                return ServiceResultData.Error("Ocorreu um erro interno acessar o banco de dados.");
            }
        }

        /// <summary>
        /// Excluí um aluno somente se ele não estiver associado a uma turma.
        /// </summary>
        public async Task<IServiceResultData> DeleteAsync(string? registry)
        {
            if (string.IsNullOrWhiteSpace(registry))
                return ServiceResultData.Error("O registro do aluno não pode ser nulo.");

            registry = registry.Trim().ToUpper();

            try
            {
                var currentStudent = await studentRepository.GetAsync(c => c.Registry == registry);

                if (currentStudent == null)
                    return ServiceResultData.Error($"Não foi encontrado um aluno com esse registro.");

                var classses = await classRepository.GetAllByStudentRegistryAsync(registry);

                if(classses.Any())
                    return ServiceResultData.Error($"Não foi possível excluir o registro do aluno pois ele está associado a uma turma.");

                await studentRepository.DeleteAsync(currentStudent);

                return new ServiceResultData(true, $"O registro do aluno do excluído com sucesso.", HttpStatusCode.OK);
            }
            catch
            {
                return ServiceResultData.Error("Ocorreu um erro interno ao deletar a turma no banco de dados.");
            }        
        }

        /// <summary>
        /// Obtém os dados de um aluno.
        /// </summary>
        public async Task<IServiceResultData> GetAsync(string? registry)
        {
            if (string.IsNullOrWhiteSpace(registry))
                return ServiceResultData.Error("O registro do aluno não pode ser nulo.");

            registry = registry.Trim().ToUpper();

            try
            {
                var currentStudent = await studentRepository.GetAsync(c => c.Registry == registry);

                if (currentStudent == null)
                    return ServiceResultData.Error($"Não foi encontrada um aluno com esse registro.");

                return ServiceResultData.Ok(new StudentGetResponse(currentStudent));
            }
            catch
            {
                return ServiceResultData.Error("Ocorreu um erro interno ao deletar a turma no banco de dados.");
            }
        }

        /// <summary>
        /// Obtém todas os alunos matrículados.
        /// </summary>
        public async Task<IServiceResultData> GetAllAsync()
        {
            try
            {
                var students = await studentRepository.GetAllAsync();

                return ServiceResultData.Ok(students.Select(c => new StudentGetResponse(c)));
            }
            catch
            {
                return ServiceResultData.Error("Ocorreu um erro interno ao obter a turma no banco de dados.");
            }
        }

        /// <summary>
        /// Desassocia o aluno de todas as suas turmas.
        /// </summary>
        public async Task<IServiceResultData> RemoveAllClasses(string? registry)
        {
            if (string.IsNullOrWhiteSpace(registry))
                return ServiceResultData.Error("O registro do aluno não pode ser nulo.");

            registry = registry.ToUpper();
            
            try
            {
                var currentStudent = await studentRepository.GetAsync(c => c.Registry == registry);

                if (currentStudent == null)
                    return ServiceResultData.Error($"Não foi encontrada um aluno com esse registro.");

                var classses = await classRepository.GetAllByStudentRegistryAsync(registry, true);

                if (classses.Any())
                    return new ServiceResultData(true, "O aluno não está associado a nenhuma turma.", HttpStatusCode.OK);

                foreach(var @class in classses)
                {
                    @class.Students.Remove(currentStudent);
                    await classRepository.UpdateAsync(@class);
                }

                return new ServiceResultData(true, $"O aluno foi removido de todas as turmas.", HttpStatusCode.OK);
            }
            catch
            {
                return ServiceResultData.Error("Ocorreu um erro interno ao deletar a turma no banco de dados.");
            }
        }

        private static IServiceResultData? RequestValidation(StudentUpdateRequest request)
        {
            if (request == null)
                return ServiceResultData.Error("A requisição no pode ser nula.");

            if (string.IsNullOrWhiteSpace(request.FullName) || request.FullName.Length > 255)
                return ServiceResultData.Error("O nome do aluno deve ser preenchido e não pode ter mais do que 255 caracteres.");

            if (string.IsNullOrWhiteSpace(request.CPF) || !request.CPF.IsValidCPF())
                return ServiceResultData.Error("O cpf do aluno é inválido.");

            if (string.IsNullOrWhiteSpace(request.Email) || !MailAddress.TryCreate(request.Email, out _) || request.Email.Length > 255)
                return ServiceResultData.Error("O email deve ser válido e não pode ter mais do que 255 caracteres.");            

            return null;
        }

        private static IServiceResultData? RequestValidation(StudentCreateRequest request)
        {
            if (request == null)
                return ServiceResultData.Error("A requisição no pode ser nula.");

            if (string.IsNullOrWhiteSpace(request.FullName) || request.FullName.Length > 255)
                return ServiceResultData.Error("O nome do aluno deve ser preenchido e não pode ter mais do que 255 caracteres.");

            if (string.IsNullOrWhiteSpace(request.CPF) || !request.CPF.IsValidCPF())
                return ServiceResultData.Error("O cpf do aluno é inválido.");

            if (string.IsNullOrWhiteSpace(request.Email) || !MailAddress.TryCreate(request.Email, out _) || request.Email.Length > 255)
                return ServiceResultData.Error("O email deve ser válido e não pode ter mais do que 255 caracteres.");

            if (request.Classes == null || request.Classes.Count == 0)
                return ServiceResultData.Error("O aluno deve ser cadastrado em, no mínimo, uma turma.");

            return null;
        }

        private async Task<IServiceResultData> CreateTransactionAsync(StudentCreateRequest request)
        {
            var context = studentRepository.AppContext;
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var entity = new StudentEntity
                {
                    FullName = request.FullName,
                    CPF = request.CPF,
                    Email = request.Email,
                    Registry = Helpers.CreateStudentRegistry(request.FullName),
                };

                var currentStudent = await studentRepository.GetAsync(s => s.CPF == entity.CPF);

                if(currentStudent != null)
                    throw new Exception("Já existe um aluno cadastrado com o CPF informado.");

                await studentRepository.CreateAsync(entity);

                foreach(var @class in request.Classes)
                {
                    var classEntity = await classRepository.GetAsync(@class.ClassRegistry, true) ?? throw new NullReferenceException("A turma selecionada não existe.");
                    
                    if (classEntity.Students.Count == 5)
                        throw new Exception("A turma selecionada não pode receber mais alunos.");

                    classEntity.Students.Add(entity);

                    await classRepository.UpdateAsync(classEntity);
                }

                transaction.Commit();

                return ServiceResultData.Ok(entity);
            }
            catch(Exception ex) 
            {
                transaction.Rollback();
                return ServiceResultData.Error($"Ocorreu um erro ao gravar os dados no banco de dados: {ex.Message}");
            }
        }
    }
}