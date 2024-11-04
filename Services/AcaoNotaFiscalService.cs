using Microsoft.EntityFrameworkCore;
using Projeto_Transportadora_MVC.Context;
using Projeto_Transportadora_MVC.Models;

namespace Projeto_Transportadora_MVC.Services
{
    public class AcaoNotaFiscalService
    {
        private readonly TransportadoraContext _context;

        public AcaoNotaFiscalService(TransportadoraContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AcaoNotaFiscal>> ObterNotasNaoFechadasAsync()
        {
            return await _context.AcoesNotaFiscal
                .Where(a => a.TipoAcao != Enums.TipoAcao.SaidaNoFechamento)
                .Include(nf => nf.NotaFiscal)
                .Include(c => c.Caminhao)
                .ToListAsync();
        }

        public async Task<AcaoNotaFiscal> ObterPorIdAsync(int id)
        {
            return await _context.AcoesNotaFiscal
                .Include(nf => nf.NotaFiscal)
                .Include(c => c.Caminhao)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task CriarAcaoAsync(AcaoNotaFiscal acaoNotaFiscal)
        {
            await _context.AcoesNotaFiscal.AddAsync(acaoNotaFiscal);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAcaoAsync(AcaoNotaFiscal acaoNotaFiscal)
        {
            _context.AcoesNotaFiscal.Update(acaoNotaFiscal);
            await _context.SaveChangesAsync();
        }
    }
}