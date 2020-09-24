using System;
using EasyNetQ;
using EventSourcing_Pagamento.Dominio.Eventos;
using Newtonsoft.Json;

namespace Teste
{
    class Program
    {
        static void Main(string[] args)
        {
            var mensageria = RabbitHutch.CreateBus("host=127.0.0.1");
            mensageria.Receive("PedidoPagamento", x => x.Add<string>(m =>
            {
                var mensagem = m;
                var testeDeConversao = JsonConvert.DeserializeObject<PedidoCriadoEvento>(mensagem);
                var x = 10;
            }));

            Console.ReadKey();
        }
    }
}