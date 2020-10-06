using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EventSourcing_Pagamento.Aplicacao.Pagamentos;
using EventSourcing_Pagamento.Dominio.Eventos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace EventSourcing_Pagamento.API.BackgroundServices.Handlers
{
    public class AlterouCartaoDeCreditoDoPedidoEventoHandler : BackgroundService
    {
        private readonly IBus _mensageria;
        private readonly string _nomeDaQueue;
        private readonly IProcessamentoDePagamento _processamentoDePagamento;
        
        public AlterouCartaoDeCreditoDoPedidoEventoHandler(IBus mensageria, IConfiguration configuration, IProcessamentoDePagamento processamentoDePagamento)
        {
            _mensageria = mensageria;
            _nomeDaQueue = configuration.GetValue<string>("RabbitQueue");
            _processamentoDePagamento = processamentoDePagamento;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _mensageria.Receive(_nomeDaQueue, x =>
                    x.Add<string>(mensagem =>
                    {
                        var alterouCartaoDeCreditoDoPedidoEvento = JsonConvert.DeserializeObject<AlterouCartaoDeCreditoDoPedidoEvento>(mensagem);
                        _processamentoDePagamento.ReprocessarPagamentoAsync(alterouCartaoDeCreditoDoPedidoEvento);
                    }));
            }
        }
    }
}