namespace Pessoa.Modelos;

public record PessoaRequest(string Nome, string CPF, DateTime DataNascimento, bool Ativo);

