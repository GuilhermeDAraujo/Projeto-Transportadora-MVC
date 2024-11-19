using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto_Transportadora_MVC.Enums;
using Projeto_Transportadora_MVC.Models;
using Projeto_Transportadora_MVC.Services;

namespace Projeto_Transportadora_MVC.Controllers
{
    public class AcaoNotaFiscalController : Controller
    {
        private readonly AcaoNotaFiscalService _acaoNotaFiscalService;

        public AcaoNotaFiscalController(AcaoNotaFiscalService acaoNotaFiscalService)
        {
            _acaoNotaFiscalService = acaoNotaFiscalService;
        }

        public IActionResult Menu()
        {
            return View();
        }

        public async Task<IActionResult> Filtrar()
        {
            var notasNaoFiscaisFechadas = await _acaoNotaFiscalService.ObterNotasNaoFechadasAsync();
            await CarregarViewBag();
            return View(notasNaoFiscaisFechadas);
        }

        public async Task<IActionResult> FiltrarAcoesFiscais(TipoAcao? tipoAcao, DateTime? dataFaturamento)
        {
            IEnumerable<AcaoNotaFiscal> notas;

            if(tipoAcao.HasValue || dataFaturamento.HasValue)
            {
                notas = await _acaoNotaFiscalService.ObterNotasPorTipoAcaoEDataFaturamentoAsync(tipoAcao, dataFaturamento);
            }
            else
            {
                notas = await _acaoNotaFiscalService.ObterNotasNaoFechadasAsync();
            }

            await CarregarViewBag();
            return View("Filtrar", notas);
        }


        public async Task CarregarViewBag()
        {
            ViewBag.TipoAcao = new SelectList(_acaoNotaFiscalService.BuscarPorStatus(), "Value", "Text");
            ViewBag.Caminhao = new SelectList(await _acaoNotaFiscalService.BuscarCaminhao(), "Placa", "Placa");
        }
    }
}