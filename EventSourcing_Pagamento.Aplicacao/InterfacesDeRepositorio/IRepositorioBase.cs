using EventSourcing_Pagamento.Dominio._Helpers;

namespace EventSourcing_Pagamento.Aplicacao.InterfacesDeRepositorio
{
    public interface IRepositorioBase<TEntidade> where TEntidade : Entidade
    {
        void Adicionar(TEntidade entity);
    }
}