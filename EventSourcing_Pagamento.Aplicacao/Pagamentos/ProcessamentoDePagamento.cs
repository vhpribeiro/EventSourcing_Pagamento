using System.Threading.Tasks;
using CreditCardValidator;
using EasyNetQ;
using EventSourcing_Pagamento.Aplicacao.InterfacesDeRepositorio;
using EventSourcing_Pagamento.Dominio.Pagamentos;
using EventSourcingPedidoPagamento.Contratos.Eventos;

namespace EventSourcing_Pagamento.Aplicacao.Pagamentos
{
    public class ProcessamentoDePagamento : IProcessamentoDePagamento
    {
        private readonly IPagamentoRepositorio _pagamentoRepositorio;
        private readonly IBus _mensageria;

        public ProcessamentoDePagamento(IPagamentoRepositorio pagamentoRepositorio, IBus mensageria)
        {
            _pagamentoRepositorio = pagamentoRepositorio;
            _mensageria = mensageria;
        }
        
        public async Task ProcessarPagamentoAsync(PedidoCriadoEvento pedidoCriadoEvento)
        {
            var pagamento = new Pagamento(pedidoCriadoEvento.IdDoPedido, pedidoCriadoEvento.NumeroDoCartao,
                pedidoCriadoEvento.NomeDoUsuario);
            
            ValidarCartaoDeCredito(pagamento, pedidoCriadoEvento.Produto, pedidoCriadoEvento.Valor);

            await _pagamentoRepositorio.Salvar(pagamento);
        }
        
        public async Task ReprocessarPagamentoAsync(AlterouCartaoDeCreditoDoPedidoEvento alterouCartaoDeCreditoDoPedidoEvento)
        {
            var pagamento = await _pagamentoRepositorio.ObterPeloIdDoPedido(alterouCartaoDeCreditoDoPedidoEvento.IdDoPedido);
            
            pagamento.AlterarCartaoDeCredito(alterouCartaoDeCreditoDoPedidoEvento.NumeroDoCartao,
                alterouCartaoDeCreditoDoPedidoEvento.NomeDoUsuario);
            
            ValidarCartaoDeCredito(pagamento, alterouCartaoDeCreditoDoPedidoEvento.Produto, alterouCartaoDeCreditoDoPedidoEvento.Valor);

            _pagamentoRepositorio.AtualizarPagamento(pagamento);
        }

        private void ValidarCartaoDeCredito(Pagamento pagamento, string produto, in decimal valor)
        {
            var detector = new CreditCardDetector(pagamento.NumeroDoCartaoDeCredito);

            if (detector.IsValid())
            {
                pagamento.Aprovar(detector.BrandName);
                
                var pagamentoAprovadoEvento = new PagamentoAprovadoEvento(pagamento.IdDoPedido,
                    pagamento.NomeNoCartaoDeCredito, pagamento.NumeroDoCartaoDeCredito, produto, valor);
                _mensageria.PublishAsync(pagamentoAprovadoEvento);
            }

            else
            {
                pagamento.Negar();
                
                var pagamentoRecusadoEvento = new PagamentoRecusadoEvento(pagamento.IdDoPedido,
                    pagamento.NomeNoCartaoDeCredito, pagamento.NumeroDoCartaoDeCredito, produto, valor);
                _mensageria.PublishAsync(pagamentoRecusadoEvento);
            }
                
        }
    }
}