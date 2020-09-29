using System.Threading.Tasks;
using EventSourcing_Pagamento.Aplicacao.InterfacesDeRepositorio;
using EventSourcing_Pagamento.Dominio.Pagamentos;
using EventSourcing_Pagamento.Infra.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcing_Pagamento.Infra.Repositorios
{
    public class PagamentoRepositorio : IPagamentoRepositorio
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PagamentoRepositorio(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        
        public async Task Salvar(Pagamento pagamento)
        {
            //TODO pensar em como melhorar isso
            using var scope = _scopeFactory.CreateScope();
            var contexto = scope.ServiceProvider.GetRequiredService<PagamentoContext>();
            await contexto.Pagamentos.AddAsync(pagamento);
            await contexto.SaveChangesAsync();
        }
    }
}