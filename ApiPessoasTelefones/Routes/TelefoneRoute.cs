using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ApiPessoasTelefones.Data;
using ApiPessoasTelefones.DTOs;
using ApiPessoasTelefones.Modelos;

namespace ApiPessoasTelefones.Routes
{
    public static class TelefoneRoute
    {
        public static void MapTelefoneRoutes(this WebApplication app)
        {
            // Agrupa rotas relacionadas a telefones
            var group = app.MapGroup("/phones");

            // Lista todos os telefones cadastrados
            group.MapGet("/", async (PessoaContext context) =>
            {
                var phones = await context.Telefones.ToListAsync();
                return Results.Ok(phones);
            })
            .WithName("ListarTelefones")
            .WithSummary("Lista todos os telefones cadastrados");

            // Adiciona um telefone para uma pessoa específica
            group.MapPost("/{personId:guid}", async (
                Guid personId,
                TelefoneResponseDTO dto,
                PessoaContext context) =>
            {
                var personExists = await context.Pessoas.AnyAsync(p => p.Id == personId);
                if (!personExists)
                    return Results.NotFound(new { message = "Pessoa não encontrada." });

                var phone = new TelefoneModelo
                {
                    Id = Guid.NewGuid(),
                    Numero = dto.Numero,
                    Tipo = dto.Tipo,
                    PessoaId = personId
                };

                context.Telefones.Add(phone);

                // Log de criação de telefone
                context.PessoaAuditLogs.Add(new PessoaAuditLog
                {
                    Id = Guid.NewGuid(),
                    Entidade = "Telefone",
                    EntidadeId = phone.Id,
                    PessoaId = personId,
                    Acao = "Criado",
                    DataHora = DateTime.UtcNow,
                    DadosAntigos = null,
                    DadosNovos = JsonSerializer.Serialize(phone)
                });

                await context.SaveChangesAsync();

                return Results.Created($"/phones/{phone.Id}", phone);
            })
            .WithName("AdicionarTelefone")
            .WithSummary("Adiciona um telefone para uma pessoa específica");

            // Edita um telefone existente
            group.MapPut("/{phoneId:guid}", async (Guid phoneId, TelefoneResponseDTO dto, PessoaContext context) =>
            {
                var phone = await context.Telefones.FindAsync(phoneId);
                if (phone is null) return Results.NotFound(new { message = "Telefone não encontrado." });

                var phoneAntigo = new TelefoneModelo
                {
                    Id = phone.Id,
                    Numero = phone.Numero,
                    Tipo = phone.Tipo,
                    PessoaId = phone.PessoaId
                };

                phone.Numero = dto.Numero;
                phone.Tipo = dto.Tipo;

                // Log de atualização de telefone
                context.PessoaAuditLogs.Add(new PessoaAuditLog
                {
                    Id = Guid.NewGuid(),
                    Entidade = "Telefone",
                    EntidadeId = phone.Id,
                    PessoaId = phone.PessoaId,
                    Acao = "Atualizado",
                    DataHora = DateTime.UtcNow,
                    DadosAntigos = JsonSerializer.Serialize(phoneAntigo),
                    DadosNovos = JsonSerializer.Serialize(phone)
                });

                await context.SaveChangesAsync();
                return Results.Ok(phone);
            })
            .WithName("EditarTelefone")
            .WithSummary("Edita um telefone existente");

            // Remove um telefone
            group.MapDelete("/{phoneId:guid}", async (Guid phoneId, PessoaContext context) =>
            {
                var phone = await context.Telefones.FindAsync(phoneId);
                if (phone is null) return Results.NotFound(new { message = "Telefone não encontrado." });

                // Log de remoção de telefone
                context.PessoaAuditLogs.Add(new PessoaAuditLog
                {
                    Id = Guid.NewGuid(),
                    Entidade = "Telefone",
                    EntidadeId = phone.Id,
                    PessoaId = phone.PessoaId,
                    Acao = "Removido",
                    DataHora = DateTime.UtcNow,
                    DadosAntigos = JsonSerializer.Serialize(phone),
                    DadosNovos = null
                });

                context.Telefones.Remove(phone);
                await context.SaveChangesAsync();
                return Results.Ok(new { message = "Telefone removido com sucesso." });
            })
            .WithName("RemoverTelefone")
            .WithSummary("Remove um telefone pelo ID");
        }
    }
}