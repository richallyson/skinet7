using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // O RequesteDelegate é uma função que processa uma requisição HTTP
                // E no caso de não existir uma exceção, queremos que o nosso middleware continue para o próximo middleware dentro da pipeline
                // E para isso usamos a função do RequestDelegate, passando o nosso context
                // E bem, isso significa que se não existir nenhuma exceção, o request move para o seu próximo estágio
                await _next(context);
            }
            catch (Exception ex)
            {
                // Porém, se houver uma exceção, nós queremos pegar essa exceção e trabalhar com ela
                // E para isso iremos criar a nosso próprio manipulador de respostas de exceção
                // E bem, o LogError irá retornar a mensagem para o nosso LoggingSystem, que no nosso caso é o nosso console
                // E caso tenhamos um erro, iremos ver dentro dele e não no front
                _logger.LogError(ex, ex.Message);

                // E com o erro printado apenas para o desenvolvedor, agora queremos pega esse erro, e manipular ele para mandar para o client
                // Ou seja, iremos escrever a nossa própria resposta. Iremos escrever a resposta dentro do context reponse
                // Primeiro especificamos o tipo da resposta que vai ser mandada, que no nosso caso vai ser um json
                context.Response.ContentType = "application/json";

                // E aqui iremos setar o erro para ser um 500. Lembrando que aqui estamos trabalhando com exceções. Ou seja, cenários
                // onde você tenta pegar algo que não existe dentro do servidor. Dessa forma vai dar um erro no servidor, pois ele não saber manipular essa chamada
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // E aqui de fato iremos criar a respost personalizada. Onde verificamos o ambiente da qual ela está sendo mandada
                // e baseada no ambiente, mandaremos respostas mais detalhadas ou não. No caso, no ambiente Dev, teremos respostas
                // Mais detalhadas pois de fato precisamos disso para criar melhorias e corrigir erros na nossa aplicação
                // Em produção mandaremos apenas o StatusCode do erro, pois o cliente não tem a necessidade de saber sobre especificidades
                // Além de não poder ter a 
                var response = _env.IsDevelopment()
                    ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiException((int)HttpStatusCode.InternalServerError);

                // Essa opção serve para poder retornar o json em camelCase. Pois fora do contexto de um APIController
                // ele não faz esse tratamento de retornar as palavras de forma "correta". Sendo assim, devemos especificar
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                // E agora iremos transformar esse objeto em um json
                var json = JsonSerializer.Serialize(response, options);

                // E por fim, passamos a resposta para o context. Como dissesmos acima, o tipo de conteúdo vai ser um json
                // Dessa forma, caso tenha um erro, o context, do qual encapsula tudo sobre uma unica requisição http
                // Vai pegar o erro esse erro e retornar para nós
                await context.Response.WriteAsync(json);
            }
        }
    }
}