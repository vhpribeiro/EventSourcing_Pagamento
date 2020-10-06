using System.Threading.Tasks;
using CreditCardValidator;
using EventSourcing_Pagamento.Aplicacao.InterfacesDeRepositorio;
using EventSourcing_Pagamento.Dominio.Eventos;
using EventSourcing_Pagamento.Dominio.Pagamentos;

namespace EventSourcing_Pagamento.Aplicacao.Pagamentos
{
    public class ProcessamentoDePagamento : IProcessamentoDePagamento
    {
        private readonly IPagamentoRepositorio _pagamentoRepositorio;

        public ProcessamentoDePagamento(IPagamentoRepositorio pagamentoRepositorio)
        {
            _pagamentoRepositorio = pagamentoRepositorio;
        }
        
        public async Task ProcessarPagamentoAsync(PedidoCriadoEvento pedidoCriadoEvento)
        {
            var pagamento = new Pagamento(pedidoCriadoEvento.IdDoPedido, pedidoCriadoEvento.NumeroDoCartao,
                pedidoCriadoEvento.NomeDoUsuario);
            
            ValidarCartaoDeCredito(pagamento);

            await _pagamentoRepositorio.Salvar(pagamento);
        }
        
        public async Task ReprocessarPagamentoAsync(AlterouCartaoDeCreditoDoPedidoEvento alterouCartaoDeCreditoDoPedidoEvento)
        {
            var pagamento = await _pagamentoRepositorio.ObterPeloIdDoPedido(alterouCartaoDeCreditoDoPedidoEvento.IdDoPedido);
            
            pagamento.AlterarCartaoDeCredito(alterouCartaoDeCreditoDoPedidoEvento.NumeroDoCartao,
                alterouCartaoDeCreditoDoPedidoEvento.NomeDoUsuario);
            
            ValidarCartaoDeCredito(pagamento);

            _pagamentoRepositorio.AtualizarPagamento(pagamento);
        }

        private static void ValidarCartaoDeCredito(Pagamento pagamento)
        {
            var detector = new CreditCardDetector(pagamento.NumeroDoCartaoDeCredito);

            if (detector.IsValid())
                pagamento.Aprovar(detector.BrandName);
            else
                pagamento.Negar();
        }
    }
}