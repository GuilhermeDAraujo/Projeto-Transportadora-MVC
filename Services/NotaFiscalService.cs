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

            if (await VerificarNotaJaCadastradaCreateAsync(notaFiscal.NumeroNotaFiscal))
                throw new InvalidOperationException("Nota Fiscal já existe");

            try
            {
                _context.NotasFiscais.Add(notaFiscal);
                await _context.SaveChangesAsync();

                await CreateAcaoNotaFiscalAsync(notaFiscal);
                return notaFiscal;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar Nota Fiscal", ex);
            }
        }

        private async Task CreateAcaoNotaFiscalAsync(NotaFiscal notaFiscal)
        {
            var acaoNotaFiscal = new AcaoNotaFiscal
            {
                TipoAcao = Enums.TipoAcao.Entrada,
                DataDaAcao = DateTime.Now.Date,
                Descriacao = "Entrada da Nota Fiscal e aguardando ação",
                NotaFiscalId = notaFiscal.Id,
                CaminhaoId = 1,
                DataAgendada = null,
                StatusAgendamento = null
            };

            _context.AcoesNotaFiscal.Add(acaoNotaFiscal);
            await _context.SaveChangesAsync();
        }

        public async Task<NotaFiscal> UpdateNotaFiscalAsync(NotaFiscal notaFiscal)
        {
            if (notaFiscal == null)
                throw new ArgumentException("Nota Fiscal inválida");

            if (await VerificarNotaJaCadastradaUpdateAsync(notaFiscal.NumeroNotaFiscal, notaFiscal.Id))
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

        public async Task<bool> VerificarNotaJaCadastradaCreateAsync(int numeroNotaFiscal)
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

        public async Task<bool> VerificarNotaJaCadastradaUpdateAsync(int numeroNotaFiscal, int idNotaFiscal)
        {
            try
            {
                return await _context.NotasFiscais.AnyAsync(nf => nf.NumeroNotaFiscal == numeroNotaFiscal && nf.Id == idNotaFiscal);
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
                return await _context.AcoesNotaFiscal
                    .Where(a => a.TipoAcao == Enums.TipoAcao.Entrada && a.DataDaAcao == hoje)
                    .Select(a => new NotaFiscal
                    {
                        Id = a.NotaFiscal.Id,
                        NumeroNotaFiscal = a.NotaFiscal.NumeroNotaFiscal,
                        EnderecoFaturado = a.NotaFiscal.EnderecoFaturado,
                        NomeCliente = a.NotaFiscal.NomeCliente,
                        DataDoFaturamento = a.NotaFiscal.DataDoFaturamento,

                    })
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