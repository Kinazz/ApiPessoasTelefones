using System.Text;
using Microsoft.EntityFrameworkCore;
using ApiPessoasTelefones.Data;
using ApiPessoasTelefones.Modelos;

namespace ApiPessoasTelefones.Routes
{
    public static class PessoaAuditLogRoute
    {
        public static void MapPessoaAuditLogRoutes(this WebApplication app)
        {
            // Agrupa rotas sob o prefixo /people/logs
            var group = app.MapGroup("/people/logs");

            // Lista todos os logs de auditoria de pessoas
            group.MapGet("/", async (PessoaContext context) =>
            {
                var logs = await context.PessoaAuditLogs
                    .OrderByDescending(l => l.DataHora)
                    .ToListAsync();
                return Results.Ok(logs);
            })
            .WithName("ListarLogsAuditoria")
            .WithSummary("Lista todos os logs de auditoria de pessoas");

            // Exporta os logs de auditoria em formato CSV
            group.MapGet("/export/csv", async (PessoaContext context) =>
            {
                var logs = await context.PessoaAuditLogs
                    .OrderByDescending(l => l.DataHora)
                    .ToListAsync();

                var sb = new StringBuilder();
                sb.AppendLine("Id,PessoaId,Acao,DataHora,DadosAntigos,DadosNovos");

                foreach (var log in logs)
                {
                    
                    sb.AppendLine($"\"{log.Id}\",\"{log.PessoaId}\",\"{log.Acao}\",\"{log.DataHora:yyyy-MM-dd HH:mm:ss}\",\"{log.DadosAntigos}\",\"{log.DadosNovos}\"");
                }

                var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                return Results.File(bytes, "text/csv", "logs_auditoria.csv");
            })
            .WithName("ExportarLogsAuditoriaCsv")
            .WithSummary("Exporta todos os logs de auditoria de pessoas em formato CSV");
        }
    }
}