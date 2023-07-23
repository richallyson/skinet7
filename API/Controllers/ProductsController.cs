using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController] // Esse bonito aqui faz as validações da parada. Ajuda a nossa vida
    [Route("api/[controller]")] // Essa forma de chamar o nome do controlador eu n conhecia. Massa dms papai
    public class ProductsController : ControllerBase
    {

        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;

        }

        // Usar o dotnet watch --no-hot-reload, ao invés só do watch. Porquê? Pois só o watch não da reload em algumas mudanças
        // Já essa versão, quando vê que não consegue dar reload em algumas mudanças, ele reseta a aplicação para alterar as mesmas
        // Porém, mesmo essa versão ainda tem problemas kk Tipo, se eu criar um novo controlador, ele n vai detectar
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _repository.GetProductsdAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _repository.GetProductIdAsync(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _repository.GetProductBrandsdAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _repository.GetProductTypesAsync());
        }
    }
}