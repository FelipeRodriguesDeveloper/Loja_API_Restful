using Loja.Dominio.Interfaces;
using Loja.Dominio.Interfaces.IRepositorios;
using Loja.Infraestrutura.DBContexto;
using Loja.Infraestrutura.Repositorios;
using Loja.Infraestrutura.UnidadeDeTrabalho;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DBContextLoja>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoBancoLoja"), b => b.MigrationsAssembly("Loja.Infraestrutura")));

//Interface e Repositório:
builder.Services.AddScoped<IRepositorioProdutos, RepositorioProdutos>();
builder.Services.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API para Produtos",
        Version = "v1",
        Description = "Esta API Restful foi desenvolvida com ASP.NET Core 6.0.4 + Entity Framework Core 6.0.5 + FluentAPI + DataAnnotation + SQL Server + Swagger + OpenAPI" +
                       " + Padrões DDD, Repository e Unit of Work"
        ,
        Contact = new OpenApiContact
        {
            Name = "Desenvolvedor Felipe Rodrigues",
            Email = "feliperodriguesdeveloper@hotmail.com",
            Url = new Uri("https://github.com/FelipeRodriguesDeveloper")
        }
    });

    //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProdutosAPI v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
