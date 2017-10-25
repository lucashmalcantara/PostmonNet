using System;
using System.Text.RegularExpressions;

namespace Postmon4Net
{
    public class ValidarCep
    {
        private const string PATTERN_CEP_VALIDO = @"^[0-9]{2}\.?[0-9]{3}-?[0-9]{3}\z";

        public static bool Validar(string CEP, bool excecaoSeInvalido = false)
        {
            bool valido = true;
            string mensagemValidacao = null;

            if (string.IsNullOrEmpty(CEP))
            {
                valido = false;
                mensagemValidacao = "O CEP não pode ser vazio ou nulo.";
            }
            else if (!Regex.IsMatch(CEP, PATTERN_CEP_VALIDO))
            {
                valido = false;
                mensagemValidacao = "CEP inválido.";
            }

            if (!valido && excecaoSeInvalido)
                throw new Exception(mensagemValidacao);

            return valido;
        }
    }
}
