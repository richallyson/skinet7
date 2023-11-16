using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // E bem, todos os nossos controllers vão herdar desse controller, e por causa disso, não tem a necessidade mais de dizer que eles
    // são um [ApiController], assim como também passar a sua Route, pois ao derivar desse BaseController, eles vão possuir 
    // as funcionalidades para acessar seus endpoints
    [ApiController] // Esse bonito aqui faz as validações da parada. Ajuda a nossa vida
    [Route("api/[controller]")] // Essa forma de chamar o nome do controlador eu n conhecia. Massa dms papai
    public class BaseApiController : ControllerBase
    {

    }
}