using Jaeger.SAT.CIF.Entities;

namespace Jaeger.SAT.CIF.Interfaces {
    public interface IRegimenFiscalResolver {
        RegimenFiscal Resolve(string descripcion);
    }
}