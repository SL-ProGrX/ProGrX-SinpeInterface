using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
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

        public async Task<CL_ObtieneInfoCuenta> ObtieneInfoCuenta(int CodEmpresa, string Identificacion, string CuentaIBAN)
        {
            var rersponse = new CL_ObtieneInfoCuenta();
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                string ident = WebUtility.UrlEncode(Identificacion);
                string cuenta = WebUtility.UrlEncode(CuentaIBAN);

                string url =
                    $"{galileoUri}/api/mKindoService/ObtieneInfoCuenta/{CodEmpresa}" +
                    $"?Identificacion={ident}&CuentaIBAN={cuenta}";

                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CL_ObtieneInfoCuenta>(json);
            }
            catch (System.Exception)
            {
                rersponse = new CL_ObtieneInfoCuenta()
                {
                    MotivoError = 28,
                    Resultado = E_Resultado.Error,
                    Estado = E_Estado.NoActiva,
                    NombreTitular = null,
                    Moneda = 0
                };
            }

            return rersponse;
        }

        public async Task<CL_ValidaCuenta> ValidaCuenta(int CodEmpresa, string Identificacion, string CuentaIBAN, int CodigoMoneda)
        {
            var rersponse = new CL_ValidaCuenta();
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                string ident = WebUtility.UrlEncode(Identificacion);
                string cuenta = WebUtility.UrlEncode(CuentaIBAN);

                string url =
                    $"{galileoUri}/api/mKindoService/ValidaCuenta/{CodEmpresa}" +
                    $"?Identificacion={ident}&CuentaIBAN={cuenta}&CodigoMoneda={CodigoMoneda}";

                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CL_ValidaCuenta>(json);
            }
            catch (System.Exception)
            {
                rersponse = new CL_ValidaCuenta()
                {
                    MotivoError = 28,
                    Resultado = E_Resultado.Error,
                };
            }

            return rersponse;
        }
    
        public async Task<CL_ResultadoTipoCambio> ObtenerTipoCambio(int CodEmpresa, SI_Rastro Rastro, int CodigoServicio, string Cuentaorigen, string CuentaDestino, decimal Monto, int Moneda)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var payload = new 
                {
                    Rastro = Rastro,
                };

                // Armamos querystring para los parámetros simples
                string qs =
                    $"?CodigoServicio={CodigoServicio}" +
                    $"&CuentaOrigen={WebUtility.UrlEncode(Cuentaorigen)}" +
                    $"&CuentaDestino={WebUtility.UrlEncode(CuentaDestino)}" +
                    $"&Monto={Monto.ToString(CultureInfo.InvariantCulture)}" +
                    $"&Moneda={Moneda}";

                var content = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                    $"{galileoUri}/api/mKindoService/ObtenerTipoCambio/{CodEmpresa}" + qs,
                    content);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CL_ResultadoTipoCambio>(json);
            }
            catch
            {
                return new CL_ResultadoTipoCambio
                {
                   ExtensionData = null,
                   TipoCambioAplicado = 0,
                   montoTotal = 0
                };
            }
        }

        public async Task<ComisionRespectivaResponse> ComisionRespectiva(int CodEmpresa, ComisionRespectivaRequest request)
        {
            var rersponse = new ComisionRespectivaResponse();
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/ComisionRespectiva/{CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ComisionRespectivaResponse>(json);
            }
            catch (System.Exception)
            {
                rersponse = new ComisionRespectivaResponse()
                {
                   codigoMonedaComision = 1,
                   comision = 0,
                   ComisionRespectivaResult = E_Resultado.Error
                };
            }

            return rersponse;
        }

        public async Task<CL_ResultadoValidacion[]> ValidaDebitos(int CodEmpresa, SI_Rastro Rastro, CL_DatosTransaccion[] Debitos)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var payload = new
                {
                    Rastro = Rastro,
                    Debitos = Debitos
                };

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/ValidaDebitos/{CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CL_ResultadoValidacion[]>(json);
            }
            catch (System.Exception)
            {
                return new CL_ResultadoValidacion[]
                {
                    new CL_ResultadoValidacion()
                    {
                        IdRelacionCliente = null,
                        InformacionAdicional = null,
                        MotivoError = 28,
                        Resultado = E_Resultado.Error
                    }
                };
            }
        }

        public async Task<CL_ResultadoValidacion[]> ValidaCreditos(int CodEmpresa, SI_Rastro Rastro, CL_DatosTransaccion[] Debitos)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var payload = new
                {
                    Rastro = Rastro,
                    Debitos = Debitos
                };

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/ValidaCreditos/{CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CL_ResultadoValidacion[]>(json);
            }
            catch (System.Exception)
            {
                return new CL_ResultadoValidacion[]
                {
                    new CL_ResultadoValidacion()
                    {
                        IdRelacionCliente = null,
                        InformacionAdicional = null,
                        MotivoError = 28,
                        Resultado = E_Resultado.Error
                    }
                };
            }
        }

    }
}