using Desafio_Tecnico_Cadastro_de_Beneficiarios.Services;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Data;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Profiles;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Services.Interface;
using DotNetEnv;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Carrega variáveis do .env 
Env.Load();

// Lê variáveis do ambiente
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPass = Environment.GetEnvironmentVariable("DB_PASS");

// Monta a connection string
var connectionString = $"Server={dbHost};Database={dbName};User Id={dbUser};Password={dbPass};TrustServerCertificate=True;";

// Configuração do DbContext 
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Evita loops em relacionamentos (ex: Beneficiário -> Plano -> Beneficiário) para resolver problema do Datetime da criação
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

// Configuração do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Desafio Técnico - Cadastro de Beneficiários API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Gustavo",
            Email = "gustavojesus79@gmail.com"
        }
    });

    // XML de documentação para o Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Injeção de dependências entre interfaces e serviços
builder.Services.AddScoped<IPlanoInterface, PlanoService>();
builder.Services.AddScoped<IBeneficiarioInterface, BeneficiarioService>();

// AutoMapper 
builder.Services.AddAutoMapper(typeof(PlanoProfile).Assembly);

var app = builder.Build();

// Aplica migrations automaticamente
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();  // Corrigido para AppDbContext
    dbContext.Database.Migrate();  // Aplica as migrations automaticamente
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS redirection e Authorization
app.UseHttpsRedirection();
app.UseAuthorization();

// Controllers
app.MapControllers();

app.Run();
