using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Specifications
{
    // Bem, o repositiorio de especificação vai replicar o comportamento do que se acontece em funções de acesso a dados
    // Como base, iremos replicar agora um FindAsync, de forma que ele não seja puramente genérico e que seja entendivel e usavel em qualquer contexto, data set
    // Ou seja, ao invés de termos algo do tipo: IReadList<T> FindAsync(Expression<Func<T, bool>> alguma coisa), que é uma função
    // que ao se olhar não se da pra entender muito o que ela deve retornar, sem falar que é uma leaky abstraction, ou seja
    // ela traz mais dados do que o necessário, iremos replicar cada passo de uma função de acesso a dados que nos retorna dados especificos
    // de forma que não traga mais dados do que o necessário e satisfaça diversos contextos
    public interface ISpecification<T>
    {
        // No caso dessa expressão, ela replica o comportamento do where em uma consulta, os critérios do que deve ser trazido
        Expression<Func<T, bool>> Criteria { get; }

        // E aqui ira replicar o comportamento do include, de no caso de eu querer trazer algo junto dos dados especificados no critério
        // Como algum objeto que está ligado ao objeto especificado. Tipo um produto, e uma lista de marcas que vendem ele
        List<Expression<Func<T, object>>> Includes { get; }
    }

    // E agora iremos criar uma classe chamada de BaseSpecification, da qual vai herdar essa interface. É lá onde iremos criar as
    // funções que irão realizar as buscas que desejamos
}