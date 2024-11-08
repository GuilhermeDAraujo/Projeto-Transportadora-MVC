using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Projeto_Transportadora_MVC.Context;
using Projeto_Transportadora_MVC.Models;

namespace Projeto_Transportadora_MVC.Services
{
    public class ImportarExcelService
    {
        private readonly TransportadoraContext _context;

        public ImportarExcelService(TransportadoraContext context)
        {
            _context = context;
        }

        public async Task<List<NotaFiscal>> ImportarNotasFiscais(Stream arquivoExcel)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            List<NotaFiscal> notasFiscais = new List<NotaFiscal>();
            List<AcaoNotaFiscal> acaoNotaFiscals = new List<AcaoNotaFiscal>();

            using (var package = new ExcelPackage(arquivoExcel))
            {
                var planilha = package.Workbook.Worksheets[0];

                if (planilha == null || planilha.Dimension == null || planilha.Dimension.End.Row < 2)
                    throw new Exception("A planilha está vazia ou não possui dados.");

                for (int linha = 2; linha <= planilha.Dimension.End.Row; linha++)
                {
                    NotaFiscal notaFiscal = new NotaFiscal
                    {
                        NumeroNotaFiscal = int.Parse(planilha.Cells[linha, 1].Text),
                        NomeCliente = planilha.Cells[linha, 2].Text,
                        EnderecoFaturado = planilha.Cells[linha, 3].Text,
                        DataDoFaturamento = DateTime.Parse(planilha.Cells[linha, 4].Text),
                        NumeroDaCarga = int.Parse(planilha.Cells[linha, 6].Text)
                    };

                    if (_context.NotasFiscais.Any(nf => nf.NumeroNotaFiscal == notaFiscal.NumeroNotaFiscal))
                        throw new InvalidOperationException($"A nota fiscal {notaFiscal.NumeroNotaFiscal} já existe.");

                    notasFiscais.Add(notaFiscal);

                    acaoNotaFiscals.Add(new AcaoNotaFiscal
                    {
                        TipoAcao = Enums.TipoAcao.Entrada,
                        DataDaAcao = DateTime.Now.Date,
                        Descriacao = "Entrada da Nota Fiscal e aguardando ação",
                        NotaFiscalId = notaFiscal.Id,
                        CaminhaoId = 1,
                        DataAgendada = null,
                        StatusAgendamento = null
                    });
                }
            }
            SalvarNotasFiscais(notasFiscais, acaoNotaFiscals);
            return notasFiscais;
        }

        private void SalvarNotasFiscais(List<NotaFiscal> notasFiscais, List<AcaoNotaFiscal> acaoNotaFiscals)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.NotasFiscais.AddRange(notasFiscais);
                    _context.SaveChanges();

                    _context.AcoesNotaFiscal.AddRange(acaoNotaFiscals);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao salvar as notas fiscais", ex);
                }
            }
        }
    }
}