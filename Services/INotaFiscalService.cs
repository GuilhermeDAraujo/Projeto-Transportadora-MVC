using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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

        public async Task<NotaFiscal> CreateNotaFiscalAsync(NotaFiscal notaFiscal)
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

        public async Task DeleteNotaFiscalAsync(NotaFiscal notaFiscal)
        {
            _context.Remove(notaFiscal);
            await _context.SaveChangesAsync();
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


        public List<NotaFiscal> ImportarNotasFiscais(Stream arquivoExcelStream)
        {
            List<NotaFiscal> notasFiscais = new List<NotaFiscal>();

            using (var package = new ExcelPackage(arquivoExcelStream))
            {
                var planilha = package.Workbook.Worksheets[0];

                for(int linha = 2; linha <= planilha.Dimension.End.Row; linha++)
                {
                    NotaFiscal notaFiscal = new NotaFiscal
                    {
                        NumeroNotaFiscal = int.Parse(planilha.Cells[linha, 1].Text),
                        NomeCliente = planilha.Cells[linha, 2].Text,
                        EnderecoFaturado = planilha.Cells[linha,3].Text,
                        DataDoFaturamento = DateTime.Parse(planilha.Cells[linha,4].Text),
                        DataDaEntrada = DateTime.Parse(planilha.Cells[linha,5].Text),
                        NumeroDaCarga = int.Parse(planilha.Cells[linha,6].Text)
                    };

                    notasFiscais.Add(notaFiscal);
                }
            }
            return notasFiscais;
        }
    }
}