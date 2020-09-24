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
            var pedido = JsonConvert.DeserializeObject<Pedido>(pedidoCriadoEvento.MetaDado);
            var pagamento = new Pagamento(pedido.Id, pedido.CartaoDeCredito.Numero, pedido.CartaoDeCredito.Expiracao, pedido.CartaoDeCredito.Nome);
            var detector = new CreditCardDetector(pedido.CartaoDeCredito.Numero);
            
            if (detector.IsValid())
                pagamento.Aprovar(detector.BrandName);
            else
                pagamento.Negar();
        }
    }
}