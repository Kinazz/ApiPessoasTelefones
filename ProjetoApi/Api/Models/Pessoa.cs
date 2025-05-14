namespace Api.Models;

public class Pessoa
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public List<Telefone> Telefones { get; set; } = new();
}