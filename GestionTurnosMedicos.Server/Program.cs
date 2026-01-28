using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// 🔗 Conexión a PostgreSQL (relacional)
builder.Services.AddDbContext<TurnosContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// 🔗 Conexión a MongoDB (documental)
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});
builder.Services.AddScoped<IMongoTurnoRepository, MongoTurnoRepository>();
builder.Services.AddScoped<TurnoSyncService>();

// 🔐 CORS para React en localhost:5173
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.WithOrigins(
                "http://localhost:5173",
                "https://localhost:5173",
                "http://localhost:52043",
                "https://localhost:52043")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

// 📦 Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Turnos Médicos API",
        Version = "v1"
    });
});

var app = builder.Build();

// 🧪 Documentación Swagger (solo en desarrollo)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔐 Redirección a HTTPS
app.UseHttpsRedirection();

// 🌐 Habilitar CORS
app.UseCors("AllowReactApp");

// 🔐 Autorización (si tienes autenticación después)
app.UseAuthorization();

app.MapControllers();

app.Run();
