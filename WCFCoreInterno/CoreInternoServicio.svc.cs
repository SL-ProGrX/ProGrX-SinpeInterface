using System;
using WCFCoreInterno.ApiGalileo;

namespace WCFCoreInterno
{
    public class CoreInterno : ICoreInterno
    {

        GalileoClient galileo = new GalileoClient();

        #region 5 . MÉTODOS DE INTEGRACIÓN DE USO GENERAL

        public bool ServicioDisponible()
        {
            return galileo.ServicioDisponible(galileo.getCodEmpresa()).Result;
        }

        public CuentaIBAN_Response ObtenerCuentaIBAN(CuentaIBAN_Request DatosCuenta)
        {
            return galileo.ObtenerCuentaIBAN(galileo.getCodEmpresa(), DatosCuenta).Result;
        }

        public CL_ObtieneInfoCuenta ObtieneInfoCuenta(string Identificacion, string CuentaIBAN)
        {
            return galileo.ObtieneInfoCuenta(galileo.getCodEmpresa(), Identificacion, CuentaIBAN).Result;
        }

        public CL_ValidaCuenta ValidaCuenta(string Identificacion, string CuentaIBAN, int CodigoMoneda)
        {
            return galileo.ValidaCuenta(galileo.getCodEmpresa(), Identificacion, CuentaIBAN, CodigoMoneda).Result;
        }

        public CL_ResultadoTipoCambio ObtenerTipoCambio(SI_Rastro Rastro, int CodigoServicio, string CuentaOrigen, string CuentaDestino, decimal Monto, int Moneda)
        {
            return galileo.ObtenerTipoCambio(galileo.getCodEmpresa(), Rastro, CodigoServicio, CuentaOrigen, CuentaDestino, Monto, Moneda).Result;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ComisionRespectivaResponse ICoreInterno.ComisionRespectiva(ComisionRespectivaRequest request)
        {
            return galileo.ComisionRespectiva(galileo.getCodEmpresa(), request).Result;
        }

        public CL_ResultadoValidacion[] ValidaDebitos(SI_Rastro rastro, CL_DatosTransaccion[] transacciones)
        {
            return galileo.ValidaDebitos(galileo.getCodEmpresa(), rastro, transacciones).Result;
        }

        public CL_ResultadoValidacion[] ValidaCreditos(SI_Rastro rastro, CL_DatosTransaccion[] transacciones)
        {
            return galileo.ValidaCreditos(galileo.getCodEmpresa(), rastro, transacciones).Result;
        }

        public ValidacionPerfilTrx_Response ValidarPerfilTransaccional(ValidacionPerfilTrx_Request transaccion)
        {
            throw new NotImplementedException();
        }

        #endregion

      

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ObtieneEstadoTransaccionResponse ICoreInterno.ObtieneEstadoTransaccion(ObtieneEstadoTransaccionRequest request)
        {
           return galileo.ObtieneEstadoTransaccion(galileo.getCodEmpresa(), request).Result;
        }


        public CL_RespuestaTransaccion[] AplicaDebitosCongelados(SI_Rastro rastro, CL_Transaccion[] Debitos)
        {
            return galileo.AplicaDebitosCongelados(galileo.getCodEmpresa(), rastro, Debitos).Result;
        }


        public CL_RespuestaTransaccion[] AplicaCreditosCongelados(SI_Rastro rastro, CL_Transaccion[] transacciones)
        {
            return galileo.AplicaCreditosCongelados(galileo.getCodEmpresa(), rastro, transacciones).Result;
        }


        public CL_ResultadoActualizacion[] ConfirmaCreditosCongelados(SI_Rastro rastro, CL_ActualizaTransaccion[] transacciones)
        {
            return galileo.ConfirmaCreditosCongelados(galileo.getCodEmpresa(), rastro, transacciones).Result;
        }


        public CL_ResultadoActualizacion[] ConfirmaDebitosCongelados(SI_Rastro rastro, CL_ActualizaTransaccion[] transacciones)
        {
            return galileo.ConfirmaDebitosCongelados(galileo.getCodEmpresa(), rastro, transacciones).Result;
        }


        public CL_RespuestaTransaccion[] AplicaTransferenciasFirmes(SI_Rastro rastro, CL_Transaccion[] transacciones)
        {
            throw new NotImplementedException();
        }


        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SaldoDisponibleResponse ICoreInterno.SaldoDisponible(SaldoDisponibleRequest request)
        {
            return galileo.SaldoDisponible(galileo.getCodEmpresa(), request).Result;
        }

        public CL_ResultadoActualizacion[] ReversaCreditos(SI_Rastro rastro, TransaccionRechazada[] transacciones)
        {
            return galileo.ReversaCreditos(galileo.getCodEmpresa(), rastro, transacciones).Result;
        }


        public CL_ResultadoActualizacion[] ReversaDebitos(SI_Rastro rastro, TransaccionRechazada[] transacciones)
        {
            return galileo.ReversaDebitos(galileo.getCodEmpresa(), rastro, transacciones).Result;
        }


        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ObtenerInformacionClienteResponse ICoreInterno.ObtenerInformacionCliente(ObtenerInformacionClienteRequest request)
        {
           return galileo.ObtenerInformacionCliente(galileo.getCodEmpresa(), request).Result;
        }


        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ObtenerProductosPorClienteResponse ICoreInterno.ObtenerProductosPorCliente(ObtenerProductosPorClienteRequest request)
        {
            return galileo.ObtenerProductosPorCliente(galileo.getCodEmpresa(), request).Result;
        }

        public bool ActualizarFechaCiclo(int ComprobanteCGP, string DocumentoSistemaInterno, int ServicioSINPE, System.DateTime FechaCiclo, string CodigoReferenciaAnterior, string CodigoReferenciaNuevo)
        {
            return galileo.ActualizarFechaCiclo(galileo.getCodEmpresa(), ComprobanteCGP, DocumentoSistemaInterno, ServicioSINPE, FechaCiclo, CodigoReferenciaAnterior, CodigoReferenciaNuevo).Result;
        }

        public bool LiquidarCiclo(int[] EntidadesAplazadas, int ServicioSINPE, string Modalidad, System.DateTime FechaCiclo)
        {
            return galileo.LiquidarCiclo(galileo.getCodEmpresa(), EntidadesAplazadas, ServicioSINPE, Modalidad, FechaCiclo).Result;
        }

        public CL_RespuestaNotificacion[] PreferenciasNotificacion(SI_Rastro rastro, CL_Notificacion[] notificacion)
        {
            throw new NotImplementedException();
        }

    }
}
