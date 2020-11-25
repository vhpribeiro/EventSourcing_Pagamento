using System.Threading.Tasks;
using EventSourcing_Pagamento.Aplicacao;

namespace EventSourcing_Pagamento.Infra.Contexts
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PagamentoContext _pagamentoContext;

        public UnitOfWork(PagamentoContext pagamentoContext)
        {
            _pagamentoContext = pagamentoContext;
        }

        public async Task Commit()
        {
            await _pagamentoContext.SaveChangesAsync();
        }
    }
}