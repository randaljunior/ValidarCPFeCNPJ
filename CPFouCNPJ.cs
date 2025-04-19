
namespace ValidarCPFeCNPJ;

public class CPFouCNPJ
{
    private object? _valor;

    private CPFouCNPJ() { }

    public CPFouCNPJ(CPF cpf)
    {
        _valor = cpf;
    }

    public CPFouCNPJ(CNPJ cpf)
    {
        _valor = cpf;
    }

    public ulong Valor => ((IDocumento?)_valor)?.Numero ?? 0;

    public new Type? GetType()
    {
        return _valor?.GetType();
    }

    public static CPFouCNPJ Create(CPF cpf)
    {
        return new CPFouCNPJ { _valor = cpf };
    }

    public static CPFouCNPJ Create(CNPJ cnpj)
    {
        return new CPFouCNPJ { _valor = cnpj };
    }

    public static CPFouCNPJ Create(ulong Numero)
    {
        if (ValidacaoDocumentos.IsCPF(Numero))
        {
            return new CPFouCNPJ { _valor = new CPF(Numero) };
        }
        else if (ValidacaoDocumentos.IsCNPJ(Numero))
        {
            return new CPFouCNPJ { _valor = new CNPJ(Numero) };
        }
        else
        {
            throw new ArgumentException("CPF ou CNPJ inválido");
        }
    }

    public string ToString(int size) => ((IDocumento?)_valor)?.ToString(size) ?? string.Empty;

    public override string ToString()
    {
        return ToString(1);
    }

    public string ToStringFormated() => ((IDocumento?)_valor)?.ToStringFormated() ?? string.Empty;

    public override bool Equals(object? obj)
    {
        if (obj is not CPF || obj is not CNPJ) return false;

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
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(CPFouCNPJ a, CPFouCNPJ b)
    {
        if (a is null && b is null) return false;
        if (a is null || b is null) return true;
        return !a.Equals(b);
    }

    public override int GetHashCode()
    {
        return _valor?.GetHashCode() ?? 0;
    }
}
