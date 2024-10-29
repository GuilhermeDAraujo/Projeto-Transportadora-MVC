using Microsoft.EntityFrameworkCore;
using Projeto_Transportadora_MVC.Context;
using Projeto_Transportadora_MVC.Models;

namespace Projeto_Transportadora_MVC.Services
{
    public class INotaFiscalService
    {
        private readonly TransportadoraContext _context;

        public INotaFiscalService(TransportadoraContext context)
        {
            _context = context;
        }

        public async Task<NotaFiscal> CreateAsync(NotaFiscal notaFiscal)
        {
            if (notaFiscal == null)
                throw new ArgumentNullException(nameof(notaFiscal));

            _context.NotasFiscais.Add(notaFiscal);
            await _context.SaveChangesAsync();
            return notaFiscal;
        }

        public async Task<NotaFiscal> UpdateNotaFiscalAsync(NotaFiscal notaFiscal)
        {
            if (notaFiscal == null)
                throw new ArgumentException("Nota Fiscal inválida");

            _context.Update(notaFiscal);
            await _context.SaveChangesAsync();
            return notaFiscal;
        }



        public async Task<List<Caminhao>> ListarTodosCaminhoesAsync()
        {
            return await _context.Caminhoes.ToListAsync();
        }

        public async Task<bool> NotaFiscalJaExisteAsync(int id, int numeroNotaFiscal)
        {
            return await _context.NotasFiscais.AnyAsync(nf => nf.Id == id && nf.NumeroNotaFiscal == numeroNotaFiscal);
        }

        public async Task<List<NotaFiscal>> ObjerNotasFiscaisDeHojeAsync()
        {
            var hoje = DateTime.Today;
            return await _context.NotasFiscais
                .Where(nf => nf.DataDaEntrada == hoje)
                .ToListAsync();
        }

        public async Task<NotaFiscal> BuscarNotaFiscalPorId(int id)
        {
            return await _context.NotasFiscais.FindAsync(id);
        }
    }
}