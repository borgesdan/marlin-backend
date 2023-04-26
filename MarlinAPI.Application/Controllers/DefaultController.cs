using MarlinAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace MarlinAPI.Application.Controllers
{
    public class DefaultController : ControllerBase
    {
        /// <summary>
        /// Converte o tipo de dado recebido pelo serviço em um IActionResult válido
        /// a ser enviado como resposta do controle.
        /// </summary>        
        protected IActionResult ConvertData(IServiceResultData resultData)
        {
            if (resultData == null)
                NoContent();

            var result = new ObjectResult(resultData)
            {
                StatusCode = (int)resultData.StatusCode()
            };

            return result;
        }
    }
}
