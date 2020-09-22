using System.Linq.Expressions;

namespace EventSourcing_Pagamento.Dominio._Helpers
{
    public class ViolacaoDeRegra
    {
        public LambdaExpression Propriedade { get; internal set; }
        public string Mensagem { get; internal set; }
    }
}