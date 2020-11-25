using System.Threading.Tasks;
using EventSourcing_Pagamento.Aplicacao.InterfacesDeRepositorio;
using EventSourcing_Pagamento.Dominio.Pagamentos;
using EventSourcing_Pagamento.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing_Pagamento.Infra.Repositorios
{
    public class PagamentoRepositorio : RepositorioBase<Pagamento>, IPagamentoRepositorio
    {
        private readonly PagamentoContext _pagamentoContext;

        public PagamentoRepositorio(PagamentoContext pagamentoContext) : base(pagamentoContext)
        {
            _pagamentoContext = pagamentoContext;
        }

        public async Task<Pagamento> ObterPeloIdDoPedido(int idDoPedido)
        {
            return await _pagamentoContext.Pagamentos.FirstOrDefaultAsync(p => p.IdDoPedido == idDoPedido);
        }

        public void AtualizarPagamento(Pagamento pagamento)
        {
            _pagamentoContext.Pagamentos.Update(pagamento);
        }
    }
}