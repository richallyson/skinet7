using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
            
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }


        // E bem, como já dito, esse Includes vai resolver o problema dos nossos includes em uma função de acesso a dados
        // Ex: db.Products.Where(x => x.ProductId == Id).Includes(x => x.ProductsType).ToListAsync()
        // E o que iremos fazer aqui é passar essas queries do Includes pro ToListAsync(), pois como sabemos, é essa função
        // Que recebe os parametos anteriores, para poder rodar de fato a query, e de alguma forma precisamos mandar esses dados para ela
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        // Aqui temos a função que adiciona a expressão a o nosso Includes. Mas ainda não passamos de fato o Inclutes pro ToListAsync()
        // Para isso precisamos de uma outra classe chamada de Specification Evaluator, que vai validar o dado recebido, e passar
        // de fato um IQueryable par o ToListAsync(), que é o tipo que ele espera para poder fazer de fato a query
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
    }
}