using System.Threading.Tasks;
using EventSourcing_Pagamento.Dominio.Pagamentos;

namespace EventSourcing_Pagamento.Aplicacao.InterfacesDeRepositorio
{
    public interface IPagamentoRepositorio
    {
        Task Salvar(Pagamento pagamento);
    }
}