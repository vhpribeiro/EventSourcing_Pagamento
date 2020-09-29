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
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO jogar essa connection string no appsettings
            optionsBuilder.UseSqlServer("Password=vhpr1706;Persist Security Info=True;User ID=sa;Initial Catalog=EventSourcingPagamento;Data Source=DESKTOP-NEOJFCR\\MSSQLSERVER2019",
                b => b.MigrationsAssembly("EventSourcing_Pagamento.Infra"));
        }

    }
}