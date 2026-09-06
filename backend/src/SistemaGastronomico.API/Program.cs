using Microsoft.OpenApi.Models;
using SistemaGastronomico.API.Middlewares;
using SistemaGastronomico.Application;
using SistemaGastronomico.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (Clean Architecture layers)
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Exception handling and problem details
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails();

// CORS for Blazor WASM and other clients
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

// Configuración exclusiva de Swagger con Swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema Gastronómico UTA - Web API",
        Version = "v1",
        Description = "API REST de la Plataforma de Recomendación y Descubrimiento de Locales de Comida Basada en Geolocalización para la Comunidad de la UTA."
    });

    // Configuración para autorizar con JWT en Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT usando el esquema Bearer. \r\n\r\n Ingrese 'Bearer' [espacio] y luego su token en el campo.\r\n\r\nEjemplo: \"Bearer eyJhbGciOi...\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseExceptionHandler();

// Enable Swagger and classic Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema Gastronómico UTA API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
