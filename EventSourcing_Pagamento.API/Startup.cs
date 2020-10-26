using EasyNetQ;
using EventSourcing_Pagamento.API.BackgroundServices;
using EventSourcing_Pagamento.API.Configuracoes;
using EventSourcing_Pagamento.Aplicacao.Pagamentos;
using EventSourcing_Pagamento.Infra.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventSourcing_Pagamento.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfiguracaoDeInjecaoDeDependencia.Configurar(services, _configuration);
            services.AddControllers();
            services.AddDbContext<PagamentoContext>(options => 
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("EventSourcing_Pagamento.Infra")));
            services.AddHostedService<RabbitMqSubscriber>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}