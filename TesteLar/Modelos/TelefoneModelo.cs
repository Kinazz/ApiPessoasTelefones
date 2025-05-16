using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pessoa.Modelos;

public class TelefoneModelo
{
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Numero { get; set; } = string.Empty;

        [Required]
        public string Tipo { get; set; } = string.Empty;

        public Guid PessoaId { get; set; }

        [JsonIgnore]
        public PessoaModelo? Pessoa { get; set; }
}