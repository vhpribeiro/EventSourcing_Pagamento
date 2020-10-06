using System;
using System.ComponentModel.DataAnnotations.Schema;
using EventSourcing_Pagamento.Dominio._Helpers;

namespace EventSourcing_Pagamento.Dominio.Pagamentos
{
    public class Pagamento
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NomeNoCartaoDeCredito { get; set; }
        public string NumeroDoCartaoDeCredito { get; set; }
        public int IdDoPedido { get; set; }
        public bool Aprovado { get; set; }
        public string BandeiraDoCartao { get; set; }
        public DateTime  DataDoPagamento {get; set; }
        
        public Pagamento() {}

        public Pagamento(int idDoPedido, string numeroDoCartaoDeCredito, string nomeNoCartaoDeCredito)
        {
            IdDoPedido = idDoPedido;
            NumeroDoCartaoDeCredito = numeroDoCartaoDeCredito;
            NomeNoCartaoDeCredito = nomeNoCartaoDeCredito;
            DataDoPagamento = DateTime.Now;
        }

        public void Aprovar(string bandeiraDoCartaoEsperada)
        {
            Aprovado = true;
            BandeiraDoCartao = bandeiraDoCartaoEsperada;
        }
        
        public void Negar() 
            => Aprovado = false;

        public void AlterarCartaoDeCredito(string numeroDoNovoCartaoDeCredito, string nomeNoCartaoDeCredito)
        {
            Validacoes<Pagamento>.Criar()
                .Obrigando(numeroDoNovoCartaoDeCredito, "É necessário informar o número do cartão de crédito")
                .Obrigando(nomeNoCartaoDeCredito, "É necessário informar o nome registrado no cartão de crédito")
                .DispararSeHouverErros();

            NumeroDoCartaoDeCredito = numeroDoNovoCartaoDeCredito;
            NomeNoCartaoDeCredito = nomeNoCartaoDeCredito;
        }
    }
}