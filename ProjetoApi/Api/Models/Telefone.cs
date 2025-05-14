namespace Api.Models;

public class Telefone
{
    public int Id { get; set; }
    public string? Numero { get; set; }
    public int PessoaId { get; set; } // Chave estrangeira
    public Pessoa? Pessoa { get; set; } // Navegação
}