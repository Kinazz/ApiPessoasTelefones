using Microsoft.EntityFrameworkCore;
using Pessoa.Data;
using Pessoa.Modelos;


namespace Pessoa.Routes;

    public static class PessoaRoute
    {
        public static void MapPessoaRoute(this WebApplication app)
        {
            var rota = app.MapGroup("/Pessoa");

            rota.MapPost("/Adicionar Nova Pessoa", async (PessoaRequest req, PessoaContext context) =>
            {
                var pessoa = new PessoaModelo(req.Nome, req.CPF, req.DataNascimento, req.Ativo);
                await context.AddAsync(pessoa);
                await context.SaveChangesAsync();
            }); 

            rota.MapGet("/Listar Pessoas", async (PessoaContext context) =>
            {
                var pessoa = await context.Pessoas.ToListAsync();
                return Results.Ok(pessoa);
            });

            rota.MapPut("/Atualizar Pessoa/{id:guid}", async (Guid id, PessoaRequest req, PessoaContext context) =>
            {
                var pessoa = await context.Pessoas.FindAsync(id);
                if (pessoa is null)
                    return Results.NotFound();

                pessoa.Atualizar(req.Nome, req.CPF, req.DataNascimento, req.Ativo);

                await context.SaveChangesAsync();
                return Results.Ok(pessoa);
            });

            rota.MapDelete("/Deletar Pessoa/{id:guid}", async (Guid id, PessoaContext context) =>
            {
                var pessoa = await context.Pessoas.FindAsync(id);
                if (pessoa is null)
                    return Results.NotFound();

                context.Remove(pessoa);
                await context.SaveChangesAsync();
                return Results.Ok(pessoa);
            });

            rota.MapPatch("/Desativar Pessoa/{id:guid}", async (Guid id, PessoaContext context) =>
            {
                var pessoa = await context.Pessoas.FindAsync(id);
                if (pessoa is null)
                    return Results.NotFound();

                pessoa.Ativo = !pessoa.Ativo;
                await context.SaveChangesAsync();
                return Results.Ok(pessoa);
            });
 

        }
    }