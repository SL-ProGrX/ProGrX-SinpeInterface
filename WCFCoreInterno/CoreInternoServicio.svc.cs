using System;
using WCFCoreInterno.ApiGalileo;

namespace WCFCoreInterno
{
    public class CoreInterno : ICoreInterno
    {

        GalileoClient galileo = new GalileoClient();

        public bool ServicioDisponible(int CodEmpresa)
        {
            return galileo.ServicioDisponible(CodEmpresa).Result;
        }

        public CL_ResultadoValidacion[] ValidaDebitos(SI_Rastro rastro, CL_DatosTransaccion[] transacciones)
        {
            throw new NotImplementedException();
        }


        public CL_ResultadoValidacion[] ValidaCreditos(SI_Rastro rastro, CL_DatosTransaccion[] transacciones)
        {
            throw new NotImplementedException();
        }


        public CL_ValidaCuenta ValidaCuenta(string Identificacion, string CuentaIBAN, int CodigoMoneda)
        {
            throw new NotImplementedException();
        }

        public ValidacionPerfilTrx_Response ValidarPerfilTransaccional(ValidacionPerfilTrx_Request transaccion)
        {
            throw new NotImplementedException();
        }

        public CuentaIBAN_Response ObtenerCuentaIBAN(int CodEmpresa, CuentaIBAN_Request DatosCuenta)
        {
            return galileo.ObtenerCuentaIBAN(CodEmpresa, DatosCuenta).Result;
        }

        public CL_ObtieneInfoCuenta ObtieneInfoCuenta(string Identificacion, string CuentaIBAN)
        {
            throw new NotImplementedException();
        }


        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ObtieneEstadoTransaccionResponse ICoreInterno.ObtieneEstadoTransaccion(ObtieneEstadoTransaccionRequest request)
        {
            throw new NotImplementedException();
        }


        public CL_RespuestaTransaccion[] AplicaDebitosCongelados(SI_Rastro rastro, CL_Transaccion[] transacciones)
        {
            throw new NotImplementedException();
        }


        public CL_RespuestaTransaccion[] AplicaCreditosCongelados(SI_Rastro rastro, CL_Transaccion[] transacciones)
        {
            throw new NotImplementedException();
        }


        public CL_ResultadoActualizacion[] ConfirmaCreditosCongelados(SI_Rastro rastro, CL_ActualizaTransaccion[] transacciones)
        {
            throw new NotImplementedException();
        }


        public CL_ResultadoActualizacion[] ConfirmaDebitosCongelados(SI_Rastro rastro, CL_ActualizaTransaccion[] transacciones)
        {
            throw new NotImplementedException();
        }


        public CL_RespuestaTransaccion[] AplicaTransferenciasFirmes(SI_Rastro rastro, CL_Transaccion[] transacciones)
        {
            throw new NotImplementedException();
        }


        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SaldoDisponibleResponse ICoreInterno.SaldoDisponible(SaldoDisponibleRequest request)
        {
            throw new NotImplementedException();
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ComisionRespectivaResponse ICoreInterno.ComisionRespectiva(ComisionRespectivaRequest request)
        {
            throw new NotImplementedException();
        }


        public CL_ResultadoActualizacion[] ReversaCreditos(SI_Rastro rastro, TransaccionRechazada[] transacciones)
        {
            throw new NotImplementedException();
        }


        public CL_ResultadoActualizacion[] ReversaDebitos(SI_Rastro rastro, TransaccionRechazada[] transacciones)
        {
            throw new NotImplementedException();
        }


        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ObtenerInformacionClienteResponse ICoreInterno.ObtenerInformacionCliente(ObtenerInformacionClienteRequest request)
        {
            throw new NotImplementedException();
        }


        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ObtenerProductosPorClienteResponse ICoreInterno.ObtenerProductosPorCliente(ObtenerProductosPorClienteRequest request)
        {
            throw new NotImplementedException();
        }


        public CL_ResultadoTipoCambio ObtenerTipoCambio(SI_Rastro Rastro, int CodigoServicio, string CuentaOrigen, string CuentaDestino, decimal Monto, int Moneda)
        {
            throw new NotImplementedException();
        }

        public bool ActualizarFechaCiclo(int ComprobanteCGP, string DocumentoSistemaInterno, int ServicioSINPE, System.DateTime FechaCiclo, string CodigoReferenciaAnterior, string CodigoReferenciaNuevo)
        {
            throw new NotImplementedException();
        }

        public bool LiquidarCiclo(int[] EntidadesAplazadas, int ServicioSINPE, string Modalidad, System.DateTime FechaCiclo)
        {
            throw new NotImplementedException();
        }

        public CL_RespuestaNotificacion[] PreferenciasNotificacion(SI_Rastro rastro, CL_Notificacion[] notificacion)
        {
            throw new NotImplementedException();
        }

    }
}
