using Microsoft.EntityFrameworkCore;
using RapidPay.DAL.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

// Using this configuration I'm sure that db context is created only once per request
// and ensures thread safety
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RapidPayDB")));

//Add services here...


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Habilita el middleware para Swagger
    app.UseSwaggerUI(); // Habilita la interfaz de usuario de Swagger
}

app.UseHttpsRedirection(); // Redirección a HTTPS

app.UseAuthorization(); // Middleware de autorización

app.MapControllers(); // Mapea los controladores de la API

app.Run();
