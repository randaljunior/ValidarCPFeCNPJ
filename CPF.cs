using MyExtensions;

namespace ValidarCPFeCNPJ;

public record struct CPF : IDocumento
{
    private const uint _size = 11;

    private ulong _numero;

    public ulong Numero
    {
        get => _numero;
        init
        {
            if (ValidacaoDocumentos.IsCPF(value))
            {
                throw new ArgumentException("CPF inválido");
            }

            _numero = value;
        }
    }

    public CPF(ulong numero)
    {
        Numero = numero;
    }

    public CPF(string numero)
    {
        if(!ValidacaoDocumentos.IsCpfStringRegex(numero))
        {
            throw new ArgumentException("CPF inválido");
        }

        var _numeroLimpo = numero.GetDigits();

        if (!ValidacaoDocumentos.IsCPF(_numeroLimpo))
        {
            throw new ArgumentException("CPF inválido");
        }

        _numero = _numeroLimpo.ToUlong() ?? 0;
    }

    public override readonly string ToString()
    {
        return _numero.ToString('0'.Repeat((int)_size));
    }

    public readonly string ToString(int size)
    {
        return _numero.ToString('0'.Repeat(size));
    }

    public readonly string ToStringFormated()
    {
        // 000.000.000-00
        // 01234567890123
        // 012 345 678 90

        var digitos = _numero.GetDigits((int)_size);
        Span<char> span = stackalloc char[(int)_size + 3];

        for (int i = 0; i <= 2; i++)
        {
            span[i] = (char)digitos[i];
        }

        span[3] = '.';

        for (int i = 3; i <= 5; i++)
        {
            span[i + 1] = (char)digitos[i];
        }

        span[7] = '.';

        for (int i = 6; i <= 8; i++)
        {
            span[i + 2] = (char)digitos[i];
        }

        span[11] = '-';

        for (int i = 9; i <= 10; i++)
        {
            span[i + 3] = (char)digitos[i];
        }

        return span.ToString();
    }

    public static implicit operator CPF(ulong numero) => new CPF(numero);

    public static implicit operator CPF(string numero) => new CPF(numero);
}
