using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Index()
        {
            var notasNaoFiscaisFechadas = await _acaoNotaFiscalService.ObterNotasNaoFechadasAsync();
            await CarregarViewBag();
            return View(notasNaoFiscaisFechadas);
        }


        public async Task CarregarViewBag()
        {
            ViewBag.TipoAcao = new SelectList(_acaoNotaFiscalService.BuscarPorStatus(), "Value", "Text");
            ViewBag.Caminhao = new SelectList(await _acaoNotaFiscalService.BuscarCaminhao(), "Placa", "Placa");
        }
    }
}