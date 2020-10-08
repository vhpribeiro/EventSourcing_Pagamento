using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EventSourcing_Pagamento.Aplicacao.Pagamentos;
using EventSourcingPedidoPagamento.Contratos.Eventos;
using Microsoft.Extensions.Hosting;

namespace EventSourcing_Pagamento.API.BackgroundServices
{
    public class RabbitMqSubscriber : BackgroundService
    {
        private readonly IBus _mensageria;
        private readonly IProcessamentoDePagamento _processamentoDePagamento;

        public RabbitMqSubscriber(IBus mensageria, IProcessamentoDePagamento processamentoDePagamento)
        {
            _mensageria = mensageria;
            _processamentoDePagamento = processamentoDePagamento;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _mensageria.Subscribe<PedidoCriadoEvento>("pedidoCriadoEvento", pedidoCriadoEvento =>
                {
                    _processamentoDePagamento.ProcessarPagamentoAsync(pedidoCriadoEvento);
                });
                _mensageria.Subscribe<AlterouCartaoDeCreditoDoPedidoEvento>("alterouEvento", alterouCartaoDeCreditoDoPedidoEvento =>
                {
                    _processamentoDePagamento.ReprocessarPagamentoAsync(alterouCartaoDeCreditoDoPedidoEvento);
                });
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _mensageria.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}