namespace EventSourcing_Pagamento.Dominio.Eventos
{
    public class AlterouCartaoDeCreditoDoPedidoEvento : Evento
    {
        public AlterouCartaoDeCreditoDoPedidoEvento(int id, string nomeDoCartao, string numeroDoCartao, string produto, decimal valor) 
            : base(id, nomeDoCartao, numeroDoCartao, produto, valor)
        {
        }
    }
}