using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Projeto_Transportadora_MVC.Context;
using Projeto_Transportadora_MVC.Enums;
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
            var notasFiscais = ProcessarArquivoExcel(arquivoExcel);
            await SalvarNotasEAssociarAcoes(notasFiscais);
            return notasFiscais;
        }

        private List<NotaFiscal> ProcessarArquivoExcel(Stream arquivoExcel)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            List<NotaFiscal> notasFisicais = new List<NotaFiscal>();

            using (var package = new ExcelPackage(arquivoExcel))
            {
                var planilha = package.Workbook.Worksheets[0];

                if (planilha == null || planilha.Dimension == null || planilha.Dimension.End.Row < 2)
                    throw new Exception("A planilha está vazia ou não possui dados");

                for (int linha = 2; linha <= planilha.Dimension.End.Row; linha++)
                {
                    NotaFiscal notaFiscal = new NotaFiscal
                    {
                        NumeroNotaFiscal = int.Parse(planilha.Cells[linha, 1].Text),
                        NomeCliente = planilha.Cells[linha, 2].Text,
                        EnderecoFaturado = planilha.Cells[linha, 3].Text,
                        DataDoFaturamento = DateTime.Parse(planilha.Cells[linha, 4].Text),
                        NumeroDaCarga = int.Parse(planilha.Cells[linha, 5].Text)
                    };

                    if (_context.NotasFiscais.Any(nf => nf.NumeroNotaFiscal == notaFiscal.NumeroNotaFiscal))
                        throw new InvalidOperationException($"A Nota Fiscal {notaFiscal.NumeroNotaFiscal} já existe");

                    if (string.IsNullOrWhiteSpace(notaFiscal.NomeCliente))
                        throw new InvalidOperationException($"A Nota Fiscal {notaFiscal.NumeroNotaFiscal} não contém Nome do Cliente");

                    if (string.IsNullOrWhiteSpace(notaFiscal.EnderecoFaturado))
                        throw new InvalidOperationException($"A Nota Fiscal {notaFiscal.NumeroNotaFiscal} não contém Endereço");

                    notasFisicais.Add(notaFiscal);
                }
            }
            return notasFisicais;
        }

        private async Task SalvarNotasEAssociarAcoes(List<NotaFiscal> notasFiscais)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.NotasFiscais.AddRange(notasFiscais);
                    await _context.SaveChangesAsync();

                    var acoesNotaFiscal = new List<AcaoNotaFiscal>();
                    foreach (var notaFiscal in notasFiscais)
                    {
                        acoesNotaFiscal.Add(new AcaoNotaFiscal
                        {
                            TipoAcao = TipoAcao.Entrada,
                            DataDaAcao = DateTime.Now.Date,
                            Descriacao = "Entrada da Nota Fiscal e aguardando ação",
                            NotaFiscalId = notaFiscal.Id,
                            CaminhaoId = 1,
                            DataAgendada = null,
                            StatusAgendamento = null
                        });
                    }

                    await _context.AcoesNotaFiscal.AddRangeAsync(acoesNotaFiscal);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (DbUpdateException ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Erro ao salvar as notas fiscais e ações", ex);
                }
            }
        }
    }
}