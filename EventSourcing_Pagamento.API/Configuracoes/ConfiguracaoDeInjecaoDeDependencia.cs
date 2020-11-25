using EasyNetQ;
using EventSourcing_Pagamento.Aplicacao;
using EventSourcing_Pagamento.Aplicacao.InterfacesDeRepositorio;
using EventSourcing_Pagamento.Aplicacao.Pagamentos;
using EventSourcing_Pagamento.Infra.Contexts;
using EventSourcing_Pagamento.Infra.Repositorios;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcing_Pagamento.API.Configuracoes
{
    public class ConfiguracaoDeInjecaoDeDependencia
    {
        public static void Configurar(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProcessamentoDePagamento, ProcessamentoDePagamento>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBus>(x => RabbitHutch.CreateBus(configuration.GetValue<string>("RabbitConnection")));
            services.AddScoped<IPagamentoRepositorio, PagamentoRepositorio>();
        }    
    }
}