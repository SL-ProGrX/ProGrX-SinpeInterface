using System;
using WCFCoreInterno.ApiGalileo;

namespace WCFCoreInterno
{
    public class CoreInterno : ICoreInterno
    {

        GalileoClient galileo = new GalileoClient();

        #region 5 . MÉTODOS DE INTEGRACIÓN DE USO GENERAL

        public bool ServicioDisponible(int CodEmpresa)
        {
            return galileo.ServicioDisponible(CodEmpresa).Result;
        }

        public CuentaIBAN_Response ObtenerCuentaIBAN(int CodEmpresa, CuentaIBAN_Request DatosCuenta)
        {
            return galileo.ObtenerCuentaIBAN(CodEmpresa, DatosCuenta).Result;
        }

        public CL_ObtieneInfoCuenta ObtieneInfoCuenta(int CodEmpresa, string Identificacion, string CuentaIBAN)
        {
            return galileo.ObtieneInfoCuenta(CodEmpresa, Identificacion, CuentaIBAN).Result;
        }

        public CL_ValidaCuenta ValidaCuenta(int CodEmpresa, string Identificacion, string CuentaIBAN, int CodigoMoneda)
        {
            return galileo.ValidaCuenta(CodEmpresa, Identificacion, CuentaIBAN, CodigoMoneda).Result;
        }

        public CL_ResultadoTipoCambio ObtenerTipoCambio(int CodEmpresa,SI_Rastro Rastro, int CodigoServicio, string CuentaOrigen, string CuentaDestino, decimal Monto, int Moneda)
        {
            return galileo.ObtenerTipoCambio(CodEmpresa, Rastro, CodigoServicio, CuentaOrigen, CuentaDestino, Monto, Moneda).Result;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ComisionRespectivaResponse ICoreInterno.ComisionRespectiva(ComisionRespectivaRequest request)
        {
            return galileo.ComisionRespectiva(request).Result;
        }

        public CL_ResultadoValidacion[] ValidaDebitos(int CodEmpresa, SI_Rastro rastro, CL_DatosTransaccion[] transacciones)
        {
            return galileo.ValidaDebitos(CodEmpresa, rastro, transacciones).Result;
        }

        public CL_ResultadoValidacion[] ValidaCreditos(int CodEmpresa, SI_Rastro rastro, CL_DatosTransaccion[] transacciones)
        {
            return galileo.ValidaCreditos(CodEmpresa, rastro, transacciones).Result;
        }

        public ValidacionPerfilTrx_Response ValidarPerfilTransaccional(ValidacionPerfilTrx_Request transaccion)
        {
            throw new NotImplementedException();
        }

        #endregion

      

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ObtieneEstadoTransaccionResponse ICoreInterno.ObtieneEstadoTransaccion(ObtieneEstadoTransaccionRequest request)
        {
           return galileo.ObtieneEstadoTransaccion(request).Result;
        }


        public CL_RespuestaTransaccion[] AplicaDebitosCongelados(int CodEmpresa, SI_Rastro rastro, CL_Transaccion[] Debitos)
        {
            return galileo.AplicaDebitosCongelados(CodEmpresa, rastro, Debitos).Result;
        }


        public CL_RespuestaTransaccion[] AplicaCreditosCongelados(int CodEmpresa, SI_Rastro rastro, CL_Transaccion[] transacciones)
        {
            return galileo.AplicaCreditosCongelados(CodEmpresa, rastro, transacciones).Result;
        }


        public CL_ResultadoActualizacion[] ConfirmaCreditosCongelados(int CodEmpresa, SI_Rastro rastro, CL_ActualizaTransaccion[] transacciones)
        {
            return galileo.ConfirmaCreditosCongelados(CodEmpresa, rastro, transacciones).Result;
        }


        public CL_ResultadoActualizacion[] ConfirmaDebitosCongelados(int CodEmpresa, SI_Rastro rastro, CL_ActualizaTransaccion[] transacciones)
        {
            return galileo.ConfirmaDebitosCongelados(CodEmpresa, rastro, transacciones).Result;
        }


        public CL_RespuestaTransaccion[] AplicaTransferenciasFirmes(int CodEmpresa, SI_Rastro rastro, CL_Transaccion[] transacciones)
        {
            throw new NotImplementedException();
        }


        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SaldoDisponibleResponse ICoreInterno.SaldoDisponible(SaldoDisponibleRequest request)
        {
            return galileo.SaldoDisponible(request).Result;
        }

        public CL_ResultadoActualizacion[] ReversaCreditos(int CodEmpresa, SI_Rastro rastro, TransaccionRechazada[] transacciones)
        {
            return galileo.ReversaCreditos(CodEmpresa, rastro, transacciones).Result;
        }


        public CL_ResultadoActualizacion[] ReversaDebitos(int CodEmpresa, SI_Rastro rastro, TransaccionRechazada[] transacciones)
        {
            return galileo.ReversaDebitos(CodEmpresa, rastro, transacciones).Result;
        }


        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ObtenerInformacionClienteResponse ICoreInterno.ObtenerInformacionCliente(ObtenerInformacionClienteRequest request)
        {
           return galileo.ObtenerInformacionCliente(request).Result;
        }


        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ObtenerProductosPorClienteResponse ICoreInterno.ObtenerProductosPorCliente(ObtenerProductosPorClienteRequest request)
        {
            return galileo.ObtenerProductosPorCliente(request).Result;
        }

        public bool ActualizarFechaCiclo(int CodEmpresa, int ComprobanteCGP, string DocumentoSistemaInterno, int ServicioSINPE, System.DateTime FechaCiclo, string CodigoReferenciaAnterior, string CodigoReferenciaNuevo)
        {
            return galileo.ActualizarFechaCiclo(CodEmpresa, ComprobanteCGP, DocumentoSistemaInterno, ServicioSINPE, FechaCiclo, CodigoReferenciaAnterior, CodigoReferenciaNuevo).Result;
        }

        public bool LiquidarCiclo(int CodEmpresa, int[] EntidadesAplazadas, int ServicioSINPE, string Modalidad, System.DateTime FechaCiclo)
        {
            return galileo.LiquidarCiclo(CodEmpresa, EntidadesAplazadas, ServicioSINPE, Modalidad, FechaCiclo).Result;
        }

        public CL_RespuestaNotificacion[] PreferenciasNotificacion(SI_Rastro rastro, CL_Notificacion[] notificacion)
        {
            throw new NotImplementedException();
        }

    }
}
