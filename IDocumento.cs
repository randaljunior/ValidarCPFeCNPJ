namespace ValidarCPFeCNPJ;

public interface IDocumento
{
    ulong Numero { get; init; }
    string ToString();
    string ToStringFormated();
    string ToString(int size);
}