using Microsoft.EntityFrameworkCore;
using ApiPessoasTelefones.Data;
using ApiPessoasTelefones.Routes;

var builder = WebApplication.CreateBuilder(args);

// Configura conexão com SQLite
builder.Services.AddDbContext<PessoaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Middleware de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapeia rotas da API
app.MapPessoaRoutes();
app.MapTelefoneRoutes();
app.MapPessoaAuditLogRoutes();

// Cria o banco de dados e aplica migrações pendentes
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PessoaContext>();
    db.Database.Migrate();
}

app.Run();
