using Newtonsoft.Json;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Postmon4Net
{
    public class EncontrarEndereco
    {
        private const string FORMATO_METODO_OBTER_CEP = "http://api.postmon.com.br/v1/cep/{0}";

        /// <summary>
        /// Retorna os dados de um endereço a patir do número do CEP.
        /// </summary>
        /// <param name="CEP">Número do CEP.</param>
        /// <param name="excecaoSeNaoEncontrar">Lança uma exceção caso não encontre o CEP se assinado como TRUE.</param>
        /// <returns>Os dados do endereço de acordo com o JSON resultado da API Postmon.</returns>
        public static EnderecoInfo PorCEP(string CEP, bool excecaoSeNaoEncontrar = false)
        {
            Validar(CEP);

            EnderecoInfo endereco = null;

            try
            {
                CEP = Limpar(CEP);
                string resposta = GetString(string.Format(FORMATO_METODO_OBTER_CEP, CEP));
                endereco = RetornarEnderecoInfo(resposta);
            }
            catch (WebException ex)
            {
                if (!Encontrou(ex))
                {
                    if (excecaoSeNaoEncontrar)
                        throw new Exception(string.Format("CEP {0} não encontrado.", CEP));
                }
                else
                    throw ex;
            }

            return endereco;
        }

        private static string GetString(string endereco)
        {
            string resposta;

            using (WebClient wc = new WebClient())
            {
                resposta = wc.DownloadString(endereco);
            }

            return resposta;
        }

        private static void Validar(string CEP)
        {
            ValidarCep.Validar(CEP, true);
        }

        private static string Limpar(string CEP)
        {
            string CEPLimpo = CEP.Trim();
            CEPLimpo = Regex.Replace(CEPLimpo, @"\D", "");
            return CEPLimpo;
        }


        private static bool Encontrou(WebException ex)
        {
            bool encontrouCEP = !(ex.Message.Contains("(404)"));
            return encontrouCEP;
        }


        private static EnderecoInfo RetornarEnderecoInfo(string jsonEndereco)
        {
            return JsonConvert.DeserializeObject<EnderecoInfo>(jsonEndereco);
        }
    }
}