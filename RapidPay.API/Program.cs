using Microsoft.EntityFrameworkCore;
using RapidPay.DAL.Data;
using RapidPay.DAL.Interfaces;
using RapidPay.DAL.Repositories;
using RapidPay.Services.Interfaces;
using RapidPay.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Using this configuration I'm sure that db context is created only once per request
// and ensures thread safety
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RapidPayDB")));

// Add services here...
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();


builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable middleware for Swagger
    app.UseSwaggerUI(); // Enable Swagger UI
}

app.UseHttpsRedirection(); // Redirect to HTTPS

app.UseAuthorization(); // Authorization middleware

app.MapControllers(); // Map API controllers

app.Run();
