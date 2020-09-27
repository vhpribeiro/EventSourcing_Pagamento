using Bogus;
using EventSourcing_Pagamento.Dominio.Pagamentos;
using EventSourcing_Pagamento.Test.Helpers._Builders.Dominio;
using ExpectedObjects;
using Xunit;

namespace EventSourcing_Pagamento.Dominio.Test.Pagamentos
{
    public class PagamentoTeste
    {
        private readonly Faker _faker;

        public PagamentoTeste()
        {
            _faker = new Faker();
        }
        
        [Fact]
        public void Deve_criar_um_pagamento()
        {
            var idDoPedido = _faker.Random.Int(0);
            var numeroDoCartaoDeCredito = _faker.Random.Int(1000000).ToString();
            var nomeNoCartaoDeCredito = _faker.Person.FullName;
            const string expiracaoDoCartaoDeCredito = "03/27";
            var pagamentoEsperado = new
            {
                IdDoPedido = idDoPedido,
                NumeroDoCartaoDeCredito = numeroDoCartaoDeCredito,
                NomeNoCartaoDeCredito = nomeNoCartaoDeCredito
            };
            
            var pagamentoObtido = new Pagamento(idDoPedido, numeroDoCartaoDeCredito, nomeNoCartaoDeCredito);
            
            pagamentoObtido.ToExpectedObject(ctx => ctx.Ignore(p => p.DataDoPagamento))
                .ShouldMatch(pagamentoEsperado);
        }

        [Fact]
        public void Deve_registrar_a_bandeira_do_cartao_ao_aprovar_pagamento()
        {
            const string bandeiraDoCartaoEsperada = "Master";
            var pagamento = PagamentoBuilder.Novo().Criar();
            
            pagamento.Aprovar(bandeiraDoCartaoEsperada);
            
            Assert.Equal(pagamento.BandeiraDoCartao, bandeiraDoCartaoEsperada);
        }

        [Fact]
        public void Deve_registrar_que_o_pagamento_foi_aprovado()
        {
            const string bandeiraDoCartaoEsperada = "Master";
            var pagamento = PagamentoBuilder.Novo().Criar();
            
            pagamento.Aprovar(bandeiraDoCartaoEsperada);
            
            Assert.True(pagamento.Aprovado);
        }
    }
}