namespace Projeto_Transportadora_MVC.Enums
{
    public enum TipoAcao
    {
        SaidaParaEntrega = 1,
        Entregue = 2,
        EntregaNaoRealizada = 3,
        RetirarNoCliente = 4,
        DevolucaoParaMatriz = 5,
        SaidaNoFechamento = 6,
        AguardandoAcao = 7
    }

    public enum StatusAgendamento
    {
        Agendado = 1,
        Entregue = 2,
        NãoEntregue = 3,
        Reagendado = 4
    }
}