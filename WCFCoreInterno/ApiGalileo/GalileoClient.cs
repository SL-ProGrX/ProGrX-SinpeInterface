using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WCFCoreInterno.ApiGalileo
{
    public class GalileoClient
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string galileoUri;
        public readonly string _token;

        public GalileoClient()
        {
            galileoUri = System.Configuration.ConfigurationManager.AppSettings["GalileoUri"];
            _token = System.Configuration.ConfigurationManager.AppSettings["GalileoToken"];
        }

        public async Task<bool> ServicioDisponible(int CodEmpresa)
        {
            var rersponse = false;
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var response = await _httpClient.GetAsync($"{galileoUri}/api/mKindoService/ServicioDisponible/{CodEmpresa}");

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(json);
            }
            catch (System.Exception)
            {
                rersponse = false;
            }

            return rersponse;
        }

        public async Task<CuentaIBAN_Response> ObtenerCuentaIBAN(int CodEmpresa, CuentaIBAN_Request DatosCuenta)
        {
            var rersponse = new CuentaIBAN_Response();
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(DatosCuenta),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/ObtenerCuentaIBAN/{CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CuentaIBAN_Response>(json);
            }
            catch (System.Exception)
            {
                rersponse = new CuentaIBAN_Response()
                { 
                    CuentaIBAN = null,
                    Errores = new List<CL_Error> { 
                        new CL_Error() 
                        { 
                            NumError = -1, 
                            Descripcion = "Error de comunicación con el servicio Galileo" 
                        } 
                    },
                    Resultado = false
                };
            }

            return rersponse;
        }

    }
}