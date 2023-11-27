using API.Extensions;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Esse texto abaixo foi escrito primeiro na questão de uso de middlewares. E bem, como sabemos, os middlewares deve ser colocados
// no começo da pipeline de requisições HTTP. E agora iremos alocar o nosso ExceptionMiddleware no topo, para capturar logo todos
// os erros e retornar para o cliente de forma x ou y, dependendo do ambiente que se encontra. E para fazer isso é bem simples:
app.UseMiddleware<ExceptionMiddleware>(); // E agora temos nosso retorno de exceção como foi desejado

// Aqui usaremos o middleware que vai fazer com que a nossa pipeline passe por aqui durante o processo de rodar as coisas do app
// E para isso precisamos criar ele antes do pipeline de requisições http
// Aqui iremos fazer com que a aplicação passe sempre pelo endpoint errors/{code}, para ver se realmente existe algum erro
// No caso, o que estamos fazendo aqui, é usar um middlewere para evitar de retornar as respostas automaticas geradas pelo .net
// Iremos ao invés disso, direcionar o usuário para a errorController, em caso de erro
// E para isso, usamos a função do app chamada de UseStatusCodePagesWithReExecute. Ela recebe como argumento o endpoint controlador
// e no caso desse zero, é aquela mesma parada relacionado a argumentos de uma função, o 0 dentro das chaves vai ser o primeiro, 1 o segundo e por ai vai...
app.UseStatusCodePagesWithReExecute("/errors/{0}");

// Configure the HTTP request pipeline.
// Removemos o uso do swagger apenas em desenvolvimento. Agora ele roda em dev e prod
// E claro, isso foi feito pelo fato de que essa API não vai ser publica. Caso seja, evite isso
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();

// Permite uso de arquivos estaticos, como imagens, fontes, txt, etc
// Para isso se deve criar a pasta wwwroot. E fizemos isso depois de criar o ProductUrlResolver
// pois com o link completo da imagem, temos acesso a ela clicando nela, mas apenas se botar esse trecho de código e armazenar
// as imagens no wwwroot
// Outra coisa, a ordem importa, sendo assim, essa instrução deve ficar antes da autorização
app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

// Código para fazer com que o migration seja aplicado sempre que rodamos o programa
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{

    logger.LogError(ex, "Um erro ocorreu durante a aplicação da migration");
}


app.Run();
