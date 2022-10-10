using MediatR;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Tests.ASanitization
{
    public class ASanitizeTest<T, TType>
        where T : IRequest<TType>
    {
        private readonly T _objToSanitize;
        private readonly List<IASanitizeRuleDefinition> _rules;
        public IMediator Mediator { get; set; }

        public List<string> ErrorMessages { get; set; }
        public List<string> SuccessMessages { get; set; }

        private int _totalErrors = 0;
        public int TotalErrors
        {
            get
            {
                return _totalErrors;
            }
        }

        public ASanitizeTest(T objToSanitize)
        {
            _objToSanitize = objToSanitize;
            _rules = new List<IASanitizeRuleDefinition>();

            ErrorMessages = new List<string>();
            SuccessMessages = new List<string>();
        }

        public ASanitizeRuleDefinition<T, TType, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            System.Diagnostics.Debug.WriteLine(expression);

            var ab = new ASanitizeRuleDefinition<T, TType, TProperty>(_objToSanitize, expression);
            ab.Mediator = Mediator;

            _rules.Add(ab);

            return ab;
        }

        public async Task RunTest()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("******************* RESULTADO DE LA PRUEBA *******************");
            sb.AppendLine();
            sb.AppendLine();

            foreach (var r in _rules)
            {
                await r.Run();
                sb.AppendLine(r.MessageResult);

                if (!r.Succcess)
                {
                    _totalErrors++;
                    ErrorMessages.Add(r.MessageResult);
                }
                else
                {
                    SuccessMessages.Add(r.MessageResult);
                }
            }

            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("******************* FIN DE LA PRUEBA *******************");

            System.Diagnostics.Trace.WriteLine(sb.ToString());
        }
    }

    public class ASanitizeRuleExecutor
    {

    }

    public enum RuleType
    {
        NotEmpty,
        Empty,
        MinLength,
        MaxLength,
        Equal,
        EmailAddress,
        GreaterThan,
        GreaterThanOrEqualTo
    }
}
