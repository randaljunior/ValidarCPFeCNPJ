using MyExtensions;

namespace ValidarCPFeCNPJ;

public record struct CPF : IDocumento
{
    private const uint _size = 11;

    private ulong _numero;

    /// <summary>
    /// Número do CPF.
    /// </summary>
    public ulong Numero
    {
        get => _numero;
        init
        {
            if (ValidaCPF.IsCPF(value))
            {
                throw new ArgumentException("CPF inválido");
            }

            _numero = value;
        }
    }

    /// <summary>
    /// Construtor que recebe um ulong com o CPF.
    /// </summary>
    /// <param name="numero"></param>
    public CPF(ulong numero)
    {
        Numero = numero;
    }

    /// <summary>
    /// Construtor que recebe uma string com o CPF.
    /// </summary>
    /// <param name="numero"></param>
    /// <exception cref="ArgumentException"></exception>
    public CPF(string numero)
    {
        if (!ValidaCPF.IsCpfStringRegex(numero) && !ValidaCPF.IsCpfRegex(numero))
        {
            throw new ArgumentException("CPF inválido");
        }

        var _numeroLimpo = numero.GetDigits();

        if (!ValidaCPF.IsCPF(_numeroLimpo))
        {
            throw new ArgumentException("CPF inválido");
        }

        _numero = _numeroLimpo.ToUlong() ?? 0;
    }

    /// <summary>
    /// Converte o CPF para uma string com o tamanho padrão (11 dígitos).
    /// </summary>
    /// <returns></returns>
    public override readonly string ToString()
    {
        return _numero.ToString('0'.Repeat((int)_size));
    }

    /// <summary>
    /// Converte o CPF para uma string com o tamanho especificado.
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public readonly string ToString(int size)
    {
        return _numero.ToString('0'.Repeat(size));
    }

    /// <summary>
    /// Converte o CPF para uma string formatada.
    /// </summary>
    /// <returns></returns>
    public readonly string ToStringFormated()
    {
        var digitos = _numero.GetDigits((int)_size);
        Span<char> span = stackalloc char[(int)_size + 3];
        
        int digitIndex = 0;
        for (int i = 0; i < span.Length; i++)
        {
            span[i] = i switch
            {
                3 or 7 => '.',
                11 => '-',
                _ => (char)(digitos[digitIndex++] + '0')
            };
        }

        return span.ToString();
    }

    public static implicit operator CPF(ulong numero) => new CPF(numero);

    public static implicit operator CPF(string numero) => new CPF(numero);
}
