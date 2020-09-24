using System;
using System.ComponentModel.DataAnnotations.Schema;
using EventSourcing_Pagamento.Dominio.Pedidos;
using Newtonsoft.Json;

namespace EventSourcing_Pagamento.Dominio.Eventos
{
    public abstract class Evento
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MetaDado { get; }
        public DateTime Data { get; }
        public int IdDoPedido { get; }

        public Evento() {}

        public Evento(int identificadorDoPedido, Pedido pedido)
        {
            IdDoPedido = identificadorDoPedido;
            Data = DateTime.Now;
            MetaDado = JsonConvert.SerializeObject(pedido);
        }
    }
}