using System.ComponentModel.DataAnnotations.Schema;
using EventSourcing_Pagamento.Dominio._Helpers;
using EventSourcing_Pagamento.Dominio.CartoesDeCredito;

namespace EventSourcing_Pagamento.Dominio.Pedidos
{
    public class Pedido
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public int CartaoDeCreditoId { get; set; }
        public CartaoDeCredito CartaoDeCredito { get; set; }

        public Pedido() {}

        public Pedido(int id, CartaoDeCredito cartaoDeCredito)
        {
            ValidarInformacoes(id, cartaoDeCredito);
            Id = id;
            CartaoDeCredito = cartaoDeCredito;
        }

        private static void ValidarInformacoes(int id, CartaoDeCredito cartaoDeCredito)
        {
            Validacoes<Pedido>.Criar()
                .Quando(id <= 0, "É necessário informar um id válido")
                .Obrigando(cartaoDeCredito, "É necessário informar um valor válido")
                .DispararSeHouverErros();
        }
    }
}