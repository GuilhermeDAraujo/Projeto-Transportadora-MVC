using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_Transportadora_MVC.Context;
using Projeto_Transportadora_MVC.Enums;
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

        public async Task<IEnumerable<AcaoNotaFiscal>> ObterNotasPorTipoAcaoEDataFaturamentoAsync(TipoAcao? tipoAcao, DateTime? dataFaturamento)
        {
            return await _context.AcoesNotaFiscal
                .Where(a => !tipoAcao.HasValue || a.TipoAcao == tipoAcao.Value)
                .Where(a => !dataFaturamento.HasValue || a.NotaFiscal.DataDoFaturamento == dataFaturamento.Value)
                .Include(nf => nf.NotaFiscal)
                .Include(c => c.Caminhao)
                .ToListAsync();
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

        public List<SelectListItem> BuscarPorStatus()
        {
            return Enum.GetValues(typeof(TipoAcao))
                .Cast<TipoAcao>()
                .Select(t => new SelectListItem
                {
                    Value = t.ToString(),
                    Text = t.ToString()
                })
                .ToList();
        }

        public async Task<List<Caminhao>> BuscarCaminhao()
        {
            return await _context.Caminhoes.ToListAsync();
        }
    }
}