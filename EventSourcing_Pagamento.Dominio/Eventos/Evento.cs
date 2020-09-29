using System;

namespace EventSourcing_Pagamento.Dominio.Eventos
{
    public abstract class Evento
    {
        public int Id { get; set; }
        public string NomeDoUsuario { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public string Produto { get; set; }
        public string NumeroDoCartao { get; set; }
        public int IdDoPedido { get; set; }

        public Evento() {}

        public Evento(int identificadorDoPedido, string nomeDoUsuairo, string numeroDoCartao, string produto, decimal valor)
        {
            IdDoPedido = identificadorDoPedido;
            Data = DateTime.Now;
            NomeDoUsuario = nomeDoUsuairo;
            NumeroDoCartao = numeroDoCartao;
            Produto = produto;
            Valor = valor;
        }
    }
}