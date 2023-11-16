using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    // Bem preciamos também criar o tratamento para as excessões, como no caso de eu tentar achar algo que não existe no servidor
    // A mensagem retornada é aquela longa mensagem, cheia de coisa, mas boa, pois ela nos mostra onde o erro acontece exatamente
    // E para isso capturaremos o StackTrace, que é gerado quando ocorre uma exceção em nossos métodos
    // E para isso iremos criar um Middleware que ira capturar as exceptions que usará essa classe sempra que tivermos uma: ExceptionMiddleware
    public class ApiException : ApiReponse
    {
        public ApiException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }

        // Essa variavel vai armazenar o StackTrace, que é justamente  mostra a sequência de chamadas de métodos que levaram até o ponto
        // onde a exceção foi lançada, começando com o método onde a exceção ocorreu e rastreando todos os métodos anteriores até o ponto 
        // de entrada do programa.
        // Para mais informações: https://chat.openai.com/share/5a457b6e-25d1-4175-acd9-b655da230305
        public string Details { get; set; }
    }
}