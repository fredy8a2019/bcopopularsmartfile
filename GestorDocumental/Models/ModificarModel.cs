using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GestorDocumental.Models
{
    /// <summary>
    /// Autor :     Elena Parr
    /// Fecha :     20 - May del 2014
    /// Desc :      Modela toda la informacion de para la noficacion de la misma
    /// </summary>
    public class ModificarModel
    {
        private static GestorDocumentalEnt db = new GestorDocumentalEnt();

        #region ModicarCodigosBarras
        public static IList<ModificarCodigosPropiedades> GetAll(string NumeroLote, string negocio)
        {
            IList<ModificarCodigosPropiedades> resultado =
             (IList<ModificarCodigosPropiedades>)HttpContext.Current.Session["CodigosBarrasNegocio"];

            if (resultado == null)
            {
                var datosUsusarios = db.spModificar_ConsultarCodigosBarras(NumeroLote, negocio);

                HttpContext.Current.Session["CodigosBarrasNegocio"] = resultado =
                        datosUsusarios.Select(x => new ModificarCodigosPropiedades
                        {
                            negocio = x.negid,
                            CodigoBarras = x.codbarras
                        }).ToList();
            }

            return resultado;
        }

        public static ModificarCodigosPropiedades One(Func<ModificarCodigosPropiedades, bool> predicate)
        {
            var negocio = HttpContext.Current.Session["Negocio"].ToString();
            var Lote = HttpContext.Current.Session["NumeroLote"].ToString();
            return GetAll(Lote, negocio).Where(predicate).FirstOrDefault();
        }

        public static void Update(ModificarCodigosPropiedades data)
        {
            if (data != null)
            {
                db.spModificar_UpdateCodigosBarras(data.negocio, data.CodigoBarras, ((Usuarios)HttpContext.Current.Session["USUARIO_LOGUEADO"]).IdUsuario);
            }
        }

        #endregion

        #region AnularNegociosLabel
        /// <summary>
        /// Anula el negocio o el codigo de barras que ingrese
        /// </summary>
        /// <param name="negocio"></param>
        /// <param name="label"></param>
        public static void anularLabelNegocio(decimal negocio, string label, int tipoAnulacion)
        {
            db.spAnulacionNegocios(negocio, ((Usuarios)HttpContext.Current.Session["USUARIO_LOGUEADO"]).IdUsuario, label, tipoAnulacion);
        }
        #endregion

        #region Descontabilizar

        public static IList<DescontabilizarPropiedades> GetAll(string codigoBarras)
        {
            IList<DescontabilizarPropiedades> resultado =
                (IList<DescontabilizarPropiedades>)HttpContext.Current.Session["datosDescontabilizar"];
            //if (resultado == null)
            //{
                var datosDescontabilizar = db.spConsultadeContabilizacion(codigoBarras);

                HttpContext.Current.Session["datosDescontabilizar"] = resultado = datosDescontabilizar.Select(x => new DescontabilizarPropiedades
                    {
                        usuario = x.NomUsuario,
                        NomCampo = x.CampDescripcion,
                        valor = x.negvalor,
                        FechaRegistro = x.FechaRegistro,
                        idCampo = x.CampId,
                        captura = x.NumCaptura,
                        negocio = x.NegId
                    }).ToList();
            //}

            return resultado;
        }

        public static DescontabilizarPropiedades OneDes(Func<DescontabilizarPropiedades, bool> predicate)
        {
            var codigoBarras = HttpContext.Current.Session["codigoBarras"].ToString();
            return GetAll(codigoBarras).Where(predicate).FirstOrDefault();
        }

        public static void Update(DescontabilizarPropiedades data)
        {
            if (data != null)
            {
                db.spUpdateContabilizacion(data.negocio, data.idCampo, ((Usuarios)HttpContext.Current.Session["USUARIO_LOGUEADO"]).IdUsuario, data.valor);
            }
        }

        #endregion

        #region AjustesDataNegocio

        public static IList<AjusteDatosNegocioPropiedades> GetAll(decimal numero)
        {
            IList<AjusteDatosNegocioPropiedades> resultado =
                (IList<AjusteDatosNegocioPropiedades>)HttpContext.Current.Session["AjustesDatos"];
            if (resultado == null)
            {
                var AjustesDatos = db.spModificar_ConsultarCapturaNegocio(numero);

                HttpContext.Current.Session["AjustesDatos"] = resultado = AjustesDatos.Select(x => new AjusteDatosNegocioPropiedades
                {
                    DescripcionCampo = x.CAMPDESCRIPCION,
                    IdCampo = x.CAMPID,
                    Indice = x.INDICE,
                    Negocio = x.NEGID,
                    Valor = x.NEGVALOR,
                    tipoCampos = x.TIPOCAMPO,
                    Captura = x.NUMEROCAPTURA,
                    TipDoc = x.TIPODOCUMENTAL
                }).ToList();
            }


            return resultado;
        }

        public static AjusteDatosNegocioPropiedades OneAjuste(Func<AjusteDatosNegocioPropiedades, bool> predicate)
        {
            var numero = decimal.Parse(HttpContext.Current.Session["negocioAjustar"].ToString());
            return GetAll(numero).Where(predicate).FirstOrDefault();
        }

        public static void Update(AjusteDatosNegocioPropiedades data)
        {
            if (data != null)
            {
                data.Captura = ModificarModel.aumentoCaptura(data.Captura);
                db.spModificar_UpdateCapturaNegocio(data.Negocio, data.IdCampo, data.Indice, data.Captura, data.Valor, ((Usuarios)HttpContext.Current.Session["USUARIO_LOGUEADO"]).IdUsuario, data.TipDoc);
            }
        }

        private static int? aumentoCaptura(int? captura)
        {
            var capturaAumentada = captura + 1;
            return capturaAumentada;
        }
        #endregion
    }


    /// <summary>
    /// Priopiedades de la tabla que se observa en la interfaz de modificar codigos de barras
    /// </summary>
    [KnownType(typeof(ModificarCodigosPropiedades))]
    public class ModificarCodigosPropiedades
    {
        [DataType("String")]
        public decimal negocio { get; set; }

        [Required]
        [DataType("String")]
        [DisplayName("Codigo de barras")]
        public string CodigoBarras { get; set; }
    }

    /// <summary>
    /// Proepiedades de la tabla que se observa en la interfaz de descontabilizar
    /// </summary>
    [KnownType(typeof(DescontabilizarPropiedades))]
    public class DescontabilizarPropiedades
    {
        [DataType("String")]
        public string usuario { get; set; }

        [Required]
        [DisplayName("Nombre Campo")]
        public string NomCampo { get; set; }

        [DataType("String")]
        public string valor { get; set; }

        [DisplayName("Fecha Registo")]
        [DataType(DataType.Date)]
        public DateTime? FechaRegistro { get; set; }

        [Required]
        [DataType("String")]
        public decimal negocio { get; set; }

        [Required]
        [DataType("String")]
        public int captura { get; set; }

        [Required]
        [DataType("String")]
        public int idCampo { get; set; }

    }

    /// <summary>
    /// Proepiedades de la tabla que se observa en la interfaz de Ajustar datos captura
    /// </summary>
    [KnownType(typeof(AjusteDatosNegocioPropiedades))]
    public class AjusteDatosNegocioPropiedades
    {
        [DataType("String")]
        public decimal? Negocio { get; set; }

        [DataType("String")]
        public int? IdCampo { get; set; }

        [DataType("String")]
        public string DescripcionCampo { get; set; }

        [DataType("String")]
        public int? Indice { get; set; }

        [Required]
        [DataType("String")]
        public string Valor { get; set; }

        [DataType("String")]
        public int? tipoCampos { get; set; }

        [DataType("String")]
        public int? Captura { get; set; }

        [DataType("String")]
        public int? TipDoc { get; set; }
    }



}