using MyExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZLinq.Simd;

namespace ValidarCPFeCNPJ;

public static partial class ValidaCNPJ
{
    private const int _CNPJSize = 14;

    [GeneratedRegex(@"^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$")]
    private static partial Regex CnpjStringRegex();

    [GeneratedRegex(@"^[0-9]{14}$")]
    private static partial Regex CnpjRegex();

    /// <summary>
    /// Verifica se um número é um CNPJ.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsCNPJ(ulong input)
    {
        return CheckCnpjDv(input.GetDigits());
    }

    /// <summary>
    /// Verifica se um número é um CNPJ.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsCNPJ(ReadOnlySpan<uint> input)
    {
        return CheckCnpjDv(input);
    }

    /// <summary>
    /// Verifica se uma string com pontuação parece um CNPJ.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsCnpjStringRegex(ReadOnlySpan<char> input)
    {
        return CnpjStringRegex().IsMatch(input);
    }

    /// <summary>
    /// Verifica se uma string sem pontuação parece um CNPJ.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsCnpjRegex(ReadOnlySpan<char> input)
    {
        return CnpjRegex().IsMatch(input);
    }

    /// <summary>
    /// Verifica se um CNPJ é válido.
    /// </summary>
    /// <param name="cnpj"></param>
    /// <returns></returns>
    public static bool CheckCnpjDv(ReadOnlySpan<char> cnpj)
    {
        if (cnpj.Length != _CNPJSize || !CnpjRegex().IsMatch(cnpj)) return false;

        Span<int> _cnpj = stackalloc int[_CNPJSize];

        for (int i = 0; i < _CNPJSize; i++)
        {
            _cnpj[i] = cnpj[i] - '0';
        }

        return CheckCnpjDv(_cnpj);
    }

    /// <summary>
    /// Verifica se um CNPJ é válido.
    /// </summary>
    /// <param name="cnpj"></param>
    /// <returns></re
    public static bool CheckCnpjDv(ReadOnlySpan<int> cnpj)
    {
        Span<uint> _cnpj = stackalloc uint[_CNPJSize];

        for (int i = 0; i < _cnpj.Length; i++)
        {
            _cnpj[i] = (uint)cnpj[i];
        }

        return CheckCnpjDv(_cnpj);
    }

    /// <summary>
    /// Verifica se um CNPJ é válido.
    /// </summary>
    /// <param name="cnpj"></param>
    /// <returns></re
    public static bool CheckCnpjDv(ReadOnlySpan<uint> cnpj)
    {
        if (cnpj.Length != _CNPJSize) return false;

        var _dvs = GetCnpjDv(cnpj.Slice(0, _CNPJSize - 2));

        if (_dvs.DV1 != cnpj[_CNPJSize - 2]) return false;

        if (_dvs.DV2 != cnpj[_CNPJSize - 1]) return false;

        return true;
    }

    public static (uint DV1, uint DV2) GetCnpjDv(ReadOnlySpan<uint> RaizCnpj)
    {
        if (RaizCnpj.Length != _CNPJSize - 2) throw new ArgumentException($"Raiz do CPF deve possuir {_CNPJSize - 2} digitos.");

        uint _dv1result, _dv2result;

        {
            Span<uint> _dv1 = stackalloc uint[] { 6, 7, 8, 9, 2, 3, 4, 5, 6, 7, 8, 9 };

            uint _dv1Sum = 0;

            for (int i = 0; i < RaizCnpj.Length; i++)
            {
                _dv1Sum += RaizCnpj[i] * _dv1[i];
            }

            _dv1result = (_dv1Sum % 11 != 10) ? _dv1Sum % 11 : 0;
        }

        {
            Span<uint> _dv2 = stackalloc uint[] { 5, 6, 7, 8, 9, 2, 3, 4, 5, 6, 7, 8, 9 };

            uint _dv2Sum = 0;

            for (int i = 0; i < RaizCnpj.Length; i++)
            {
                _dv2Sum += RaizCnpj[i] * _dv2[i];
            }

            _dv2Sum += _dv1result * _dv2[^1];

            _dv2result = (_dv2Sum % 11 != 10) ? _dv2Sum % 11 : 0;
        }

        return (_dv1result, _dv2result);
    }
}
