using System.ComponentModel.DataAnnotations.Schema;

namespace EventSourcing_Pagamento.Dominio._Helpers
{
    public abstract class Entidade
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}