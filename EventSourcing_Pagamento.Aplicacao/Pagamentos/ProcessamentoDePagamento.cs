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
            var detector = new CreditCardDetector(pagamento.NumeroDoCartaoDeCredito);
            
            if (detector.IsValid())
                pagamento.Aprovar(detector.BrandName);
            else
                pagamento.Negar();

            await _pagamentoRepositorio.Salvar(pagamento);
        }
    }
}