using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
            : base(x => (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                        (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
                        (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId))
        {

            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);

            // Bem, explicando o applypaging e porquê ele faz essa multiplicação maluca com o pageIndex
            // E bem, o primero parametro que precisamos passar para função é o skip, ou seja, quantos objetos vão ser pulados
            // Ententendo que o pageSize é o numero de objetos que vai ser printado na lista e o pageIndex é o numero da página atual
            // precisamos fazer esse calculo para saber quantos objetos já foram printados. Por exemplo, eu tenho o pageSize 3
            // e o pageIndex 1. Dessa forma, estamos na primeira página retornando 3 objetos. Nesse caso, não precisamos pular nada
            // pois se estamos na primeira página, estamos trabalhando com os primeiros objetos retornados, ou seja, o pageIndex é 0 nesse caso
            // Agora se o pageSize é 2, e o pageIndex é 2, ou seja, a segunda página, precisamos subtrair o valor do pageIndex, para saber
            // quantos objetos já foram mostrados na tela anteriormente, para poder pular os itens que já foram printados
            // Dessa forma iremos ter o pageSize 3 * pageIndex 1, que é igual a 3, ou seja, iremos pular os 3 primeiros objetos
            // E o segundo parametro que é o take, vai pegar esses objetos e printar na tela
            // E como ultimo exemplo, supomos que o pageSize é 5, e o pageIndex é 6, fazendo o calculo, iremos ter um total de 25 objetos já mostrados
            // Ou seja, o que vai ser mostrado na tela, sáo os objetos do 26 ao 30, fazendo que que os outros 25 já mostrados sejam skipados
            // Para mais informações: https://chat.openai.com/share/d5b5894b-d78d-4f95-8746-40d8b110b346
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            // Aqui é uma forma inteligente de se ver o que vai ser passado no sort, para ser passado na query string
            // Dependendo do que for passado na query string, função x vai ro   dar. E isso abre para gente um leque gigante
            // de tipos de sorte por ascendencia ou descendencia que podemos fazer. Assim como também adaptar para outros contextos
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
        }

        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}