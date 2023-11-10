using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.ProductRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Adicionando o AutoMapper como serviço
// É preciso dizer onde ele está localizado, e o parametro passado faz isso, dizendo o local dele
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// Permite uso de arquivos estaticos, como imagens, fontes, txt, etc
// Para isso se deve criar a pasta wwwroot. E fizemos isso depois de criar o ProductUrlResolver
// pois com o link completo da imagem, temos acesso a ela clicando nela, mas apenas se botar esse trecho de código e armazenar
// as imagens no wwwroot
// Outra coisa, a ordem importa, sendo assim, essa instrução deve ficar antes da autorização
app.UseStaticFiles();

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
