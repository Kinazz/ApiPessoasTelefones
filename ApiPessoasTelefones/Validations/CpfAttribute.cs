using System.ComponentModel.DataAnnotations;

namespace ApiPessoasTelefones.Validations;

// Validador customizado para CPF
public class CpfAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        // Verifica se o valor é uma string
        if (value is not string cpf) return false;

        // Remove pontos e traços do CPF
        cpf = cpf.Trim().Replace(".", "").Replace("-", "");

        // Verifica se o CPF tem 11 dígitos numéricos
        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            return false;

        // Lista de CPFs inválidos conhecidos (todos os dígitos iguais)
        var invalids = new[]
        {
            "00000000000", "11111111111", "22222222222", "33333333333",
            "44444444444", "55555555555", "66666666666", "77777777777",
            "88888888888", "99999999999"
        };
        if (invalids.Contains(cpf)) return false;

        // Multiplicadores para cálculo dos dígitos verificadores
        int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        // Calcula o primeiro dígito verificador
        string tempCpf = cpf.Substring(0, 9);
        int sum = 0;
        for (int i = 0; i < 9; i++)
            sum += int.Parse(tempCpf[i].ToString()) * mult1[i];

        int rest = sum % 11;
        if (rest < 2)
            rest = 0;
        else
            rest = 11 - rest;

        string digit = rest.ToString();
        tempCpf += digit;

        // Calcula o segundo dígito verificador
        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += int.Parse(tempCpf[i].ToString()) * mult2[i];

        rest = sum % 11;
        if (rest < 2)
            rest = 0;
        else
            rest = 11 - rest;

        digit += rest.ToString();

        // Verifica se os dígitos calculados coincidem com os do CPF informado
        return cpf.EndsWith(digit);
    }
}