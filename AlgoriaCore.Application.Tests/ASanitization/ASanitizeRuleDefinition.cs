
using AlgoriaCore.Application.Exceptions;
using AlgoriaCore.Domain.Exceptions.Abstracts;
using MediatR;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Tests.ASanitization
{
    public class ASanitizeRuleDefinition<T, TType, TProperty> : IASanitizeRuleDefinition
        where T : IRequest<TType>
    {
        protected T _target;
		private readonly Expression<Func<T, TProperty>> _expression;
        public RuleType RuleType { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public IMediator Mediator { get; set; }

        public bool Succcess { get; set; }
        public string MessageResult { get; set; }

        public T Target { get { return _target; } }

        public ASanitizeRuleDefinition(T target, Expression<Func<T, TProperty>> expression)
        {
            _target = target;
            _expression = expression;
        }

        public async Task Run()
        {
            bool resp = false;
            string propertyName = string.Empty;
            object testedValue = string.Empty;
            object currentValue = string.Empty;
            string messageResult;
            PropertyInfo member = null;

            var expected = GetExpected();

            try
            {
                member = GetPropertyInfo();
                propertyName = member.Name;

                Func<T, TProperty> x = _expression.Compile();
                currentValue = x.Invoke(_target);

                testedValue = GetValueForTest(member);

                // Setting test value
                member.SetValue(_target, testedValue);
                await Mediator.Send(_target);

                resp = true;

                messageResult = "Method executed with not exceptions.";
            }
            catch (ValidationException avEx)
            {
                resp = false;

                if (avEx.Failures.Values.Count > 1)
                {
                    // Si hay más de un error, entonces está fallando otra cosa en la validación
                    resp = true; // Por lo tanto se marca como "falla de la prueba"
                }

                var fails = string.Empty;
                foreach (var v in avEx.Failures.Values)
                {
                    fails += string.Join(",", v);
                }
                messageResult = avEx.Message + " " + fails;
            }
            catch (AlgoriaCoreException aex)
            {
                resp = !expected;
                messageResult = aex.Message + " (AlgoriaCoreException found. No ValidationException raised.)";
            }
            catch (Exception ex)
            {
                resp = !expected;
                messageResult = ex.Message + " (Exception found. No ValidationException raised.)";
            }

            // Devolver el valor original
            member.SetValue(_target, currentValue);

            string result = "SUCCESS";

            if (expected != resp)
            {
                Succcess = false;
                result = "FAILED";
            }
            else
            {
                Succcess = true;
            }

            MessageResult = string.Format("Validating '{0}' on property '{1}' with '{2}' value. Test '{3}'\n\t{4}\n", RuleType.ToString(), propertyName, testedValue, result, messageResult);
        }

        private PropertyInfo GetPropertyInfo()
        {
            MemberExpression member = _expression.Body as MemberExpression;

            PropertyInfo propInfo = member.Member as PropertyInfo;

            return propInfo;
        }

        private object GetValueForTest(PropertyInfo member)
        {
            object value = null;
            string dataStr;
            TypeCode typeCode;

            switch (RuleType)
            {
                case RuleType.Empty:
                    value = "TEST";
                    break;
                case RuleType.NotEmpty:
                    if (IsNumericType(member))
                    {
                        value = 0;
                    }
                    else
                    {
                        value = string.Empty;
                    }

                    break;
                case RuleType.MaxLength:
                    var l = Int64.Parse(Data.ToString());
                    l = l + 1;  // Se agrega un caracter de más para que la prueba falle

                    StringBuilder sb = new StringBuilder();
                    for (var i = 1; i <= l; i++)
                    {
                        sb.Append('A');
                    }

                    sb.AppendFormat("(más de {0} caracteres).", (l - 1));

                    value = sb.ToString();
                    break;
                case RuleType.MinLength:
                    var lm = Int64.Parse(Data.ToString());
                    lm = lm - 1;  // Se agrega un caracter de más para que la prueba falle

                    StringBuilder sb1 = new StringBuilder();
                    for (var i = 1; i <= lm; i++)
                    {
                        sb1.Append('A');
                    }
                    value = sb1.ToString();
                    break;
                case RuleType.EmailAddress:
                    value = "invalid_mail";
                    break;
                case RuleType.Equal:
                    value = Data.ToString();
                    break;
                case RuleType.GreaterThan:
                    dataStr = Data.ToString();
                    typeCode = Type.GetTypeCode(member.PropertyType);

                    if (typeCode == TypeCode.Object)
                    {
                        typeCode = Type.GetTypeCode(member.PropertyType.GenericTypeArguments[0]);
                    }

                    value = GetNumber(typeCode, dataStr);

                    break;
                case RuleType.GreaterThanOrEqualTo:
                    dataStr = Data.ToString();
                    typeCode = Type.GetTypeCode(member.PropertyType);

                    if (typeCode == TypeCode.Object)
                    {
                        typeCode = Type.GetTypeCode(member.PropertyType.GenericTypeArguments[0]);
                    }

                    value = GetNumber(typeCode, dataStr, -1);
                    break;
            }

            return value;
        }

        private bool GetExpected()
        {
            bool expect = false;
            switch (RuleType)
            {
                case RuleType.Empty:
                    expect = false;
                    break;
                case RuleType.NotEmpty:
                    expect = false;
                    break;
                case RuleType.MaxLength:
                    expect = false;
                    break;
                case RuleType.MinLength:
                    expect = false;
                    break;
                case RuleType.Equal:
                    expect = false;
                    break;
            }

            return expect;
        }

        public bool IsNumericType(PropertyInfo member)
        {
            switch (Type.GetTypeCode(member.PropertyType))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        private static object GetNumber(TypeCode typeCode, string dataStr, int suma = 0)
        {
            object value = 0;

            switch (typeCode)
            {
                case TypeCode.Byte:
                    value = (byte)(Convert.ToByte(dataStr) + Convert.ToByte(suma.ToString()));
                    break;
                case TypeCode.SByte:
                    value = (sbyte)(Convert.ToSByte(dataStr) + Convert.ToSByte(suma.ToString()));
                    break;
                case TypeCode.UInt16:
                    value = (ushort)(Convert.ToUInt16(dataStr) + Convert.ToUInt16(suma.ToString()));
                    break;
                case TypeCode.UInt32:
                    value = Convert.ToUInt32(dataStr) + Convert.ToUInt32(suma.ToString());
                    break;
                case TypeCode.UInt64:
                    value = Convert.ToUInt64(dataStr) + Convert.ToUInt64(suma.ToString());
                    break;
                case TypeCode.Int16:
                    value = (short)(Convert.ToInt16(dataStr) + Convert.ToInt16(suma.ToString()));
                    break;
                case TypeCode.Int32:
                    value = Convert.ToInt32(dataStr) + Convert.ToInt32(suma.ToString());
                    break;
                case TypeCode.Int64:
                    value = Convert.ToInt64(dataStr) + Convert.ToInt64(suma.ToString());
                    break;
                case TypeCode.Decimal:
                    value = Convert.ToDecimal(dataStr) + Convert.ToDecimal(suma.ToString());
                    break;
                case TypeCode.Double:
                    value = Convert.ToDouble(dataStr) + Convert.ToDouble(suma.ToString());
                    break;
                case TypeCode.Single:
                    value = Convert.ToSingle(dataStr) + Convert.ToSingle(suma.ToString());
                    break;
            }

            return value;
        }
    }
}

