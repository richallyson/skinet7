using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            // Aqui verificamos todas as expressões que não retornam uma lista. Caso não sejam nulas, são aplicadas

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            // É importante a paginação vir sempre ao fim, pois primeiro iremos filtra tudo, com o que a gente quer trazer, e na ordem
            // que queremos trazer e depois disso, iremos aplicar a paginação. E claro, que isso se aplica também a ordenação.
            // Iremos ordenar os dados trazidos de acordo com os critérios, e só depois iremos ordenar eles e por fim paginar os mesmos
            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // Bem, como funciona o Aggregate. Ele pode ter uma ou duas sobrecargas, no nosso caso iremos usar duas.
            // Primeiro pegamos a nossa query que já pode ter um Criteria ou não, que no nosso caso é um Where
            // Depois disso iremos pegar essa query e chamar o aggregate passando os Includes do spec
            // E o que ele vai fazer? Vai pegar todos os includes passados e adicionar na nossa query de acordo com a ordem escolhida
            // Dentro da primeira sobrecarga temos o query, que é o valor base, por assim dizer
            // E depois temos em uma função anonima o current e o include. O current representa o estado atual da primeira sobrecarga
            // no nosso caso, o query. E o include representa o que vai ser adicionado. E bem, como ocorre o processo.
            // Lembresse que o query recebe um inputQuery que é um IQueryable, dessa forma ele vai ter acesso as funções desse tipo
            // O retorno dessa função vai ser a adição do o include passado para o current, e dessa forma acontece a primeira iteração
            // Na próxima iteração o current vai mudar, pois ele agora representa o estado atual da query com a adição do include
            // E dessa forma ele vai ser por exemplo: db.Products.Where(x => x.Produto == "Verde").Includes(x => x.Autor);
            // A partir da próxima iteração, caso aconteceça, ele vai adicionar o próximo include ao fim da query e ai por diante
            // até percorrer tudo, e dessa forma deixando a nossa query completa
            // No caso de uma sobrecargar, ver esse link: https://chat.openai.com/share/b32947be-f11f-4d93-89ac-f760879424f6
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}