using System.Text.RegularExpressions;
using MyExtensions;

namespace ValidarCPFeCNPJ;

public static partial class ValidacaoDocumentos
{
    private const int _CPFSize = 11;
    private const int _CNPJSize = 14;

    public static bool IsCPF(ulong numero)
    {
        return CheckCpfDv(numero.GetDigits());
    }

    public static bool IsCPF(ReadOnlySpan<uint> numero)
    {
        return CheckCpfDv(numero);
    }

    public static bool IsCNPJ(ulong numero)
    {
        return CheckCnpjDv(numero.GetDigits());
    }

    public static bool IsCNPJ(ReadOnlySpan<uint> numero)
    {
        return CheckCnpjDv(numero);
    }

    [GeneratedRegex(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$")]
    public static partial Regex CpfStringRegex();

    public static bool IsCpfStringRegex(ReadOnlySpan<char> input)
    {
        return CpfStringRegex().IsMatch(input);
    }

    [GeneratedRegex(@"^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$")]
    public static partial Regex CnpjStringRegex();

    public static bool IsCnpjStringRegex(ReadOnlySpan<char> input)
    {
        return CnpjStringRegex().IsMatch(input);
    }

    [GeneratedRegex(@"^[0-9]{11}$")]
    public static partial Regex CpfRegex();

    public static bool IsCpfRegex(ReadOnlySpan<char> input)
    {
        return CpfRegex().IsMatch(input);
    }

    [GeneratedRegex(@"^[0-9]{14}$")]
    public static partial Regex CnpjRegex();

    public static bool IsCnpjRegex(ReadOnlySpan<char> input)
    {
        return CnpjRegex().IsMatch(input);
    }

    public static bool CheckCpfDv(ReadOnlySpan<char> cpf)
    {
        if (cpf.Length != _CPFSize || !CpfRegex().IsMatch(cpf)) return false;

        Span<int> _cpf = stackalloc int[_CPFSize];

        for (int i = 0; i < _cpf.Length; i++)
        {
            int.TryParse(cpf.Slice(i, 1), out _cpf[i]);
        }

        return CheckCpfDv(_cpf);
    }

    public static bool CheckCpfDv(ReadOnlySpan<uint> cpf)
    {
        if (cpf.Length != _CPFSize) return false;

        var _dvs = GetCpfDv(cpf.Slice(0, _CPFSize - 2));

        if (_dvs.DV1 != cpf[_CPFSize - 2]) return false;

        if (_dvs.DV2 != cpf[_CPFSize - 1]) return false;

        return true;
    }

    public static bool CheckCpfDv(ReadOnlySpan<int> cpf)
    {
        Span<uint> _cpf = stackalloc uint[_CPFSize];

        for (int i = 0; i < _cpf.Length; i++)
        {
            _cpf[i] = (uint)cpf[i];
        }

        return CheckCnpjDv(_cpf);
    }

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

    public static bool CheckCnpjDv(ReadOnlySpan<char> cnpj)
    {
        if (cnpj.Length != _CNPJSize || !CnpjRegex().IsMatch(cnpj)) return false;

        Span<int> _cnpj = stackalloc int[_CNPJSize];

        for (int i = 0; i < _cnpj.Length; i++)
        {
            int.TryParse(cnpj.Slice(i, 1), out _cnpj[i]);
        }

        return CheckCnpjDv(_cnpj);
    }

    public static bool CheckCnpjDv(ReadOnlySpan<int> cnpj)
    {
        Span<uint> _cnpj = stackalloc uint[_CNPJSize];

        for (int i = 0; i < _cnpj.Length; i++)
        {
            _cnpj[i] =  (uint)cnpj[i];
        }

        return CheckCnpjDv(_cnpj);
    }

    public static bool CheckCnpjDv(ReadOnlySpan<uint> cnpj)
    {
        if (cnpj.Length != _CNPJSize) return false;

        var _dvs = GetCpfDv(cnpj.Slice(0, _CNPJSize - 2));

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