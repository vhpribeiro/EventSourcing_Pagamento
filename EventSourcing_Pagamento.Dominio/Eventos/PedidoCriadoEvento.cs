using EventSourcing_Pagamento.Dominio.Pedidos;

namespace EventSourcing_Pagamento.Dominio.Eventos
{
    public class PedidoCriadoEvento : Evento
    {   
        public PedidoCriadoEvento() {}
        
        public PedidoCriadoEvento(Pedido pedido) : base(pedido.Id, pedido.CartaoDeCredito.Nome, pedido.CartaoDeCredito.Numero, pedido.Produto, pedido.Valor)
        {
        }
    }
}