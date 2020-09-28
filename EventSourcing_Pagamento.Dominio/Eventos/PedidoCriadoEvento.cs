
namespace EventSourcing_Pagamento.Dominio.Eventos
{
    public class PedidoCriadoEvento : Evento
    {   
        public PedidoCriadoEvento() {}
        
        public PedidoCriadoEvento(int id, string nomeDoCartao, string numeroDoCartao, string produto, decimal valor) 
            : base(id, nomeDoCartao, numeroDoCartao, produto, valor)
        {
        }
    }
}