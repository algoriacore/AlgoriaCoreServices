using AlgoriaCore.Application.Extensions;
using AlgoriaCore.Extensions;
using FluentValidation;

namespace AlgoriaCore.Application.Extensiones
{
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Define un validador de expresiones regulares para un formato de RFC en el constructor de reglas actual, pero solo para propiedades de cadena
        /// La validación fallará si el valor regresado por el lambda no coincide con la expresión regular
		/// </summary>
		/// <typeparam name="T">Tipo de objeto que está siendo validado</typeparam>
		/// <param name="ruleBuilder">El contructor de reglas en elc ual el validaro debe ser  definido</param>
		/// <returns></returns>
		public static IRuleBuilderOptions<T, string> RFC<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches("^[a-zA-ZÑñ&]{3,4}[0-9]{2}[0-1]{1}[0-9]{1}[0-3]{1}[0-9]{1}([a-zA-Z,0-9]{1}[a-zA-Z,0-9]{1}[0-9,a-zA-Z]{1})?$");
        }

        public static IRuleBuilderOptions<T, string> RFCPhysical<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            //return ruleBuilder.Matches(@"^[a-zA-ZÑñ&]{4}[0-9]{2}[0-1][0-9][0-3][0-9][a-zA-Z,0-9]?[a-zA-Z,0-9]?[0-9,a-zA-Z]?$");
            return ruleBuilder.Matches(@"^([A-Z,Ñ,&]{4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$");
        }

        public static IRuleBuilderOptions<T, string> RFCMoral<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            //return ruleBuilder.Matches(@"^[a-zA-ZÑñ&]{3}[0-9]{2}[0-1][0-9][0-3][0-9][a-zA-Z,0-9]?[a-zA-Z,0-9]?[0-9,a-zA-Z]?$");
            return ruleBuilder.Matches(@"^([A-Z,Ñ,&]{3}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$");
        }

        public static IRuleBuilderOptions<T, string> CURP<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches("^[A-Z][A,E,I,O,U,X][A-Z]{2}[0-9]{2}[0-1][0-9][0-3][0-9][M,H]{1}[A-Z]{2}[B,C,D,F,G,H,J,K,L,M,N,Ñ,P,Q,R,S,T,V,W,X,Y,Z]{3}[0-9,A-Z][0-9]$");
        }

        public static IRuleBuilderOptions<T, string> CP<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches("^[0-9]{5}$");

        }
        public static IRuleBuilderOptions<T, string> IsNumberInt<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(@"^[0-9]+$");
        }

        public static IRuleBuilderOptions<T, string> CLABE<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(@"^[0-9]{18}$");
        }

        public static IRuleBuilderOptionsConditions<T, string> ValidCharacters<T>(this IRuleBuilder<T, string> ruleBuilder, string errorMessage)
        {
            return ruleBuilder.ValidCharacters(@"^[a-zA-Z0-9 Ññ!\""%&'´\-:;>=<@_,./{}\+`~áéíóúÁÉÍÓÚüÜ]+$", errorMessage);
        }

        public static IRuleBuilderOptionsConditions<T, string> ValidCharacters<T>(this IRuleBuilder<T, string> ruleBuilder, string expression, string errorMessage)
        {
            return ruleBuilder.Custom((value, context) => {
                if (value.IsNullOrEmpty() || expression.IsNullOrEmpty() || expression.Length == 0)
                {
                    return;
                }

                string invalidCharacters = value.GetInvalidCharacters(expression);

                if (invalidCharacters.Length > 0)
                {
                    context.AddFailure(errorMessage + " '" + invalidCharacters + "'");
                }
            });
        }
    }
}
