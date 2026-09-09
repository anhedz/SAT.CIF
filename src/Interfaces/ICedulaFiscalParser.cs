using Jaeger.SAT.CIF.Entities;

namespace Jaeger.SAT.CIF.Interfaces {
    public interface ICedulaFiscalParser {
        CedulaFiscal Parse(string html);
    }
}