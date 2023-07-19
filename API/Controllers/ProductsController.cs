using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController] // Esse bonito aqui faz as validações da parada. Ajuda a nossa vida
    [Route("api/[controller]")] // Essa forma de chamar o nome do controlador eu n conhecia. Massa dms papai
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly StoreContext _context;

        public ProductsController(ILogger<ProductsController> logger, StoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Usar o dotnet watch --no-hot-reload, ao invés só do watch. Porquê? Pois só o watch não da reload em algumas mudanças
        // Já essa versão, quando vê que não consegue dar reload em algumas mudanças, ele reseta a aplicação para alterar as mesmas
        // Porém, mesmo essa versão ainda tem problemas kk Tipo, se eu criar um novo controlador, ele n vai detectar
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _context.Products.FindAsync(id); ;
        }
    }
}