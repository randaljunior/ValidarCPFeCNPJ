using MyExtensions;
using System.Text.RegularExpressions;

namespace ValidarCPFeCNPJ;

public static partial class ValidaCPF
{
    private const int _CPFSize = 11;

    [GeneratedRegex(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$")]
    private static partial Regex CpfStringRegex();

    [GeneratedRegex(@"^[0-9]{11}$")]
    private static partial Regex CpfRegex();

    /// <summary>
    /// Verifica se um número é um CPF.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsCPF(ulong input)
    {
        return CheckCpfDv(input.GetDigits());
    }

    /// <summary>
    /// Verifica se um número é um CPF.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsCPF(ReadOnlySpan<uint> input)
    {
        return CheckCpfDv(input);
    }

    /// <summary>
    /// Verifica se uma string sem pontuação parece um CPF.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsCpfRegex(ReadOnlySpan<char> input)
    {
        return CpfRegex().IsMatch(input);
    }

    /// <summary>
    /// Verifica se uma string com pontuação parece um CPF.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsCpfStringRegex(ReadOnlySpan<char> input)
    {
        return CpfStringRegex().IsMatch(input);
    }

    /// <summary>
    /// Verifica se um CPF é válido.
    /// </summary>
    /// <param name="cpf"></param>
    /// <returns></returns>
    public static bool CheckCpfDv(ReadOnlySpan<char> cpf)
    {
        if (cpf.Length != _CPFSize || !CpfRegex().IsMatch(cpf)) return false;

        Span<int> _cpf = stackalloc int[_CPFSize];

        for (int i = 0; i < _CPFSize; i++)
        {
            _cpf[i] = cpf[i] - '0';
        }

        return CheckCpfDv(_cpf);
    }

    /// <summary>
    /// Verifica se um CPF é válido.
    /// </summary>
    /// <param name="cpf"></param>
    /// <returns></returns>
    public static bool CheckCpfDv(ReadOnlySpan<uint> cpf)
    {
        if (cpf.Length != _CPFSize) return false;

        var _dvs = GetCpfDv(cpf.Slice(0, _CPFSize - 2));

        if (_dvs.DV1 != cpf[_CPFSize - 2]) return false;

        if (_dvs.DV2 != cpf[_CPFSize - 1]) return false;

        return true;
    }

    /// <summary>
    /// Verifica se um CPF é válido.
    /// </summary>
    /// <param name="cpf"></param>
    /// <returns></returns>
    public static bool CheckCpfDv(ReadOnlySpan<int> cpf)
    {
        Span<uint> _cpf = stackalloc uint[_CPFSize];

        for (int i = 0; i < _cpf.Length; i++)
        {
            _cpf[i] = (uint)cpf[i];
        }

        return CheckCpfDv(_cpf);
    }

    /// <summary>
    /// Obtem os dígitos verificadores de um CPF.
    /// </summary>
    /// <param name="RaizCpf">CPF sem os Dígitos Verificadores</param>
    /// <returns></returns>
    public static (uint DV1, uint DV2) GetCpfDv(ReadOnlySpan<uint> RaizCpf)
    {
        if (RaizCpf.Length != _CPFSize - 2) throw new ArgumentException($"Raiz do CPF deve possuir {_CPFSize - 2} digitos.");

        uint _dv1result, _dv2result;

        {
            Span<uint> _dv1 = stackalloc uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            uint _dv1Sum = 0;

            for (int i = 0; i < RaizCpf.Length; i++)
            {
                _dv1Sum += RaizCpf[i] * _dv1[i];
            }

            _dv1result = (_dv1Sum % 11 != 10) ? _dv1Sum % 11 : 0;
        }

        {
            Span<uint> _dv2 = stackalloc uint[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            uint _dv2Sum = 0;

            for (int i = 0; i < RaizCpf.Length; i++)
            {
                _dv2Sum += RaizCpf[i] * _dv2[i];
            }

            _dv2Sum += _dv1result * _dv2[^1];

            _dv2result = (_dv2Sum % 11 != 10) ? _dv2Sum % 11 : 0;
        }

        return (_dv1result, _dv2result);
    }
}
