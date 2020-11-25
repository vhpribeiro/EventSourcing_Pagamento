using EventSourcing_Pagamento.Aplicacao.InterfacesDeRepositorio;
using EventSourcing_Pagamento.Dominio._Helpers;
using EventSourcing_Pagamento.Infra.Contexts;

namespace EventSourcing_Pagamento.Infra.Repositorios
{
    public class RepositorioBase<TEntidade> : IRepositorioBase<TEntidade> where TEntidade : Entidade
    {
        protected PagamentoContext _pagamentoContext;

        public RepositorioBase(PagamentoContext pagamentoContext)
        {
            _pagamentoContext = pagamentoContext;
        }
        
        public void Adicionar(TEntidade entidade)
        {
            _pagamentoContext.Set<TEntidade>().Add(entidade);
        }
    }
}