using Microsoft.EntityFrameworkCore;
using Pessoa.Data;
using Pessoa.DTOs;
using Pessoa.Modelos;

namespace Pessoa.Routes
{
    public static class TelefoneRoute
    {
        public static void MapTelefoneRota(this WebApplication app)
        {
            var rota = app.MapGroup("/Telefone");

            rota.MapGet("/ListarTelefones", async (PessoaContext context) =>
            {
                var telefones = await context.Telefones.ToListAsync();
                return Results.Ok(telefones);
            });

            rota.MapPost("/{pessoaId:guid}/AdicionarTelefone", async (
                Guid pessoaId,
                TelefoneDTO dto,
                PessoaContext context) =>
            {
                var pessoaExiste = await context.Pessoas.AnyAsync(p => p.Id == pessoaId);
                if (!pessoaExiste)
                    return Results.NotFound("Pessoa não encontrada.");

                var telefone = new TelefoneModelo
                {
                    Id = Guid.NewGuid(),
                    Numero = dto.Numero,
                    Tipo = dto.Tipo,
                    PessoaId = pessoaId
                };

                context.Telefones.Add(telefone);
                await context.SaveChangesAsync();

                return Results.Created($"/Telefone/{telefone.Id}", telefone);
            });

            rota.MapPut("/EditarTelefone/{telefoneId:guid}", async (Guid telefoneId, TelefoneDTO dto, PessoaContext context) =>
            {
                var telefone = await context.Telefones.FindAsync(telefoneId);
                if (telefone is null) return Results.NotFound("Telefone não encontrado.");

                telefone.Numero = dto.Numero;
                telefone.Tipo = dto.Tipo;

                await context.SaveChangesAsync();
                return Results.Ok(telefone);
            });

            rota.MapDelete("/DeletarTelefone/{telefoneId:guid}", async (Guid telefoneId, PessoaContext context) =>
            {
                var telefone = await context.Telefones.FindAsync(telefoneId);
                if (telefone is null) return Results.NotFound("Telefone não encontrado.");

                context.Telefones.Remove(telefone);
                await context.SaveChangesAsync();
                return Results.Ok("Telefone removido com sucesso.");
            });
        }
    }
}