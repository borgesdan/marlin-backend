using MarlinAPI.Domain.Contracts;
using MarlinAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace MarlinAPI.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : DefaultController
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>Cadastra uma novo aluno.</summary>
        /// <remarks>
        /// O aluno precisa estar associado a, pelo menos, uma turma.
        /// 
        /// Exemplo de requisição:
        /// <code>
        ///{
        ///  "fullName": "Marcos Antônio da Silva",
        ///  "cpf": "803.730.250-41",
        ///  "email": "marcos@email.com",
        ///  "classes": [
        ///    {
        ///      "classRegistry": "CL8C7A1C28"
        ///    }
        ///  ]
        ///}
        ///</code>
        ///
        /// Exemplo de resposta:
        /// <code>
        ///{
        ///  "data": {
        ///    "studentRegistry": "MAR-2023-514E"
        ///  },
        ///  "succeeded": true,
        ///  "message": null
        ///}
        ///</code>
        ///
        /// Retorna o registro único do aluno (ex.: MAR-2023-514E) para ser utilizado em futuras requisições.
        /// </remarks>  
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentCreateRequest request)
           => ConvertData(await _studentService.CreateAsync(request));

        /// <summary>Atualiza os dados de um aluno por seu registro.</summary>
        /// <remarks>
        /// 
        /// Exemplo de requisição:
        /// <code>
        ///{
        ///  "fullName": "Marcos Rodrigues da Silva",
        ///  "cpf": "14739193043",
        ///  "email": "marcos@email.com"
        ///}
        ///</code>
        ///
        /// Exemplo de resposta:
        /// <code>
        ///{
        ///  "succeeded": true,
        ///  "message": "O aluno foi atualizado com sucesso."
        ///}
        /// </code>
        /// </remarks>  
        [HttpPatch("{registry}")]
        public async Task<IActionResult> Update(string? registry, [FromBody] StudentUpdateRequest request)
           => ConvertData(await _studentService.UpdateAsync(registry, request));

        /// <summary>Exclui um aluno pelo seu registro.</summary>
        /// <remarks>       
        /// O aluno só poderá ser excluído se não estiver associado a nenhuma turma.
        ///
        /// Exemplo de resposta:
        /// <code>
        ///{
        /// "succeeded": true,
        /// "message": "O aluno foi excluído com sucesso."
        ///}
        /// </code>
        /// </remarks>  
        [HttpDelete("{registry}")]
        public async Task<IActionResult> Delete(string? registry)
           => ConvertData(await _studentService.DeleteAsync(registry));

        /// <summary>Obtém um aluno pelo seu registro.</summary>
        /// <remarks>    
        ///
        /// Exemplo de resposta:
        /// <code>
        ///{
        ///  "data": {
        ///    "fullName": "Marcos antonio da silva",
        ///    "cpf": "147.391.930-43",
        ///    "email": "marcos@email.com",
        ///    "registry": "MAR-2023-514E"
        ///  },
        ///  "succeeded": true,
        ///  "message": null
        ///}
        /// </code>
        /// </remarks>  
        [HttpGet("{registry}")]
        public async Task<IActionResult> Get(string? registry)
           => ConvertData(await _studentService.GetAsync(registry));

        /// <summary>
        /// Desassocia o aluno de todas as suas turmas.
        /// </summary>
        /// <remarks>
        /// Exemplo de resposta:
        /// <code>
        ///{
        /// "succeeded": true,
        /// "message": "O aluno foi desassociado de todas as turmas com sucesso."
        ///}
        /// </code>
        /// </remarks>
        [HttpPost("{registry}/detach/all")]
        public async Task<IActionResult> DetachAll(string? registry)
         => ConvertData(await _studentService.GetAsync(registry));

        /// <summary>
        /// Obtém todos os alunos matriculados.
        /// </summary>
        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
           => ConvertData(await _studentService.GetAllAsync());
    }
}