using System.ComponentModel.DataAnnotations;

namespace Pessoa.Modelos;

public class PessoaModelo
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public string Cpf { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }

    public bool Ativo { get; set; }

    public List<TelefoneModelo> Telefones { get; set; } = new();
}