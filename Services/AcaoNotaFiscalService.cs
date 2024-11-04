using Projeto_Transportadora_MVC.Context;

namespace Projeto_Transportadora_MVC.Services
{
    public class AcaoNotaFiscalService
    {
        private readonly TransportadoraContext _context;

        public AcaoNotaFiscalService(TransportadoraContext context)
        {
            _context = context;
        }

        
    }
}