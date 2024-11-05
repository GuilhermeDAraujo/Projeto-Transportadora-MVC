using System.ComponentModel.DataAnnotations;

namespace Projeto_Transportadora_MVC.Enums
{
    public enum TipoAcao
    {
        [Display(Name = "Saída para Entrega")] SaidaParaEntrega = 1, 
        [Display(Name = "Entregue")] Entregue = 2, 
        [Display(Name = "Entrega Não Realizada")] EntregaNaoRealizada = 3, 
        [Display(Name = "Retirar no Cliente")] RetirarNoCliente = 4, 
        [Display(Name = "Devolução para Matriz")] DevolucaoParaMatriz = 5, 
        [Display(Name = "Aguardando Ação")] AgendarEntrega = 6,
        [Display(Name = "Saída no Fechamento")] SaidaNoFechamento = 7, 
        [Display(Name = "Aguardando Ação")] AguardandoAcao = 8
    }

    public enum StatusAgendamento
    {
        Agendado = 1,
        Entregue = 2,
        NaoEntregue = 3,
        Reagendado = 4
    }
}