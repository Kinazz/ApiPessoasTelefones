using Microsoft.EntityFrameworkCore;
using Pessoa.Data;
using Pessoa.Modelos;

namespace Pessoa.Routes
{
    public static class PessoaRoute
    {
        public static void MapPessoaRota(this WebApplication app)
        {
            var rota = app.MapGroup("/Pessoa");

            rota.MapGet("/ListarPessoas", async (PessoaContext context) =>
            {
                var pessoas = await context.Pessoas.Include(p => p.Telefones).ToListAsync();
                return Results.Ok(pessoas);
            });

            rota.MapGet("/Pesquisar por/{id:guid}", async (Guid id, PessoaContext context) =>
            {
                var pessoa = await context.Pessoas.Include(p => p.Telefones).FirstOrDefaultAsync(p => p.Id == id);
                return pessoa is null ? Results.NotFound("Pessoa não encontrada.") : Results.Ok(pessoa);
            });

            rota.MapPost("/CriarPessoa", async (PessoaModelo pessoa, PessoaContext context) =>
            {
                pessoa.Id = Guid.NewGuid();
                context.Pessoas.Add(pessoa);
                await context.SaveChangesAsync();
                return Results.Created($"/Pessoa/{pessoa.Id}", pessoa);
            });

            rota.MapPut("/EditarPessoa/{id:guid}", async (Guid id, PessoaModelo pessoaEditada, PessoaContext context) =>
            {
                var pessoa = await context.Pessoas.FindAsync(id);
                if (pessoa is null) return Results.NotFound("Pessoa não encontrada.");

                pessoa.Nome = pessoaEditada.Nome;
                pessoa.Cpf = pessoaEditada.Cpf;
                pessoa.DataNascimento = pessoaEditada.DataNascimento;
                pessoa.Ativo = pessoaEditada.Ativo;

                await context.SaveChangesAsync();
                return Results.Ok(pessoa);
            });

            rota.MapDelete("/DeletarPessoa/{id:guid}", async (Guid id, PessoaContext context) =>
            {
                var pessoa = await context.Pessoas.FindAsync(id);
                if (pessoa is null) return Results.NotFound("Pessoa não encontrada.");

                context.Pessoas.Remove(pessoa);
                await context.SaveChangesAsync();
                return Results.Ok("Pessoa removida com sucesso.");
            });
        }
    }
}
