using Bogus;
using EventSourcing_Pagamento.Dominio.Pagamentos;

namespace EventSourcing_Pagamento.Test.Helpers._Builders.Dominio
{
    public class PagamentoBuilder
    {
        private static readonly Faker Faker = new Faker();
        private int _idDoPedido = Faker.Random.Int(0);
        private string _numeroDoCartaoDeCredito = Faker.Random.Int(1000000).ToString();
        private string _nomeNoCartaoDeCredito = Faker.Person.FullName;
        private string _expiracaoDoCartaoDeCredito = "03/207";

        public static PagamentoBuilder Novo() 
            => new PagamentoBuilder();

        public PagamentoBuilder ComIdDoPedido(int idDoPedido)
        {
            _idDoPedido = idDoPedido;
            return this;
        }

        public PagamentoBuilder ComNumeroDoCartaoDeCredito(string numeroDoCartaoDeCredito)
        {
            _numeroDoCartaoDeCredito = numeroDoCartaoDeCredito;
            return this;
        }

        public PagamentoBuilder ComNomeNoCartaoDeCredito(string nomeNoCartaoDeCredito)
        {
            _nomeNoCartaoDeCredito = nomeNoCartaoDeCredito;
            return this;
        }

        public PagamentoBuilder ComExpiracaoDoCartaoDeCredito(string expiracaoDoCartaoDeCredito)
        {
            _expiracaoDoCartaoDeCredito = expiracaoDoCartaoDeCredito;
            return this;
        }

        public Pagamento Criar() 
            => new Pagamento(_idDoPedido, _numeroDoCartaoDeCredito, _expiracaoDoCartaoDeCredito, _nomeNoCartaoDeCredito);
    }
}