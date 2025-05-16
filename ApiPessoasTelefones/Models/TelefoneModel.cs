using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiPessoasTelefones.Modelos;

// Modelo de telefone associado a uma pessoa
public class TelefoneModelo
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Numero { get; set; } = string.Empty;

    [Required]
    public string Tipo { get; set; } = string.Empty;

    // Chave estrangeira para Pessoa
    public Guid PessoaId { get; set; }

    // Evita loop na serialização JSON
    [JsonIgnore]
    public PessoaModelo? Pessoa { get; set; }
}