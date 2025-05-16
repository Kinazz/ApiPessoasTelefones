namespace ApiPessoasTelefones.DTOs;

// DTO para resposta de pessoa
public class PessoaResponseDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public bool Ativo { get; set; }
    public List<TelefoneResponseDTO> Telefones { get; set; } = new();
}

// DTO para resposta de telefone
public class TelefoneResponseDTO
{
    public Guid Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
}