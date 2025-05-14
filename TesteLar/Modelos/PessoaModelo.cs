namespace Pessoa.Modelos;

public class PessoaModelo
{
    public Guid Id { get; init; } 
    public string Nome { get; private set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public bool Ativo { get; set; }

    public List<TelefoneModelo> Telefones { get; set; } = new(); // Adicionado

    public PessoaModelo() { }
    
    public PessoaModelo(string nome, string cpf, DateTime dataNascimento, bool ativo, List<TelefoneModelo>? telefones = null)
    {
        Id = Guid.NewGuid(); //Gera um novo ID único
        Nome = nome; 
        CPF = cpf;
        DataNascimento = dataNascimento;
        Ativo = ativo;
        Telefones = telefones ?? new List<TelefoneModelo>();
        
    }

    public void Atualizar(string nome, string cpf, DateTime dataNascimento, bool ativo, List<TelefoneModelo>? telefones = null)
    {
        Nome = nome;
        CPF = cpf;
        DataNascimento = dataNascimento;
        Ativo = ativo;
        if (telefones != null)
            Telefones = telefones;
    }

}