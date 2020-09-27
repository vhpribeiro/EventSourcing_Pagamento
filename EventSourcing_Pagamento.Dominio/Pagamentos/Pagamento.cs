using System;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}