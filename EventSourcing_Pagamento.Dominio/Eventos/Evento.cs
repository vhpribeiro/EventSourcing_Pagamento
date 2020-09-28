using System;

namespace EventSourcing_Pagamento.Dominio.Eventos
{
    public abstract class Evento
    {
        public int Id { get; set; }
        public string NomeDoUsuario { get; }
        public DateTime Data { get; }
        public decimal Valor { get; }
        public string Produto { get; }
        public string NumeroDoCartao { get; }
        public int IdDoPedido { get; }

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