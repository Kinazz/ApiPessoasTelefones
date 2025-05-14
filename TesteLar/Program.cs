using Pessoa.Data;
using Pessoa.Routes;
using Telefone.Data;
using Telefone.Routes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer(); // Adiciona suporte para endpoints minimal API
builder.Services.AddSwaggerGen();  
builder.Services.AddScoped<PessoaContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();             // Ativa o middleware Swagger
    app.UseSwaggerUI();   
}


app.MapPessoaRoute();
app.MapTelefoneRoute();

app.UseHttpsRedirection();
app.Run();