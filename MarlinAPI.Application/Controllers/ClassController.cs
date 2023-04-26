using MarlinAPI.Domain.Contracts;
using MarlinAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace MarlinAPI.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : DefaultController
    {
        private readonly ClassService _classService;

        public ClassController(ClassService classService)
        {
            _classService = classService;
        }

        /// <summary>Cadastra uma nova turma.</summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// <code>
        ///{
        ///  "number": 1,
        ///  "year": "2023.1",
        ///  "level": 3
        ///}
        ///</code>
        ///
        /// Todos os campos são obrigatórios.
        /// O campo year segue o padrão: 2023 ou 2023.1, onde .1 se refere ao semestre do ano letivo.
        /// O campo level representa o nível 1- Básico, 2- Intermediário, 3- Avançado
        ///
        /// Exemplo de resposta:
        /// <code>
        ///{
        ///  "succeeded": true,
        ///  "message": "A turma foi criada com sucesso. Registro CL8C7A1C28"
        ///}
        /// </code>
        /// 
        /// Retorna o registro único da turma (ex.: CL8C7A1C28) para ser utilizado em futuras requisições.
        /// </remarks>  
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClassCreateRequest request)
           => ConvertData(await _classService.CreateAsync(request));

        /// <summary>Atualiza o cadastro de uma turma ao informar seu registro único.</summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// <code>
        ///{
        ///  "number": 2,
        ///  "year": "2023",
        ///  "level": 2
        ///}
        ///</code>
        ///
        /// Todos os campos são obrigatórios.
        /// O campo year segue o padrão: 2023 ou 2023.1, onde .1 se refere ao semestre do ano letivo.
        /// O campo level representa o nível 1- Básico, 2- Intermediário, 3- Avançado
        ///
        /// Exemplo de resposta:
        /// <code>
        ///{
        /// "succeeded": true,
        /// "message": "A turma foi atualizada com sucesso."
        ///}
        /// </code>
        /// </remarks>  
        [HttpPatch("{registry}")]
        public async Task<IActionResult> Update(string? registry, [FromBody] ClassUpdateRequest request)
           => ConvertData(await _classService.UpdateAsync(registry, request));

        /// <summary>Exclui a turma pelo seu registro.</summary>
        /// <remarks>       
        /// A turma só poderá ser excluída se não contiver alunos matrículados nela.
        ///
        /// Exemplo de resposta:
        /// <code>
        ///{
        /// "succeeded": true,
        /// "message": "A turma foi excluída com sucesso."
        ///}
        /// </code>
        /// </remarks>  
        [HttpDelete("{registry}")]
        public async Task<IActionResult> Delete(string? registry)
           => ConvertData(await _classService.DeleteAsync(registry));

        /// <summary>Obtém uma turma pelo seu registro.</summary>
        /// <remarks>    
        ///
        /// Exemplo de resposta:
        /// <code>
        ///{
        ///  "data": {
        ///    "number": 2,
        ///    "year": "2023",
        ///    "level": "Intermediário",
        ///    "registry": "CL8C7A1C28",
        ///    "students": []
        ///  },
        ///  "succeeded": true,
        ///  "message": null
        ///}
        /// </code>
        /// </remarks>  
        [HttpGet("{registry}")]
        public async Task<IActionResult> Get(string? registry)
           => ConvertData(await _classService.GetAsync(registry));

        /// <summary>
        /// Obtém todas as turmas cadastradas.
        /// </summary>
        /// <remarks>  
        /// 
        /// Exemplo de resposta:
        /// <code>
        ///{
        ///  "data": [
        ///    {
        ///      "number": 1,
        ///      "year": "2023.1",
        ///      "level": "Avançado",
        ///      "registry": "CL9B651A35"
        ///    },
        ///    {
        ///      "number": 2,
        ///      "year": "2023",
        ///      "level": "Intermediário",
        ///      "registry": "CL8C7A1C28"
        ///    }
        ///  ],
        ///  "succeeded": true,
        ///  "message": null
        ///}
        /// </code>
        /// </remarks>  
        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
           => ConvertData(await _classService.GetAllAsync());

        /// <summary>Remove todos os alunos de uma turma.</summary>
        /// <remarks>
        ///
        /// Exemplo de resposta:
        /// <code>
        ///{
        /// "succeeded": true,
        /// "message": "Todos os alunos foram retirados da turma."
        ///}
        /// </code>
        /// </remarks>  
        [HttpPost("{registry}/students/detach/all")]
        public async Task<IActionResult> RemoveAll(string? registry)
           => ConvertData(await _classService.RemoveAllStudents(registry));

        /// <summary>Remove um aluno da turma por seu registro de aluno.</summary>
        /// <remarks>
        ///
        /// Exemplo de resposta:
        /// <code>
        ///{
        /// "succeeded": true,
        /// "message": "O aluno foi removido da turma."
        ///}
        /// </code>
        /// </remarks>  
        [HttpPost("{classRegistry}/students/detach/{studentRegistry}")]
        public async Task<IActionResult> RemoveStudent(string? classRegistry, string? studentRegistry)
           => ConvertData(await _classService.RemoveStudent(classRegistry, studentRegistry));

        /// <summary>
        /// Matricula um aluno a uma turma específica.
        /// </summary>
        ///  /// <remarks>
        ///
        /// Exemplo de resposta:
        /// <code>
        ///{
        /// "succeeded": true,
        /// "message": "O aluno foi matriculado na turma solicitada."
        ///}
        /// </code>
        /// </remarks>  
        [HttpPost("{classRegistry}/students/register/{studentRegistry}")]
        public async Task<IActionResult> RegisterStudent(string? classRegistry, string? studentRegistry)
           => ConvertData(await _classService.RegisterStudent(classRegistry, studentRegistry));
    }
}
