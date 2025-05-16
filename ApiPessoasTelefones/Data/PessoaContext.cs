using Microsoft.EntityFrameworkCore;
using ApiPessoasTelefones.Modelos;

namespace ApiPessoasTelefones.Data
{
    // Contexto do banco de dados para pessoas e telefones
    public class PessoaContext : DbContext
    {
        public PessoaContext(DbContextOptions<PessoaContext> options) : base(options) { }

        // Tabela de pessoas
        public DbSet<PessoaModelo> Pessoas { get; set; }

        // Tabela de telefones
        public DbSet<TelefoneModelo> Telefones { get; set; }

        // Configuração do relacionamento entre Pessoa e Telefone
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PessoaModelo>()
                .HasMany(p => p.Telefones)
                .WithOne(t => t.Pessoa)
                .HasForeignKey(t => t.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        // Tabela de logs de auditoria
        public DbSet<PessoaAuditLog> PessoaAuditLogs { get; set; }
    }
}

