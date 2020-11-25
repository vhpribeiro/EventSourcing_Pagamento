using System.Threading.Tasks;
using EventSourcing_Pagamento.Dominio.Pagamentos;

namespace EventSourcing_Pagamento.Aplicacao.InterfacesDeRepositorio
{
    public interface IPagamentoRepositorio
    {
        void Adicionar(Pagamento pagamento);
        Task<Pagamento> ObterPeloIdDoPedido(int idDoPedido);
        void AtualizarPagamento(Pagamento pagamento);
    }
}