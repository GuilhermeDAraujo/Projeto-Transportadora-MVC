using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Drawing.Chart;
using Projeto_Transportadora_MVC.Context;
using Projeto_Transportadora_MVC.Models;

namespace Projeto_Transportadora_MVC.Services
{
    public class NotaFiscalService
    {
        private readonly TransportadoraContext _context;

        public NotaFiscalService(TransportadoraContext context)
        {
            _context = context;
        }

        public async Task<NotaFiscal> CreateNotaFiscalAsync(NotaFiscal notaFiscal)
        {
            if (notaFiscal == null)
                throw new ArgumentNullException(nameof(notaFiscal));

            if (await NotaFiscalJaExisteAsync(notaFiscal.NumeroNotaFiscal))
                throw new InvalidOperationException("Nota Fiscal já existe");

            try
            {
                _context.NotasFiscais.Add(notaFiscal);
                await _context.SaveChangesAsync();
                return notaFiscal;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar Nota Fiscal", ex);
            }
        }

        public async Task<NotaFiscal> UpdateNotaFiscalAsync(NotaFiscal notaFiscal)
        {
            if (notaFiscal == null)
                throw new ArgumentException("Nota Fiscal inválida");

            if (await NotaFiscalJaExisteAsync(notaFiscal.NumeroNotaFiscal))
                throw new InvalidOperationException("Nota Fiscal já existe.");

            try
            {
                _context.Update(notaFiscal);
                await _context.SaveChangesAsync();
                return notaFiscal;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar a Nota Fiscal", ex);
            }
        }

        public async Task DeleteNotaFiscalAsync(NotaFiscal notaFiscal)
        {
            try
            {
                _context.Remove(notaFiscal);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar a Nota Fiscal", ex);
            }
        }



        public async Task<List<Caminhao>> ListarTodosCaminhoesAsync()
        {
            try
            {
                return await _context.Caminhoes.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar Caminhões", ex);
            }
        }

        public async Task<bool> NotaFiscalJaExisteAsync(int numeroNotaFiscal)
        {
            try
            {
                return await _context.NotasFiscais.AnyAsync(nf => nf.NumeroNotaFiscal == numeroNotaFiscal);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao verificar existência da Nota Fisacl", ex);
            }
        }

        public async Task<List<NotaFiscal>> ObterjerNotasFiscaisDeHojeAsync()
        {
            var hoje = DateTime.Today;

            try
            {
                return await _context.NotasFiscais
                    .Where(nf => nf.DataDaEntrada == hoje)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter Notas Fiscai lançadas hoje", ex);
            }
        }

        public async Task<NotaFiscal> BuscarNotaFiscalPorId(int id)
        {
            try
            {
                return await _context.NotasFiscais.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar a Nota Fiscal pelo ID", ex);
            }
        }
    }
}