using API.Errors;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;


namespace API.Controllers
{
    // Aqui a gente da um override no root que a gente pega da BaseApiController
    // Isso aqui está sendo criar para lhe dar com erros de endpoints que não existem
    // E para isso a gente precisa alterar a pipeline do funcionamento da nossa aplicação através do uso de middleware
    // No caso, alocado na Program.cs
    [Route("errors/{code}")]

    // Aqui a gente vai fazer com que o swagger ignore esse controller. Se não fizer isso, ele vai reclamar que o controller não
    // tem uma rota definida para as suas funções. No caso da error controller, não podemos fazer isso, pois não sabemos qual tipo
    // de requisição ele vai usar, ele vai ser passado para outros controladores para retornar erro, podendo ser get, post, put, etc
    // E bem, usando o decorador abaixo, a gente faz com que o swagger ignore esse controller, sendo assim, impedido o erro
    // E isso também serve para caso a gente queria deixar algo de fora do swagger, por exemplo, não documentar algum controlador
    // como é feito no WeatherForecast
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiReponse(code));
        }
    }
}