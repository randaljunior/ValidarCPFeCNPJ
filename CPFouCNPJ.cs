using MyExtensions;

namespace ValidarCPFeCNPJ;

public class CPFouCNPJ
{
    private IDocumento? _valor;

    private CPFouCNPJ() { }

    /// <summary>
    /// Construtor que recebe um CPF.
    /// </summary>
    /// <param name="cpf"></param>
    public CPFouCNPJ(CPF cpf)
    {
        _valor = cpf;
    }

    /// <summary>
    /// Construtor que recebe um CNPJ.
    /// </summary>
    /// <param name="cnpj"></param>
    public CPFouCNPJ(CNPJ cnpj)
    {
        _valor = cnpj;
    }

    /// <summary>
    /// Número do CPF ou CNPJ.
    /// </summary>
    public ulong Valor => _valor?.Numero ?? 0;

    /// <summary>
    /// Retorna verdadeiro se o valor armazeando é um CPF.
    /// </summary>
    public bool IsCPF => _valor is CPF;

    /// <summary>
    /// Retorna verdadeiro se o valor armazeando é um CNPJ.
    /// </summary>
    public bool IsCNPJ => _valor is CNPJ;

    /// <summary>
    /// Retorna o tipo do documento (CPF ou CNPJ).
    /// </summary>
    public Type? TipoDocumento => _valor?.GetType();

    /// <summary>
    /// Cria um CPFouCNPJ a partir de uma numero de CPF ou CNPJ.
    /// </summary>
    /// <param name="numero"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static CPFouCNPJ Create(ulong numero)
    {
        if (ValidaCPF.IsCPF(numero))
        {
            return new CPFouCNPJ { _valor = new CPF(numero) };
        }
        else if (ValidaCNPJ.IsCNPJ(numero))
        {
            return new CPFouCNPJ { _valor = new CNPJ(numero) };
        }
        else
        {
            throw new ArgumentException("CPF ou CNPJ inválido", nameof(numero));
        }
    }

    /// <summary>
    /// Cria um CPFouCNPJ a partir de uma numero de CPF ou CNPJ.
    /// </summary>
    /// <param name="numero"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static CPFouCNPJ Create(string numero)
    {
        if (ValidaCPF.IsCPF(numero.GetDigits()))
        {
            return new CPFouCNPJ { _valor = new CPF(numero) };
        }
        else if (ValidaCNPJ.IsCNPJ(numero.GetDigits()))
        {
            return new CPFouCNPJ { _valor = new CNPJ(numero) };
        }
        else
        {
            throw new ArgumentException("CPF ou CNPJ inválido", nameof(numero));
        }
    }

    /// <summary>
    /// Retorna uma string com o CPF ou CNPJ em um tamanho específico.
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public string ToString(int size) => _valor?.ToString(size) ?? string.Empty;

    /// <summary>
    /// Retorna uma string com o CPF ou CNPJ em um tamanho padrão (11 ou 14 dígitos).
    /// </summary>
    /// <returns></returns>
    public override string ToString() => _valor?.ToString() ?? string.Empty;

    /// <summary>
    /// Retorna uma string com o CPF ou CNPJ formatado.
    /// </summary>
    /// <returns></returns>
    public string ToStringFormated() => _valor?.ToStringFormated() ?? string.Empty;

    public static implicit operator CPFouCNPJ(ulong numero) => CPFouCNPJ.Create(numero);

    public static implicit operator CPFouCNPJ(string numero) => CPFouCNPJ.Create(numero);

    public override bool Equals(object? obj)
    {
        if (!(obj is CPF || obj is CNPJ))
            return false;

        if (obj is CNPJ _cnpj && _valor is CNPJ _cnpjInterno)
        {
            return _cnpj.Numero == _cnpjInterno.Numero;
        }
        else if (obj is CPF _cpf && _valor is CPF _cpfInterno)
        {
            return _cpf.Numero == _cpfInterno.Numero;
        }

        return false;
    }

    public static bool operator ==(CPFouCNPJ a, CPFouCNPJ b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(CPFouCNPJ a, CPFouCNPJ b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return _valor?.GetHashCode() ?? 0;
    }
}
