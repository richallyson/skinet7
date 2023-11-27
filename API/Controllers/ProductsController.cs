using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {

        // Para entender melhor vá no GenericRepository
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo,
                                  IGenericRepository<ProductBrand> productBrandRepo,
                                  IGenericRepository<ProductType> productTypeRepo,
                                  IMapper mapper)
        {
            _productsRepo = productsRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }

        // Usar o dotnet watch --no-hot-reload, ao invés só do watch. Porquê? Pois só o watch não da reload em algumas mudanças
        // Já essa versão, quando vê que não consegue dar reload em algumas mudanças, ele reseta a aplicação para alterar as mesmas
        // Porém, mesmo essa versão ainda tem problemas kk Tipo, se eu criar um novo controlador, ele n vai detectar

        // [HttpGet]
        // public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(
        //     [FromQuery] ProductSpecParams productParams)
        // {
        //     var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
        //     var products = await _productsRepo.ListAsync(spec);

        //     return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        // }

        // Sobre como essa função está funcionando agora
        // O nosso retorno vai ser um Paginationdo tipo <ProductToReturnDtO>. Isso acontece, pois a classe pagination recebe
        // um tipo genérico.
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItens = await _productsRepo.CountAsync(countSpec);

            var products = await _productsRepo.ListAsync(spec);

            // Na classe Pagination tempos uma propriedade do tipo IReadOnlyList, ela é responsável por armazenar os dados covertidos
            // da nossa chamada. Ou seja, ela vai guardar os valores do DTO nela. Vai pegar a classe base, mapear e armazenar
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            // E no retorno, passamos uma Pagination do tipo ProductToReturnDto, passando os parametros especificos para ter uma
            // paginação perfeita. Ou seja, agora teremos um objeto retornado que tem o pageIndex, pageSize, o total de itens e os
            // dados em si
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItens, data));
        }

        [HttpGet("{id}")]
        // Esse decorador ajuda na documentação do swagger. Se a gente não especificar quais os tipos de resposta das funções
        // dos nossos controladores, por padrão ele sempre vai ter como retorno documentado o 200. Que no caso dessa
        // a gente pode ter tanto o 200 quanto o 404 em caso de não achar um produto na nossa base.
        // E como eu disse, por padrão tempos a 200, que retorna justamente o objeto retornado nessa função. E no swagger
        // ele vai está documentado bem bonitinho, mostrando como deve ser o retorno da dto em formato json. Já no caso da
        // resposta 404, ele não vai está documentado como deve ser a nossa ApiResponse, que tem um statusCode e uma message
        // E para isso, precisamos escificar qual o tipo de retorno que o 404 vai ter, que no nosso caso, vai ser um json
        // com o formato da classe ApiResponse. Como dito, a gente já tem o 200 como retorno default, mas é interessante documentar
        // e mostrar todos os tipos de retorno que as funções do controlador tem. E no caso, para ver como seria o retorno do 404
        // basta remover o typeof, para ver como ficaria.
        // E bem, apesar de eu ter dito que devemos fazer isso para todas as funções, não é bem assim kk, o interessante seria
        // o próprio swagger detectar o tipo de retorno que a função vai ter, seja erro ou o objeto em sim, e documentar isso.
        // Até agora não ensinou como é feito, mas quem sabe uma hora vai ser. Porém, esse fica aqui de exemplo
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);
            if (product == null) return NotFound(new ApiReponse(404));
            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }
}