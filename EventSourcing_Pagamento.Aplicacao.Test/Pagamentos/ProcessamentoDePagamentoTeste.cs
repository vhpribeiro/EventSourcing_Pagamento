using System.Threading.Tasks;
using Bogus;
using EasyNetQ;
using EventSourcing_Pagamento.Aplicacao.InterfacesDeRepositorio;
using EventSourcing_Pagamento.Aplicacao.Pagamentos;
using EventSourcing_Pagamento.Dominio.Pagamentos;
using EventSourcing_Pagamento.Test.Helpers._Builders.Dominio;
using EventSourcingPedidoPagamento.Contratos.Eventos;
using Moq;
using Xunit;

namespace EventSourcing_Pagamento.Aplicacao.Test.Pagamentos
{
    public class ProcessamentoDePagamentoTeste
    {
        private readonly Mock<IPagamentoRepositorio> _pagamentoRepositorio;
        private readonly Mock<IBus> _mensageria;
        private readonly ProcessamentoDePagamento _processamentoDePagamento;
        private readonly Faker _faker;

        public ProcessamentoDePagamentoTeste()
        {
            _faker = new Faker();
            _pagamentoRepositorio = new Mock<IPagamentoRepositorio>();
            _mensageria = new Mock<IBus>();
            _processamentoDePagamento = new ProcessamentoDePagamento(_pagamentoRepositorio.Object, _mensageria.Object);
        }
        
        [Theory]
        [InlineData("348343749287434")]
        [InlineData("6771896725800366")]
        [InlineData("347143835400458")]
        public async Task Deve_aprovar_o_pagamento_de_um_cartao_valido_ao_processar_um_pagamento(string numeroDeCartaoDeCreditoValido)
        {
            var id = _faker.Random.Int(0);
            var nomeDoCartao = _faker.Person.FullName;
            var produto = _faker.Random.String2(9);
            var valor = _faker.Random.Decimal();
            var pedidoCriadoEvento = new PedidoCriadoEvento(id, nomeDoCartao, numeroDeCartaoDeCreditoValido, produto, valor);
            _pagamentoRepositorio.Setup(pr => pr.Salvar(It.IsAny<Pagamento>()));
            _mensageria.Setup(m => m.PublishAsync(It.IsAny<PagamentoAprovadoEvento>()));
            
            await _processamentoDePagamento.ProcessarPagamentoAsync(pedidoCriadoEvento);
            
            _pagamentoRepositorio.Verify(pr => pr.Salvar(It.Is<Pagamento>(p => p.Aprovado)));
        }
        
        [Theory]
        [InlineData("348343749287434")]
        [InlineData("6771896725800366")]
        [InlineData("347143835400458")]
        public async Task Deve_enviar_evento_de_pagamento_aprovado_quando_aprovar_o_pagamento_de_um_cartao_ao_processar_um_pagamento(string numeroDeCartaoDeCreditoValido)
        {
            var id = _faker.Random.Int(0);
            var nomeDoCartao = _faker.Person.FullName;
            var produto = _faker.Random.String2(9);
            var valor = _faker.Random.Decimal();
            var pedidoCriadoEvento = new PedidoCriadoEvento(id, nomeDoCartao, numeroDeCartaoDeCreditoValido, produto, valor);
            _pagamentoRepositorio.Setup(pr => pr.Salvar(It.IsAny<Pagamento>()));
            _mensageria.Setup(m => m.PublishAsync(It.IsAny<PagamentoAprovadoEvento>()));
            
            await _processamentoDePagamento.ProcessarPagamentoAsync(pedidoCriadoEvento);
            
            _mensageria.Verify(m => m.PublishAsync(It.Is<PagamentoAprovadoEvento>(p => p.IdDoPedido == id)));
        }
        
        [Theory]
        [InlineData("349287434")]
        [InlineData("677186")]
        [InlineData("34754567")]
        public async Task Deve_negar_o_pagamento_de_um_cartao_invalido_ao_processar_um_pagamento(string numeroDeCartaoDeCreditoInvalido)
        {
            var id = _faker.Random.Int(0);
            var nomeDoCartao = _faker.Person.FullName;
            var produto = _faker.Random.String2(9);
            var valor = _faker.Random.Decimal();
            var pedidoCriadoEvento = new PedidoCriadoEvento(id, nomeDoCartao, numeroDeCartaoDeCreditoInvalido, produto, valor);
            _pagamentoRepositorio.Setup(pr => pr.Salvar(It.IsAny<Pagamento>()));
            _mensageria.Setup(m => m.PublishAsync(It.IsAny<PagamentoRecusadoEvento>()));
            
            await _processamentoDePagamento.ProcessarPagamentoAsync(pedidoCriadoEvento);
            
            _pagamentoRepositorio.Verify(pr 
                => pr.Salvar(It.Is<Pagamento>(p => p.Aprovado == false)));
        }
        
