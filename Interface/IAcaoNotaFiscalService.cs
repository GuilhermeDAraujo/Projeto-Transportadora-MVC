using Projeto_Transportadora_MVC.Models;

namespace Projeto_Transportadora_MVC.Interface
{
    public interface IAcaoNotaFiscalService
    {
        Task<IEnumerable<AcaoNotaFiscal>> ObterNotasNaoFechadasAsync();
        Task<AcaoNotaFiscal> ObterPorIdAsync(int id);
        Task CriarAsync(AcaoNotaFiscal acaoNotaFiscal);
        Task AtualizarAsync(AcaoNotaFiscal acaoNotaFiscal);
    }
}