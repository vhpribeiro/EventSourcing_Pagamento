using System.Threading.Tasks;

namespace EventSourcing_Pagamento.Aplicacao
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}