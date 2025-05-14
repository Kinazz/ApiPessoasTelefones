namespace Pessoa.Modelos;

public class TelefoneModelo
{
    public Guid Id { get; set; } // Chave primária obrigatória

    public string Numero { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // Ex: "Celular", "Residencial", etc.
    public Guid PessoaId { get; set; }
    public PessoaModelo? Pessoa { get; set; }
}