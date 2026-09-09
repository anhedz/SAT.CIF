using System;
using Jaeger.SAT.CIF.Entities;
using Jaeger.SAT.CIF.Interfaces;
using Jaeger.SAT.CIF.Services;

namespace CIF.Sample {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("********************************************************");
            Console.WriteLine("** Consulta de Cédula Fiscal");
            Console.WriteLine("********************************************************");
            IQueryService service = new QueryService();
            IRequest request = Request.Create().AddRFC("XXXXXXXX").AddId("00000000").Build();
            ICedulaFiscal cedulaFiscal;
            var responseCedula = service.Execute(request);
            Console.WriteLine(request.URL);
            if (responseCedula.IsValida) {
                cedulaFiscal = responseCedula.CedulaFiscal;
                if (cedulaFiscal.TipoPersona == CedulaFiscal.TipoPersonaEnum.Fisica) {
                    Console.WriteLine(cedulaFiscal.Fisica.ToString());
                } else if (cedulaFiscal.TipoPersona == CedulaFiscal.TipoPersonaEnum.Moral) {
                    Console.WriteLine(cedulaFiscal.Moral.ToString());
                }
            } else {
                Console.WriteLine(responseCedula.Message);
            }
        }
    }
}
