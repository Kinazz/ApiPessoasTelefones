using Microsoft.EntityFrameworkCore;
using Pessoa.Modelos;


namespace Pessoa.Data;

public class PessoaContext : DbContext
{
   public  DbSet<PessoaModelo> Pessoas { get; set; }   
   public  DbSet<TelefoneModelo> Telefones { get; set; }   



   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       optionsBuilder.UseSqlite("Data Source=Pessoa.sqlite");
       base.OnConfiguring(optionsBuilder);
    }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PessoaModelo>()
            .HasMany(p => p.Telefones)
            .WithOne(t => t.Pessoa)
            .HasForeignKey(t => t.PessoaId);

        base.OnModelCreating(modelBuilder);
    }
}