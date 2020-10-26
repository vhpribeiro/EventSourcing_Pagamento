using EventSourcing_Pagamento.Dominio.Pagamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventSourcing_Pagamento.Infra.Contexts
{
    public class PagamentoContext : DbContext
    {
        private readonly IConfiguration _configuration;
        
        public PagamentoContext() { }
        
        public PagamentoContext(DbContextOptions<PagamentoContext> opcoes, IConfiguration configuration)
            : base(opcoes)
        {
            _configuration = configuration;
        }
        
        public DbSet<Pagamento> Pagamentos { get; set; }

    }
}