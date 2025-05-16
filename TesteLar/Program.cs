using Microsoft.EntityFrameworkCore;
using Pessoa.Data;
using Pessoa.Routes;

var builder = WebApplication.CreateBuilder(args);

// Configurar conexão com SQLite (ajuste se usar outro banco)
builder.Services.AddDbContext<PessoaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionar Swagger e outros serviços
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapear rotas
app.MapPessoaRota();
app.MapTelefoneRota();

app.Run();
