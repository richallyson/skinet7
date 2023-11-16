using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.ProductRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        // Bem, como sabemos essa é uma classe de extensão, que extende, aplica mais funções ao que se quer, no nosso caso
        // ao IServiceColletion, e isso se da através do primeiro parametro, com o this. Ou seja, quando essa função for chamada
        // ela não vai precisar de dois parametros para funcionar, apenas de um, pois o primeiro parametro é relacionado a classe ou
        // interface que ela está extendendo. No nosso caso, precisaremos apenas definir o segundo parametro na sua chamada, no caso
        // o config. E bem, porquê dessa classe? É para evitar que nossa classe Program.cs fique com muitas informações. Aqui a gente
        // vai colocar todos os serviços que iremos usar na nossa aplicação e retornar eles, e na program, chamaremos apenas essa função
        // E para ver isso melhor, basta ir na classe Program e ver como essa função é chamada
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Adicionando o AutoMapper como serviço
            // É preciso dizer onde ele está localizado, e o parametro passado faz isso, dizendo o local dele
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Aqui: https://chat.openai.com/share/b5c20287-1bd2-4c8d-9b19-6581a203cedb
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ActionContext =>
                {
                    var errors = ActionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorsReponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorsReponse);
                };
            });

            return services;
        }
    }
}