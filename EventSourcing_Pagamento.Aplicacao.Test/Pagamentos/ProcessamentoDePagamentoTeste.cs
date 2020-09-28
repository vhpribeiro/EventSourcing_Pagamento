using Bogus;
using EventSourcing_Pagamento.Aplicacao.InterfacesDeRepositorio;
using EventSourcing_Pagamento.Aplicacao.Pagamentos;
using EventSourcing_Pagamento.Dominio.Eventos;
using EventSourcing_Pagamento.Dominio.Pagamentos;
using Moq;
using Xunit;

namespace EventSourcing_Pagamento.Aplicacao.Test.Pagamentos
{
    public class ProcessamentoDePagamentoTeste
    {
        private readonly Mock<IPagamentoRepositorio> _pagamentoRepositorio;
        private readonly ProcessamentoDePagamento _processamentoDePagamento;
        private readonly Faker _faker;

        public ProcessamentoDePagamentoTeste()
        {
            _faker = new Faker();
            _pagamentoRepositorio = new Mock<IPagamentoRepositorio>();
            _processamentoDePagamento = new ProcessamentoDePagamento(_pagamentoRepositorio.Object);
        }
        
        [Theory]
        [InlineData("348343749287434")]
        [InlineData("6771896725800366")]
        [InlineData("347143835400458")]
        public void Deve_aprovar_o_pagamento_de_um_cartao_invalido(string numeroDeCartaoValidos)
        {
            var id = _faker.Random.Int(0);
            var nomeDoCartao = _faker.Person.FullName;
            var produto = _faker.Random.String2(9);
            var valor = _faker.Random.Decimal();
            var teste = _faker.Finance.CreditCardNumber();
            var pedidoCriadoEvento = new PedidoCriadoEvento(id, nomeDoCartao, numeroDeCartaoValidos, produto, valor);
            
            _processamentoDePagamento.ProcessarPagamentoAsync(pedidoCriadoEvento);
            
            _pagamentoRepositorio.Verify(pr => pr.Salvar(It.Is<Pagamento>(p => p.Aprovado)));
        }
        
        [Fact]
        public void Deve_negar_o_pagamento_de_um_cartao_invalido()
        {
            var id = _faker.Random.Int(0);
            var nomeDoCartao = _faker.Person.FullName;
            var numeroDoCartao = _faker.Random.Int(0).ToString();
            var produto = _faker.Random.String2(9);
            var valor = _faker.Random.Decimal();
            var pedidoCriadoEvento = new PedidoCriadoEvento(id, nomeDoCartao, numeroDoCartao, produto, valor);
            
            _processamentoDePagamento.ProcessarPagamentoAsync(pedidoCriadoEvento);
            
            _pagamentoRepositorio.Verify(pr => pr.Salvar(It.Is<Pagamento>(p => p.Aprovado == false)));
        }
    }
}