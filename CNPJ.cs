using MyExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidarCPFeCNPJ;

public record struct CNPJ : IDocumento
{
    private const uint _size = 14;

    private ulong _numero;

    public ulong Numero
    {
        get => _numero;
        init
        {
            if (ValidacaoDocumentos.IsCNPJ(value))
            {
                throw new ArgumentException("CNPJ inválido");
            }

            _numero = value;
        }
    }

    public CNPJ(ulong numero)
    {
        Numero = numero;
    }

    public CNPJ(string numero)
    {
        if (!ValidacaoDocumentos.IsCnpjStringRegex(numero))
        {
            throw new ArgumentException("CPF inválido");
        }

        var _numeroLimpo = numero.GetDigits();

        if (!ValidacaoDocumentos.IsCNPJ(_numeroLimpo))
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
        // 00.000.000/0000-00
        // 012345678901234567
        // 01 234 567 8901 23

        var digitos = _numero.GetDigits((int)_size);
        Span<char> span = stackalloc char[(int)_size + 3];

        for (int i = 0; i <= 1; i++)
        {
            span[i] = (char)digitos[i];
        }

        span[2] = '.';

        for (int i = 2; i <= 4; i++)
        {
            span[i + 1] = (char)digitos[i];
        }

        span[6] = '.';

        for (int i = 5; i <= 7; i++)
        {
            span[i + 2] = (char)digitos[i];
        }

        span[10] = '/';

        for (int i = 8; i <= 11; i++)
        {
            span[i + 3] = (char)digitos[i];
        }

        span[15] = '-';

        for (int i = 12; i <= 13; i++)
        {
            span[i + 4] = (char)digitos[i];
        }

        return span.ToString();
    }

    public static implicit operator CNPJ(ulong numero) => new CNPJ(numero);

    public static implicit operator CNPJ(string numero) => new CNPJ(numero);
}
