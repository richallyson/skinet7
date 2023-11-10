using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<Product, ProductToReturnDto>()
                // Mapeando o retorno para retornar o valor desejado. Nesse caso queremos o nome do Brand do produto
                // assim como o nome do tipo do produto. Sem isso ele vai retornar Core.Entities."Modelo", pois ele retorna o nome
                // do objeto já que na hora do mapper ele vai ver que a propriedade no Dto é uma string
                .ForMember(p => p.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(p => p.ProductType, o => o.MapFrom(s => s.ProductType.Name))

                // E aqui iremos fazer um mapeamento para a forma como o link da PictureUrl. Da forma que está, está pegando
                // o caminho da aplicação em si, da pasta. Ex: images/products/sb-ang2.png
                // Porém, queremos especificar o link de acordo com o link da nossa aplicação, que antes de tudo tem o https://localhost:5001/
                // Para isso foi criada outra classe de Helper, que vai fazer a concatenação do nosso link localhost, com o caminho da imagem
                // Dessa forma, teremos o link completo da imagem
                .ForMember(p => p.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
        }
    }
}