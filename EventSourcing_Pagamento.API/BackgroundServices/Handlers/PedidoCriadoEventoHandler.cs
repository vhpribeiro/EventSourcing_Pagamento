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
    public class PedidoCriadoEventoHandler : BackgroundService
    {
        private readonly IBus _mensageria;
        private readonly string _nomeDaQueue;
        private readonly IProcessamentoDePagamento _processamentoDePagamento;

        public PedidoCriadoEventoHandler(IBus mensageria, IConfiguration configuration, IProcessamentoDePagamento processamentoDePagamento)
        {
            _mensageria = mensageria;
            _nomeDaQueue = configuration.GetValue<string>("RabbitQueue");
            _processamentoDePagamento = processamentoDePagamento;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _mensageria.Receive(_nomeDaQueue, x =>
                x.Add<string>(m =>
                {
                    var mensagem = m;
                    var testeDeConversao = JsonConvert.DeserializeObject<PedidoCriadoEvento>(mensagem);
                    _processamentoDePagamento.ProcessarPagamentoAsync(testeDeConversao);
                }));
        }
    }
}