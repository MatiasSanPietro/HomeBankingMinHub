using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TransactionService.Models;
// todavia no cambio el front para recibir mis endpoints, applicationUrl en http properties

var MyAllowOriginsSpecifics = "_myAllowOriginsSpecifics";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowOriginsSpecifics,
        policy =>
        {
            policy.WithOrigins("http://localhost:5077", "http://localhost:5109") //puedo agregar mas origenes
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

// Add DbContext to the container
builder.Services.AddDbContext<TransactionServiceContext>(options => 
options.UseMySQL(builder.Configuration.GetConnectionString("ConexionDatabase")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        options.LoginPath = new PathString("/index.html");
    });

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ClientOnly", policy => policy.RequireClaim("Client"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowOriginsSpecifics);

app.UseAuthorization();

app.MapControllers();

app.Run();
