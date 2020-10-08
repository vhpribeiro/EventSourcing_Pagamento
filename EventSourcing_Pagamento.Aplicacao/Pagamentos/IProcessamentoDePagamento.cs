using System.Threading.Tasks;
using EventSourcingPedidoPagamento.Contratos.Eventos;

namespace EventSourcing_Pagamento.Aplicacao.Pagamentos
{
    public interface IProcessamentoDePagamento
    {
        Task ProcessarPagamentoAsync(PedidoCriadoEvento pedidoCriadoEvento);
        Task ReprocessarPagamentoAsync(AlterouCartaoDeCreditoDoPedidoEvento alterouCartaoDeCreditoDoPedidoEvento);
    }
}