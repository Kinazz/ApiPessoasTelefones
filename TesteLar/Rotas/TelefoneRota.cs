using Microsoft.EntityFrameworkCore;
using Pessoa.Data;
using Pessoa.Modelos;
using Telefone.Data;



namespace Telefone.Routes;

    public static class TelefoneRoute
    {
        public static void MapTelefoneRoute(this WebApplication app)
        {
            var rota = app.MapGroup("/Telefone");
            rota.MapGet("/Listar Telefone", async (PessoaContext context) =>
            {
                var telefone = await context.Telefones.ToListAsync();
                return Results.Ok(telefone);
            });

rota.MapPost("/{pessoaId:guid}/AdicionarTelefone", async (Guid pessoaId, TelefoneModelo telefone, PessoaContext context) =>
{
    var pessoa = await context.Pessoas.Include(p => p.Telefones).FirstOrDefaultAsync(p => p.Id == pessoaId);
    if (pessoa is null)
        return Results.NotFound("Pessoa não encontrada.");

    telefone.Id = Guid.NewGuid();
    telefone.PessoaId = pessoaId;
    pessoa.Telefones.Add(telefone);

    await context.SaveChangesAsync();
    return Results.Ok(telefone);
});

rota.MapPut("/EditarTelefone/{telefoneId:guid}", async (Guid telefoneId, TelefoneModelo telefoneEditado, PessoaContext context) =>
{
    var telefone = await context.Telefones.FindAsync(telefoneId);
    if (telefone is null)
        return Results.NotFound("Telefone não encontrado.");

    telefone.Numero = telefoneEditado.Numero;
    telefone.Tipo = telefoneEditado.Tipo;

    await context.SaveChangesAsync();
    return Results.Ok(telefone);
});

rota.MapDelete("/DeletarTelefone/{telefoneId:guid}", async (Guid telefoneId, PessoaContext context) =>
{
    var telefone = await context.Telefones.FindAsync(telefoneId);
    if (telefone is null)
        return Results.NotFound("Telefone não encontrado.");

    context.Telefones.Remove(telefone);
    await context.SaveChangesAsync();
    return Results.Ok("Telefone removido com sucesso.");
});
        }}