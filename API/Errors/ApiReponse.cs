using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiReponse
    {
        // Message fica como null, pois pode ser que a resposta do erro não tenha uma mensagem. Dessa forma criaremos uma nós mesmos
        // Ou seja, se na verificação a mensagem vier como null, iremos criar uma mensagem personalizada para o erro
        public ApiReponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            // O que é o ??. Bem ele se chama Null Coalescing Operator. E ele basicamente vê se o primeiro argumento é null ou não
            // e se o message for null, ele executa o que vem depois dele, que no nosso caso é a função que cria uma mensagem personalizada
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            // Aqui iremos usar a nova forma chique de switch que existe a partir do C# 8
            return statusCode switch
            {
                400 => "A bad request, you have made.",
                401 => "Authorized, you are not.",
                404 => "Resource found, i was not.",
                500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate. Hate leads to career change.",
                _ => null
            };
        }
    }
}