        [Theory]
        [InlineData("349287434")]
        [InlineData("677186")]
        [InlineData("34754567")]
        public async Task Deve_enviar_evento_de_pagamento_recusado_quando_negar_o_pagamento_de_um_cartao_ao_processar_um_pagamento(string numeroDeCartaoDeCreditoValido)
        {
            var id = _faker.Random.Int(0);
            var nomeDoCartao = _faker.Person.FullName;
            var produto = _faker.Random.String2(9);
            var valor = _faker.Random.Decimal();
            var pedidoCriadoEvento = new PedidoCriadoEvento(id, nomeDoCartao, numeroDeCartaoDeCreditoValido, produto, valor);
            _pagamentoRepositorio.Setup(pr => pr.Salvar(It.IsAny<Pagamento>()));
            _mensageria.Setup(m => m.PublishAsync(It.IsAny<PagamentoRecusadoEvento>()));
            
            await _processamentoDePagamento.ProcessarPagamentoAsync(pedidoCriadoEvento);
            
            _mensageria.Verify(m => m.PublishAsync(It.Is<PagamentoRecusadoEvento>(p => p.IdDoPedido == id)));
        }

        [Fact]
        public async Task Deve_atualizar_informacoes_do_cartao_de_credito_ao_reprocessar_pagamento()
        {
            var id = _faker.Random.Int(0);
            var nomeDoCartao = _faker.Person.FullName;
            var numeroDeCartao = _faker.Random.Int(0).ToString();
            var produto = _faker.Random.String2(9);
            var valor = _faker.Random.Decimal();
            var alterouCartaoDeCreditoDoPedidoEvento = new AlterouCartaoDeCreditoDoPedidoEvento(id, nomeDoCartao, numeroDeCartao, produto, valor);
            var pagamento = PagamentoBuilder.Novo().Criar();
            _pagamentoRepositorio.Setup(pr => pr.ObterPeloIdDoPedido(id)).ReturnsAsync(pagamento);
            _pagamentoRepositorio.Setup(pr => pr.AtualizarPagamento(It.IsAny<Pagamento>()));
            
            await _processamentoDePagamento.ReprocessarPagamentoAsync(alterouCartaoDeCreditoDoPedidoEvento);
            
            _pagamentoRepositorio.Verify(pr 
                => pr.AtualizarPagamento(It.Is<Pagamento>(p 
                    => p.NumeroDoCartaoDeCredito == numeroDeCartao && p.NomeNoCartaoDeCredito == nomeDoCartao)));
        }
        
        [Theory]
        [InlineData("348343749287434")]
        [InlineData("6771896725800366")]
        [InlineData("347143835400458")]
        public async Task Deve_aprovar_o_pagamento_de_um_cartao_valido_ao_reprocessar_um_pagamento(string numeroDeCartaoDeCreditoValido)
        {
            var id = _faker.Random.Int(0);
            var nomeDoCartao = _faker.Person.FullName;
            var produto = _faker.Random.String2(9);
            var valor = _faker.Random.Decimal();
            var alterouCartaoDeCreditoDoPedidoEvento = new AlterouCartaoDeCreditoDoPedidoEvento(id, nomeDoCartao, numeroDeCartaoDeCreditoValido, produto, valor);
            var pagamento = PagamentoBuilder.Novo().Criar();
            _pagamentoRepositorio.Setup(pr => pr.ObterPeloIdDoPedido(id)).ReturnsAsync(pagamento);
            _pagamentoRepositorio.Setup(pr => pr.AtualizarPagamento(It.IsAny<Pagamento>()));
            _mensageria.Setup(m => m.PublishAsync(It.IsAny<PagamentoAprovadoEvento>()));
            
            await _processamentoDePagamento.ReprocessarPagamentoAsync(alterouCartaoDeCreditoDoPedidoEvento);
            
            _pagamentoRepositorio.Verify(pr 
                => pr.AtualizarPagamento(It.Is<Pagamento>(p 
                    => p.Aprovado && p.NumeroDoCartaoDeCredito == numeroDeCartaoDeCreditoValido && p.NomeNoCartaoDeCredito == nomeDoCartao)));
        }
        
