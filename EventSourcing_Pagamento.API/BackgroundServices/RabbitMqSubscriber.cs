using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EventSourcing_Pagamento.Aplicacao.Pagamentos;
using EventSourcingPedidoPagamento.Contratos.Eventos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventSourcing_Pagamento.API.BackgroundServices
{
    public class RabbitMqSubscriber : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMqSubscriber(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var escopo = _scopeFactory.CreateScope())
            {
                var mensageria = escopo.ServiceProvider.GetService<IBus>();
                var processamentoDePagamento = escopo.ServiceProvider.GetService<IProcessamentoDePagamento>();
                
                while (!stoppingToken.IsCancellationRequested)
                {
                    mensageria.Subscribe<PedidoCriadoEvento>("pedidoCriadoEvento", pedidoCriadoEvento =>
                    {
                        processamentoDePagamento.ProcessarPagamentoAsync(pedidoCriadoEvento);
                    });
                    mensageria.Subscribe<AlterouCartaoDeCreditoDoPedidoEvento>("alterouEvento", alterouCartaoDeCreditoDoPedidoEvento =>
                    {
                        processamentoDePagamento.ReprocessarPagamentoAsync(alterouCartaoDeCreditoDoPedidoEvento);
                    });
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            using (var escopo = _scopeFactory.CreateScope())
            {
                var mensageria = escopo.ServiceProvider.GetService<IBus>();
                mensageria.Dispose();
                return base.StopAsync(cancellationToken);
            }
        }
    }
}