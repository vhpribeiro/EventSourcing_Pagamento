using System.Threading.Tasks;
using EventSourcing_Pagamento.Dominio.Eventos;

namespace EventSourcing_Pagamento.Aplicacao.Pagamentos
{
    public interface IProcessamentoDePagamento
    {
        Task ProcessarPagamentoAsync(PedidoCriadoEvento pedidoCriadoEvento);
    }
}