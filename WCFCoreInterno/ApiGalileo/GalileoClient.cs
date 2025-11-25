using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Util;

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

        #region Métodos de integración de uso general

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

        public async Task<ComisionRespectivaResponse> ComisionRespectiva(ComisionRespectivaRequest request)
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
                         $"{galileoUri}/api/mKindoService/ComisionRespectiva/{request.CodEmpresa}",
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
                    rastro = Rastro,
                    transacciones = Debitos
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
                    rastro = Rastro,
                    transacciones = Debitos
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

        #endregion

        #region Métodos para la integración transaccional
       
        public async Task<CL_RespuestaTransaccion[]> AplicaDebitosCongelados(int CodEmpresa, SI_Rastro Rastro, CL_Transaccion[] Debitos)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var payload = new
                {
                    rastro = Rastro,
                    transacciones = Debitos
                };

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/AplicaDebitosCongelados/{CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CL_RespuestaTransaccion[]>(json);
            }
            catch (System.Exception)
            {
                return new CL_RespuestaTransaccion[]
                {
                    new CL_RespuestaTransaccion()
                    {
                        IdRelacionCliente = null,
                        InformacionAdicional = null,
                        MotivoError = 28,
                        Resultado = E_Resultado.Error
                    }
                };
            }
        }

        public async Task<CL_RespuestaTransaccion[]> AplicaCreditosCongelados(int CodEmpresa, SI_Rastro Rastro, CL_Transaccion[] Creditos)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var payload = new
                {
                    rastro = Rastro,
                    transacciones = Creditos
                };

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/AplicaCreditosCongelados/{CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CL_RespuestaTransaccion[]>(json);
            }
            catch (System.Exception)
            {
                return new CL_RespuestaTransaccion[]
                {
                    new CL_RespuestaTransaccion()
                    {
                        IdRelacionCliente = null,
                        InformacionAdicional = null,
                        MotivoError = 28,
                        Resultado = E_Resultado.Error
                    }
                };
            }
        }

        public async Task<CL_ResultadoActualizacion[]> ConfirmaDebitosCongelados(int CodEmpresa, SI_Rastro Rastro, CL_ActualizaTransaccion[] Transacciones)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var payload = new
                {
                    rastro = Rastro,
                    transacciones = Transacciones
                };

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/ConfirmaDebitosCongelados/{CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CL_ResultadoActualizacion[]>(json);
            }
            catch (System.Exception)
            {
                return new CL_ResultadoActualizacion[]
                {
                    new CL_ResultadoActualizacion()
                    {
                        ExtensionData = null,
                        IdRelacionCliente = null,
                        Resultado = E_ResultadoActualizacion.Error
                    }
                };
            }
        }

        public async Task<CL_ResultadoActualizacion[]> ConfirmaCreditosCongelados(int CodEmpresa, SI_Rastro Rastro, CL_ActualizaTransaccion[] Transacciones)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var payload = new
                {
                    rastro = Rastro,
                    transacciones = Transacciones
                };

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/ConfirmaCreditosCongelados/{CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CL_ResultadoActualizacion[]>(json);
            }
            catch (System.Exception)
            {
                return new CL_ResultadoActualizacion[]
                {
                    new CL_ResultadoActualizacion()
                    {
                        ExtensionData = null,
                        IdRelacionCliente = null,
                        Resultado = E_ResultadoActualizacion.Error
                    }
                };
            }
        }

        public async Task<CL_ResultadoActualizacion[]> ReversaCreditos(int CodEmpresa, SI_Rastro Rastro, TransaccionRechazada[] Transacciones)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var payload = new
                {
                    rastro = Rastro,
                    transacciones = Transacciones
                };

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/ReversaCreditos/{CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CL_ResultadoActualizacion[]>(json);
            }
            catch (System.Exception)
            {
                return new CL_ResultadoActualizacion[]
                {
                    new CL_ResultadoActualizacion()
                    {
                        ExtensionData = null,
                        IdRelacionCliente = null,
                        Resultado = E_ResultadoActualizacion.Error
                    }
                };
            }
        }

        public async Task<CL_ResultadoActualizacion[]> ReversaDebitos(int CodEmpresa, SI_Rastro Rastro, TransaccionRechazada[] Transacciones)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var payload = new
                {
                    rastro = Rastro,
                    transacciones = Transacciones
                };

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/ReversaDebitos/{CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CL_ResultadoActualizacion[]>(json);
            }
            catch (System.Exception)
            {
                return new CL_ResultadoActualizacion[]
                {
                    new CL_ResultadoActualizacion()
                    {
                        ExtensionData = null,
                        IdRelacionCliente = null,
                        Resultado = E_ResultadoActualizacion.Error
                    }
                };
            }
        }

        public async Task<ObtieneEstadoTransaccionResponse> ObtieneEstadoTransaccion(ObtieneEstadoTransaccionRequest Request)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(Request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/ObtieneEstadoTransaccion/{Request.CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObtieneEstadoTransaccionResponse>(json);
            }
            catch (System.Exception)
            {
                return new ObtieneEstadoTransaccionResponse
                {
                    ComprobanteInterno = null,
                    ObtieneEstadoTransaccionResult = false
                };
            }
        }

        #endregion

        #region Métodos para la integración de la liquidación de la cámara
        public async Task<bool> ActualizarFechaCiclo(int CodEmpresa, int ComprobanteCGP, string DocumentoSistemaInterno, int ServicioSINPE, DateTime FechaCiclo, string CodigoReferenciaAnterior, string CodigoReferenciaNuevo)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var payload = new
                {
                    comprobanteCGP = ComprobanteCGP,
                    documentoSistemaInterno = DocumentoSistemaInterno,
                    servicioSINPE = ServicioSINPE,
                    fechaCiclo = FechaCiclo,
                    codigoReferenciaAnterior = CodigoReferenciaAnterior,
                    codigoReferenciaNuevo = CodigoReferenciaNuevo
                };

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/ActualizarFechaCiclo/{CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(json);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<bool> LiquidarCiclo(int CodEmpresa, int[] EntidadesAplazadas, int ServicioSINPE, string Modalidad, DateTime FechaCiclo)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var payload = new
                {
                    entidadesAplazadas = EntidadesAplazadas,
                    servicioSINPE = ServicioSINPE,
                    modalidad = Modalidad,
                    fechaCiclo = FechaCiclo
                };

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/LiquidarCiclo/{CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(json);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        #endregion

        #region Métodos para la integración del PortalCGP

        public async Task<SaldoDisponibleResponse> SaldoDisponible(SaldoDisponibleRequest Request)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(Request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/SaldoDisponible/{Request.CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SaldoDisponibleResponse>(json);
            }
            catch (System.Exception)
            {
                return new SaldoDisponibleResponse
                {
                    disponible = false,
                    SaldoDisponibleResult = E_Resultado.Error
                };
            }
        }

        public async Task<ObtenerInformacionClienteResponse> ObtenerInformacionCliente(ObtenerInformacionClienteRequest request)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/ObtenerInformacionCliente/{request.CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObtenerInformacionClienteResponse>(json);
            }
            catch (System.Exception)
            {
                return new ObtenerInformacionClienteResponse
                {
                    informacionCliente = null,
                    ObtenerInformacionClienteResult = E_Resultado.Error
                };
            }
        }

        public async Task<ObtenerProductosPorClienteResponse> ObtenerProductosPorCliente(ObtenerProductosPorClienteRequest request)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                         $"{galileoUri}/api/mKindoService/ObtenerProductosPorCliente/{request.CodEmpresa}",
                         jsonContent
                     );

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObtenerProductosPorClienteResponse>(json);
            }
            catch (System.Exception)
            {
                return new ObtenerProductosPorClienteResponse
                {
                    productos = null,
                    ObtenerProductosPorClienteResult = E_Resultado.Error
                };
            }
        }

        #endregion

    }
}