        [Theory]
        [InlineData("348343749287434")]
        [InlineData("6771896725800366")]
        [InlineData("347143835400458")]
        public async Task Deve_enviar_evento_de_pagamento_aprovado_ao_aprovar_o_pagamento_de_um_cartao_no_reprocessamento_de_um_pagamento(string numeroDeCartaoDeCreditoInvalido)
        {
            var id = _faker.Random.Int(0);
            var nomeDoCartao = _faker.Person.FullName;
            var produto = _faker.Random.String2(9);
            var valor = _faker.Random.Decimal();
            var alterouCartaoDeCreditoDoPedidoEvento = new AlterouCartaoDeCreditoDoPedidoEvento(id, nomeDoCartao, numeroDeCartaoDeCreditoInvalido, produto, valor);
            var pagamento = PagamentoBuilder.Novo().Criar();
            _pagamentoRepositorio.Setup(pr => pr.ObterPeloIdDoPedido(id)).ReturnsAsync(pagamento);
            _pagamentoRepositorio.Setup(pr => pr.AtualizarPagamento(It.IsAny<Pagamento>()));
            _mensageria.Setup(m => m.PublishAsync(It.IsAny<PagamentoAprovadoEvento>()));
            
            await _processamentoDePagamento.ReprocessarPagamentoAsync(alterouCartaoDeCreditoDoPedidoEvento);
            
            _mensageria.Verify(m => m.PublishAsync(It.Is<PagamentoAprovadoEvento>(p => p.IdDoPedido == pagamento.IdDoPedido)));
        }
        
        [Theory]
        [InlineData("349287434")]
        [InlineData("677186")]
        [InlineData("34754567")]
        public async Task Deve_negar_o_pagamento_de_um_cartao_invalido_ao_reprocessar_um_pagamento(string numeroDeCartaoDeCreditoInvalido)
        {
            var id = _faker.Random.Int(0);
            var nomeDoCartao = _faker.Person.FullName;
            var produto = _faker.Random.String2(9);
            var valor = _faker.Random.Decimal();
            var alterouCartaoDeCreditoDoPedidoEvento = new AlterouCartaoDeCreditoDoPedidoEvento(id, nomeDoCartao, numeroDeCartaoDeCreditoInvalido, produto, valor);
            var pagamento = PagamentoBuilder.Novo().Criar();
            _pagamentoRepositorio.Setup(pr => pr.ObterPeloIdDoPedido(id)).ReturnsAsync(pagamento);
            _pagamentoRepositorio.Setup(pr => pr.AtualizarPagamento(It.IsAny<Pagamento>()));
            _mensageria.Setup(m => m.PublishAsync(It.IsAny<PagamentoRecusadoEvento>()));
            
            await _processamentoDePagamento.ReprocessarPagamentoAsync(alterouCartaoDeCreditoDoPedidoEvento);
            
            _pagamentoRepositorio.Verify(pr 
                => pr.AtualizarPagamento(It.Is<Pagamento>(p 
                    => p.Aprovado == false && p.NumeroDoCartaoDeCredito == numeroDeCartaoDeCreditoInvalido && p.NomeNoCartaoDeCredito == nomeDoCartao)));
        }
        
        [Theory]
        [InlineData("349287434")]
        [InlineData("677186")]
        [InlineData("34754567")]
        public async Task Deve_enviar_evento_de_pagamento_negado_ao_negar_o_pagamento_de_um_cartao_no_reprocessamento_de_um_pagamento(string numeroDeCartaoDeCreditoInvalido)
        {
            var id = _faker.Random.Int(0);
            var nomeDoCartao = _faker.Person.FullName;
            var produto = _faker.Random.String2(9);
            var valor = _faker.Random.Decimal();
            var alterouCartaoDeCreditoDoPedidoEvento = new AlterouCartaoDeCreditoDoPedidoEvento(id, nomeDoCartao, numeroDeCartaoDeCreditoInvalido, produto, valor);
            var pagamento = PagamentoBuilder.Novo().Criar();
            _pagamentoRepositorio.Setup(pr => pr.ObterPeloIdDoPedido(id)).ReturnsAsync(pagamento);
            _pagamentoRepositorio.Setup(pr => pr.AtualizarPagamento(It.IsAny<Pagamento>()));
            _mensageria.Setup(m => m.PublishAsync(It.IsAny<PagamentoRecusadoEvento>()));
            
            await _processamentoDePagamento.ReprocessarPagamentoAsync(alterouCartaoDeCreditoDoPedidoEvento);
            
            _mensageria.Verify(m => m.PublishAsync(It.Is<PagamentoRecusadoEvento>(p => p.IdDoPedido == pagamento.IdDoPedido)));
        }
    }
}