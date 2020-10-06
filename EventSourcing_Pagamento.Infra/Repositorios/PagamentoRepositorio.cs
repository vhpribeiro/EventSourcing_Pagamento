using System.Threading.Tasks;
using EventSourcing_Pagamento.Aplicacao.InterfacesDeRepositorio;
using EventSourcing_Pagamento.Dominio.Pagamentos;
using EventSourcing_Pagamento.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
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
            var contexto = ObterEscopo();
            await contexto.Pagamentos.AddAsync(pagamento);
            await contexto.SaveChangesAsync();
        }

        public async Task<Pagamento> ObterPeloIdDoPedido(int idDoPedido)
        {
            var contexto = ObterEscopo();
            return await contexto.Pagamentos.FirstOrDefaultAsync(p => p.IdDoPedido == idDoPedido);
        }

        public void AtualizarPagamento(Pagamento pagamento)
        {
            var contexto = ObterEscopo();
            contexto.Pagamentos.Update(pagamento);
        }
        
        private PagamentoContext ObterEscopo()
        {
            using var scope = _scopeFactory.CreateScope();
            var contexto = scope.ServiceProvider.GetRequiredService<PagamentoContext>();
            return contexto;
        }
    }
}