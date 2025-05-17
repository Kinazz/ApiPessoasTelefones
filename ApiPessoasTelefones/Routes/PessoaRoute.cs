using System.Text;
using Microsoft.EntityFrameworkCore;
using ApiPessoasTelefones.Data;
using ApiPessoasTelefones.Modelos;
using System.Text.Json;

namespace ApiPessoasTelefones.Routes
{
    public static class PessoaRoute
    {
        public static void MapPessoaRoutes(this WebApplication app)
        {
            // Agrupa rotas sob o prefixo /people
            var group = app.MapGroup("/people");

            // Lista todas as pessoas cadastradas
            group.MapGet("/", async (PessoaContext context) =>
            {
                var people = await context.Pessoas.Include(p => p.Telefones).ToListAsync();
                return Results.Ok(people);
            })
            .WithName("ListarPessoas")
            .WithSummary("Lista todas as pessoas cadastradas");

            // Busca uma pessoa pelo ID
            group.MapGet("/{id:guid}", async (Guid id, PessoaContext context) =>
            {
                var person = await context.Pessoas.Include(p => p.Telefones).FirstOrDefaultAsync(p => p.Id == id);
                return person is null ? Results.NotFound(new { message = "Pessoa não encontrada." }) : Results.Ok(person);
            })
            .WithName("BuscarPessoaPorId")
            .WithSummary("Busca uma pessoa pelo seu ID");

            // Cria uma nova pessoa
            group.MapPost("/", async (PessoaModelo person, PessoaContext context) =>
            {
                person.Id = Guid.NewGuid();
                context.Pessoas.Add(person);

                // Log de criação
                context.PessoaAuditLogs.Add(new PessoaAuditLog
                {
                    Id = Guid.NewGuid(),
                    Entidade = "Pessoa",           // Define o tipo de entidade
                    EntidadeId = person.Id,        // Id da pessoa criada
                    PessoaId = person.Id,          // Id da pessoa (igual ao EntidadeId para pessoa)
                    Acao = "Criado",
                    DataHora = DateTime.UtcNow,
                    DadosAntigos = null,
                    DadosNovos = JsonSerializer.Serialize(person)
                });

                await context.SaveChangesAsync();
                return Results.Created($"/people/{person.Id}", person);
            })
            .WithName("CriarPessoa")
            .WithSummary("Cria uma nova pessoa");

            // Atualiza uma pessoa existente
            group.MapPut("/{id:guid}", async (Guid id, PessoaModelo updatedPerson, PessoaContext context) =>
            {
                var person = await context.Pessoas.Include(p => p.Telefones).FirstOrDefaultAsync(p => p.Id == id);
                if (person is null) return Results.NotFound(new { message = "Pessoa não encontrada." });

                // Salva estado antigo para log
                var personAntigo = new PessoaModelo
                {
                    Id = person.Id,
                    Nome = person.Nome,
                    Cpf = person.Cpf,
                    DataNascimento = person.DataNascimento,
                    Ativo = person.Ativo,
                    Telefones = person.Telefones.Select(t => new TelefoneModelo
                    {
                        Id = t.Id,
                        Numero = t.Numero,
                        Tipo = t.Tipo,
                        PessoaId = t.PessoaId
                    }).ToList()
                };

                person.Nome = updatedPerson.Nome;
                person.Cpf = updatedPerson.Cpf;
                person.DataNascimento = updatedPerson.DataNascimento;
                person.Ativo = updatedPerson.Ativo;

                // Log de atualização
                context.PessoaAuditLogs.Add(new PessoaAuditLog
                {
                    Id = Guid.NewGuid(),
                    Entidade = "Pessoa",           // Define o tipo de entidade
                    EntidadeId = person.Id,        // Id da pessoa alterada
                    PessoaId = person.Id,          // Id da pessoa (igual ao EntidadeId para pessoa)
                    Acao = "Atualizado",
                    DataHora = DateTime.UtcNow,
                    DadosAntigos = JsonSerializer.Serialize(personAntigo),
                    DadosNovos = JsonSerializer.Serialize(person)
                });

                await context.SaveChangesAsync();
                return Results.Ok(person);
            })
            .WithName("AtualizarPessoa")
            .WithSummary("Atualiza uma pessoa existente");

            // Remove uma pessoa
            group.MapDelete("/{id:guid}", async (Guid id, PessoaContext context) =>
            {
                var person = await context.Pessoas.Include(p => p.Telefones).FirstOrDefaultAsync(p => p.Id == id);
                if (person is null) return Results.NotFound(new { message = "Pessoa não encontrada." });

                // Log de remoção
                context.PessoaAuditLogs.Add(new PessoaAuditLog
                {
                    Id = Guid.NewGuid(),
                    Entidade = "Pessoa",           // Define o tipo de entidade
                    EntidadeId = person.Id,        // Id da pessoa removida
                    PessoaId = person.Id,          // Id da pessoa (igual ao EntidadeId para pessoa)
                    Acao = "Removido",
                    DataHora = DateTime.UtcNow,
                    DadosAntigos = JsonSerializer.Serialize(person),
                    DadosNovos = null
                });

                context.Pessoas.Remove(person);
                await context.SaveChangesAsync();
                return Results.Ok(new { message = "Pessoa removida com sucesso." });
            })
            .WithName("RemoverPessoa")
            .WithSummary("Remove uma pessoa pelo ID");

            // Busca uma pessoa pelo número do telefone
            group.MapGet("/by-phone/{numero}", async (string numero, PessoaContext context) =>
            {
                var person = await context.Pessoas
                    .Include(p => p.Telefones)
                    .FirstOrDefaultAsync(p => p.Telefones.Any(t => t.Numero == numero));
                if (person is null)
                    return Results.NotFound(new { message = "Pessoa não encontrada para este telefone." });

                
                var dto = new ApiPessoasTelefones.DTOs.PessoaResponseDTO
                {
                    Id = person.Id,
                    Nome = person.Nome,
                    Cpf = person.Cpf,
                    DataNascimento = person.DataNascimento,
                    Ativo = person.Ativo,
                    Telefones = person.Telefones.Select(t => new ApiPessoasTelefones.DTOs.TelefoneResponseDTO
                    {
                        Id = t.Id,
                        Numero = t.Numero,
                        Tipo = t.Tipo
                    }).ToList()
                };

                return Results.Ok(dto);
            })
            .WithName("BuscarPessoaPorTelefone")
            .WithSummary("Busca uma pessoa pelo número do telefone");

            // Exporta todas as pessoas cadastradas em formato CSV
            group.MapGet("/export/csv", async (PessoaContext context) =>
            {
                var people = await context.Pessoas.Include(p => p.Telefones).ToListAsync();
                var sb = new StringBuilder();
                sb.AppendLine("Id,Nome,Cpf,DataNascimento,Ativo,Telefones");

                foreach (var p in people)
                {
                    var telefones = string.Join(" | ", p.Telefones.Select(t => $"{t.Numero} ({t.Tipo})"));
                    sb.AppendLine($"{p.Id},{p.Nome},{p.Cpf},{p.DataNascimento:yyyy-MM-dd},{p.Ativo},{telefones}");
                }

                var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                return Results.File(bytes, "text/csv", "pessoas.csv");
            })
            .WithName("ExportarPessoasCsv")
            .WithSummary("Exporta todas as pessoas cadastradas em formato CSV");

            // Busca pessoas pelo nome 
            group.MapGet("/search/by-name/{nome}", async (string nome, PessoaContext context) =>
            {
                var pessoas = await context.Pessoas
                    .Include(p => p.Telefones)
                    .Where(p => EF.Functions.Like(p.Nome.ToLower(), $"%{nome.ToLower()}%"))
                    .ToListAsync();

                if (pessoas.Count == 0)
                    return Results.NotFound(new { message = "Nenhuma pessoa encontrada com esse nome." });

                
                var dtos = pessoas.Select(person => new ApiPessoasTelefones.DTOs.PessoaResponseDTO
                {
                    Id = person.Id,
                    Nome = person.Nome,
                    Cpf = person.Cpf,
                    DataNascimento = person.DataNascimento,
                    Ativo = person.Ativo,
                    Telefones = person.Telefones.Select(t => new ApiPessoasTelefones.DTOs.TelefoneResponseDTO
                    {
                        Id = t.Id,
                        Numero = t.Numero,
                        Tipo = t.Tipo
                    }).ToList()
                }).ToList();

                return Results.Ok(dtos);
            })
            .WithName("BuscarPessoaPorNome")
            .WithSummary("Busca pessoas pelo nome (busca parcial, não sensível a maiúsculas/minúsculas)");
        }
    }
}
