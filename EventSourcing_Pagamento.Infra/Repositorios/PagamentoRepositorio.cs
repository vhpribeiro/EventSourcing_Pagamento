using System.Threading.Tasks;
using EventSourcing_Pagamento.Aplicacao.InterfacesDeRepositorio;
using EventSourcing_Pagamento.Dominio.Pagamentos;
using EventSourcing_Pagamento.Infra.Contexts;

namespace EventSourcing_Pagamento.Infra.Repositorios
{
    public class PagamentoRepositorio : IPagamentoRepositorio
    {
        private readonly PagamentoContext _context;

        public PagamentoRepositorio(PagamentoContext context)
        {
            _context = context;
        }
        
        public async Task Salvar(Pagamento pagamento)
        {
            await _context.Pagamentos.AddAsync(pagamento);
            await _context.SaveChangesAsync();
        }
    }
}