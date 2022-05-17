using Loja.Dominio.Entidades;
using Loja.Dominio.Interfaces;
using Loja.Dominio.Interfaces.IRepositorios;
using Loja.Infraestrutura.DBContexto;
using Loja.Infraestrutura.Repositorios;
using Loja.Infraestrutura.UnidadeDeTrabalho;
using Loja.WebAPILogins.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DBContextLoja>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoBancoLoja"), b => b.MigrationsAssembly("Loja.Infraestrutura")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<DBContextLoja>();

//INTERFACE E REPOSITÓRIO
builder.Services.AddScoped<IRepositorioUsuarios, RepositorioUsuarios>();
builder.Services.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API para Logins",
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true, 
            ValidateIssuerSigningKey = true,

            //Configurações da sua propria empresa:
            ValidIssuer = "Teste.Securiry.Bearer",
            ValidAudience = "Teste.Securiry.Bearer",
            IssuerSigningKey = JwtSecurityKey.Create("Secret_Key-12345678")
        };

        //Eventos para caso aconteça algum erro:
        option.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
