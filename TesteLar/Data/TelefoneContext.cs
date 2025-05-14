using Microsoft.EntityFrameworkCore;
using Pessoa.Modelos;


namespace Telefone.Data;

public class TelefoneContext : DbContext
{

   public  DbSet<TelefoneModelo> Telefones { get; set; }   
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PessoaModelo>()
            .HasMany(p => p.Telefones)
            .WithOne(t => t.Pessoa)
            .HasForeignKey(t => t.PessoaId);

        base.OnModelCreating(modelBuilder);
    }
}