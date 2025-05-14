using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("BancoEmMemoria"));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();  

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();             // Ativa o middleware Swagger
    app.UseSwaggerUI(); 
}

app.MapPost("/Pessoas", async (AppDbContext db, Pessoa pessoa) =>
{
    db.Pessoas.Add(pessoa);
    await db.SaveChangesAsync();
    return Results.Created($"/pessoas/{pessoa.Id}", pessoa);
});

app.MapGet("/telefones", async (AppDbContext db) =>
{
    return await db.Telefones.ToListAsync();
});

app.UseHttpsRedirection();
app.Run();


