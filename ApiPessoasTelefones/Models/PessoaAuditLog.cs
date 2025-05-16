namespace ApiPessoasTelefones.Modelos;

public class PessoaAuditLog
{
    public Guid Id { get; set; }
    public string Entidade { get; set; } = string.Empty; // "Pessoa" ou "Telefone"
    public Guid EntidadeId { get; set; } // Id da pessoa ou telefone
    public Guid? PessoaId { get; set; } 
    public string Acao { get; set; } = string.Empty; // Criado, Atualizado, Removido
    public DateTime DataHora { get; set; }
    public string? DadosAntigos { get; set; }
    public string? DadosNovos { get; set; }
}

