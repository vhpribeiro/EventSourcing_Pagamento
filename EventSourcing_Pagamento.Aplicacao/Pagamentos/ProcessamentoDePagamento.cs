using System;
using System.Threading;
using System.Threading.Tasks;
using CreditCardValidator;
using EasyNetQ;
using EventSourcing_Pagamento.Dominio.Eventos;
using EventSourcing_Pagamento.Dominio.Pagamentos;
using EventSourcing_Pagamento.Dominio.Pedidos;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace EventSourcing_Pagamento.Aplicacao.Pagamentos
{
    public class ProcessamentoDePagamento : IProcessamentoDePagamento
    {
        public async Task ProcessarPagamentoAsync(PedidoCriadoEvento pedidoCriadoEvento)
        {
            var pagamento = new Pagamento(pedidoCriadoEvento.IdDoPedido, pedidoCriadoEvento.NumeroDoCartao,
                pedidoCriadoEvento.NomeDoUsuario);
            var detector = new CreditCardDetector(pagamento.NumeroDoCartaoDeCredito);
            
            if (detector.IsValid())
                pagamento.Aprovar(detector.BrandName);
            else
                pagamento.Negar();
        }
    }
}