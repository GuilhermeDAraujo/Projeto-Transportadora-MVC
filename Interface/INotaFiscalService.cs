using Projeto_Transportadora_MVC.Models;

namespace Projeto_Transportadora_MVC.Interface
{
    public interface INotaFiscalService
    {
        Task<NotaFiscal> CreateNotaFiscalAsync(NotaFiscal notaFiscal);
        Task<NotaFiscal> UpdateNotaFiscalAsync(NotaFiscal notaFiscal);
        Task DeleteNotaFiscalAsync(NotaFiscal notaFiscal);
        Task<List<Caminhao>> ListarTodosCaminhoesAsync();
        Task<bool> NotaFiscalJaExisteAsync(int id, int numeroNotaFiscal);
        Task<List<NotaFiscal>> ObterNotasFiscaisDeHojeAsync();
        Task<NotaFiscal> BuscarNotaFiscalPorId(int id);
    }
}