using System.ComponentModel.DataAnnotations;
using ApiPessoasTelefones.Validations;

namespace ApiPessoasTelefones.Modelos;

// Modelo de pessoa utilizado na aplicação
public class PessoaModelo
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 dígitos.")]
    [Cpf(ErrorMessage = "CPF inválido.")]
    public string Cpf { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }

    public bool Ativo { get; set; }

    // Lista de telefones associados à pessoa
    public List<TelefoneModelo> Telefones { get; set; } = new();
}