using System;

namespace EventSourcing_Pagamento.Dominio._Helpers
{
    public static class ExtensoesDeDateTime
    {
        public static bool EhDataValida(this DateTime date)
        {
            var dataMinima = new DateTime(1900, 01, 01);
            return date > dataMinima;
        }
    }
}