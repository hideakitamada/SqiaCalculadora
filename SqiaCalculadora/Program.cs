using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using SqiaCalculadora.Repositories;
using SqiaCalculadora.Services;
using SqiaCalculadora.Utils;
using SqiaCalculadora.Validators;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using SqiaCalculadora.Settings;
using SqiaCalculadora.Models;
using Microsoft.Extensions.Options;
using SqiaCalculadora.Data;

var builder = WebApplication.CreateBuilder(args);
var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

builder.Services.AddControllers().AddFluentValidation(fv =>
    fv.RegisterValidatorsFromAssemblyContaining<InvestimentoRequestValidator>());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("calculadora"));

builder.Services.AddScoped<IInvestimentoService, InvestimentoService>();
builder.Services.AddScoped<ICotacaoRepository, CotacaoRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<RetryPolicyOptions>(builder.Configuration.GetSection("RetryPolicy"));
builder.Services.Configure<PollySettings>(builder.Configuration.GetSection("PollyPolicies"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Calculadora SQIA",
        Description = "Teste Técnico",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Hugo Tamada",
            Email = "hideakitamada@gmail.com"
        }
    });
});

builder.Services.AddHttpClient("ResilientClient")
    .AddPolicyHandler((sp, _) =>
    {
        var settings = sp.GetRequiredService<IOptions<PollySettings>>().Value;
        return PollyPolicyExtensions.GetResiliencePolicy(settings);
    });

var app = builder.Build();

//Inicializa o banco de dados com dados de exemplo
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    db.Cotacoes.AddRange(
        new Cotacao { Id =   1, Data = new DateTime(2025, 1,  1, 0, 0, 0), Indexador = "SQI", Valor = 10.50M },
        new Cotacao { Id =   2, Data = new DateTime(2025, 1,  2, 0, 0, 0), Indexador = "SQI", Valor = 10.50M },
        new Cotacao { Id =   3, Data = new DateTime(2025, 1,  3, 0, 0, 0), Indexador = "SQI", Valor = 10.50M },
        new Cotacao { Id =   6, Data = new DateTime(2025, 1,  6, 0, 0, 0), Indexador = "SQI", Valor = 12.25M },
        new Cotacao { Id =   7, Data = new DateTime(2025, 1,  7, 0, 0, 0), Indexador = "SQI", Valor = 12.25M },
        new Cotacao { Id =   8, Data = new DateTime(2025, 1,  8, 0, 0, 0), Indexador = "SQI", Valor = 12.25M },
        new Cotacao { Id =   9, Data = new DateTime(2025, 1,  9, 0, 0, 0), Indexador = "SQI", Valor = 12.25M },
        new Cotacao { Id =  10, Data = new DateTime(2025, 1, 10, 0, 0, 0), Indexador = "SQI", Valor = 12.25M },
        new Cotacao { Id =  13, Data = new DateTime(2025, 1, 13, 0, 0, 0), Indexador = "SQI", Valor = 12.25M },
        new Cotacao { Id =  14, Data = new DateTime(2025, 1, 14, 0, 0, 0), Indexador = "SQI", Valor = 12.25M },
        new Cotacao { Id =  15, Data = new DateTime(2025, 1, 15, 0, 0, 0), Indexador = "SQI", Valor = 12.25M },
        new Cotacao { Id =  16, Data = new DateTime(2025, 1, 16, 0, 0, 0), Indexador = "SQI", Valor =  9.00M },
        new Cotacao { Id =  17, Data = new DateTime(2025, 1, 17, 0, 0, 0), Indexador = "SQI", Valor =  9.00M },
        new Cotacao { Id =  20, Data = new DateTime(2025, 1, 20, 0, 0, 0), Indexador = "SQI", Valor =  9.00M },
        new Cotacao { Id =  21, Data = new DateTime(2025, 1, 21, 0, 0, 0), Indexador = "SQI", Valor =  7.75M },
        new Cotacao { Id =  22, Data = new DateTime(2025, 1, 22, 0, 0, 0), Indexador = "SQI", Valor =  7.75M },
        new Cotacao { Id =  23, Data = new DateTime(2025, 1, 23, 0, 0, 0), Indexador = "SQI", Valor =  7.75M },
        new Cotacao { Id =  24, Data = new DateTime(2025, 1, 24, 0, 0, 0), Indexador = "SQI", Valor =  7.75M },
        new Cotacao { Id =  27, Data = new DateTime(2025, 1, 27, 0, 0, 0), Indexador = "SQI", Valor =  8.25M },
        new Cotacao { Id =  28, Data = new DateTime(2025, 1, 28, 0, 0, 0), Indexador = "SQI", Valor =  8.25M },
        new Cotacao { Id =  29, Data = new DateTime(2025, 1, 29, 0, 0, 0), Indexador = "SQI", Valor =  8.25M },
        new Cotacao { Id =  30, Data = new DateTime(2025, 1, 30, 0, 0, 0), Indexador = "SQI", Valor =  8.25M },
        new Cotacao { Id =  31, Data = new DateTime(2025, 1, 31, 0, 0, 0), Indexador = "SQI", Valor =  8.25M },
        new Cotacao { Id = 313, Data = new DateTime(2025, 3, 13, 0, 0, 0), Indexador = "SQI", Valor = 12.00M },
        new Cotacao { Id = 314, Data = new DateTime(2025, 3, 14, 0, 0, 0), Indexador = "SQI", Valor = 12.50M },
        new Cotacao { Id = 317, Data = new DateTime(2025, 3, 17, 0, 0, 0), Indexador = "SQI", Valor = 11.00M },
        new Cotacao { Id = 318, Data = new DateTime(2025, 3, 18, 0, 0, 0), Indexador = "SQI", Valor = 12.20M },
        new Cotacao { Id = 319, Data = new DateTime(2025, 3, 19, 0, 0, 0), Indexador = "SQI", Valor = 13.00M },
        new Cotacao { Id = 320, Data = new DateTime(2025, 3, 20, 0, 0, 0), Indexador = "SQI", Valor = 12.40M },
        new Cotacao { Id = 321, Data = new DateTime(2025, 3, 21, 0, 0, 0), Indexador = "SQI", Valor = 12.70M }
    );

    db.SaveChanges();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();