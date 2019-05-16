using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc.UI;
using Telerik.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class ModificarController : Controller
    {
        #region ModificarCodigosBarras

        public ActionResult ModificarCodigoBarras(GridEditMode? mode, GridButtonType? type, GridInsertRowPosition?
         insertRowPosition)
        {
            ViewData["mode"] = mode ?? GridEditMode.InLine;
            ViewData["type"] = type ?? GridButtonType.Text;
            HttpContext.Session["NumeroLote"] = string.Empty;
            HttpContext.Session["Negocio"] = string.Empty;
            return View();
        }

        [GridAction]
        public ActionResult _GetAll(FormCollection formData)
        {
            try
            {
                GridModel result = new GridModel();

                for (int i = 0; i < formData.AllKeys.Length; i++)
                {
                    if (formData.AllKeys[i] == "NumeroLote")
                    {
                        if ((!(string.IsNullOrEmpty(formData["NumeroLote"].ToString().Trim()))))
                        {
                            if (HttpContext.Session["NumeroLote"].ToString() != formData["NumeroLote"].ToString().Trim())
                            {
                                Session["CodigosBarrasNegocio"] = null;
                            }
                            Session["NumeroLote"] = formData["NumeroLote"].ToString().Trim();
                            Session["Negocio"] = string.Empty;
                            result = new GridModel(ModificarModel.GetAll(formData["NumeroLote"].ToString().Trim(), null));

                        }
                    }
                    else if (formData.AllKeys[i] == "Negocio")
                    {
                        if ((!(string.IsNullOrEmpty(formData["Negocio"].ToString().Trim()))))
                        {
                            if (HttpContext.Session["Negocio"].ToString() != formData["Negocio"].ToString().Trim())
                            {
                                Session["CodigosBarrasNegocio"] = null;
                            }
                            Session["Negocio"] = formData["Negocio"].ToString().Trim();
                            Session["NumeroLote"] = string.Empty;
                            result = new GridModel(ModificarModel.GetAll(null, formData["Negocio"].ToString().Trim()));
                        }
                    }

                }

                return View(result);
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en ModificarController metodo _GetAll " + es.Message + " linea " + es.StackTrace + "Usuario" + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
                throw;
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _EditRow(decimal id)
        {

            try
            {
                ModificarCodigosPropiedades data = ModificarModel.One(p => p.negocio == id);
                TryUpdateModel(data);
                ModificarModel.Update(data);
                var negocio = Session["Negocio"].ToString();
                var Lote = Session["NumeroLote"].ToString();
                return View(new GridModel(ModificarModel.GetAll(Lote, negocio)));
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en ModificarController metodo _EditRow " + es.Message + " linea " + es.StackTrace + "Usuario" + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
                throw;
            }


        }

        #endregion

        #region Anulaciones

        public ActionResult Anular()
        {
            return View();
        }

        public ActionResult ejecutarAnulacion(FormCollection dato)
        {
            for (int i = 0; i < dato.AllKeys.Length; i++)
            {
                if (dato.AllKeys[i] == "txtLabel")
                {
                    if ((!(string.IsNullOrEmpty(dato[dato.AllKeys[i]].ToString().Trim()))))
                    {
                        ModificarModel.anularLabelNegocio(0, dato[dato.AllKeys[i]].ToString().Trim(), 0);
                    }
                }
                else if (dato.AllKeys[i] == "txtNegocio")
                {
                    if ((!(string.IsNullOrEmpty(dato[dato.AllKeys[i]].ToString().Trim()))))
                    {
                        ModificarModel.anularLabelNegocio(decimal.Parse(dato[dato.AllKeys[i]].ToString().Trim()), string.Empty, 2);
                    }
                }
                else if (dato.AllKeys[i] == "txtRechazo")//Se hace el rechazo por que el cliente lo devuelve
                {
                    if ((!(string.IsNullOrEmpty(dato[dato.AllKeys[i]].ToString().Trim()))))
                    {
                        ModificarModel.anularLabelNegocio(0, dato[dato.AllKeys[i]].ToString().Trim(), 3);
                    }
                }
            }

            return View();
        }
        #endregion

        #region Descontabilizar

        public ActionResult Descontabilizacion(GridEditMode? mode, GridButtonType? type, GridInsertRowPosition?
         insertRowPosition)
        {
            ViewData["mode"] = mode ?? GridEditMode.InLine;
            ViewData["type"] = type ?? GridButtonType.Text;
            ViewData["insertRowPosition"] = insertRowPosition ?? GridInsertRowPosition.Top;
            HttpContext.Session["codigoBarras"] = "";
            return View();
        }

        [GridAction]
        public ActionResult _GetAllDescontabilizar(FormCollection formData)
        {
            try
            {
                GridModel result = new GridModel();

                for (int i = 0; i < formData.AllKeys.Length; i++)
                {
                    if (formData.AllKeys[i] == "txtNumeroSAP")
                    {
                        if (((!(string.IsNullOrEmpty(formData[formData.AllKeys[i]].ToString().Trim())))))
                        {

                            if (HttpContext.Session["codigoBarras"].ToString() != formData[formData.AllKeys[i]].ToString().Trim())
                            {
                                Session["datosDescontabilizar"] = null;
                            }
                            Session["codigoBarras"] = formData[formData.AllKeys[i]].ToString().Trim();
                            result = new GridModel(ModificarModel.GetAll(formData[formData.AllKeys[i]].ToString().Trim()));

                        }
                    }
                }

                return View(result);
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en ModificarController metodo _GetAllDescontabilizar " + es.Message + " linea " + es.StackTrace + "Usuario" + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
                throw;
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _EditRowDes(decimal negocio, int captura, int idCampo)
        {
            try
            {
                DescontabilizarPropiedades data = ModificarModel.OneDes(p => p.negocio == negocio && p.captura == captura && p.idCampo == idCampo);
                TryUpdateModel(data);
                ModificarModel.Update(data);
                var codigoBarras = Session["codigoBarras"].ToString();
                return View(new GridModel(ModificarModel.GetAll(codigoBarras)));
            }
            catch (Exception es)
            {
                LogRepository.registro("Error en ModificarController metodo _EditRowDes " + es.Message + " linea " + es.StackTrace + "Usuario" + ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario);
                throw;
            }

        }

        #endregion

        #region AjustesDatos

        public ActionResult AjustesDatos(GridEditMode? mode, GridButtonType? type, GridInsertRowPosition?
         insertRowPosition)
        {
            ViewData["mode"] = mode ?? GridEditMode.InLine;
            ViewData["type"] = type ?? GridButtonType.Text;
            ViewData["insertRowPosition"] = insertRowPosition ?? GridInsertRowPosition.Top;
            HttpContext.Session["negocioAjustar"] = string.Empty;
            return View();
        }

        [GridAction]
        public ActionResult _GetAllAjusteDatos(FormCollection formData)
        {
            try
            {
                GridModel result = new GridModel();
                for (int i = 0; i < formData.AllKeys.Length; i++)
                {
                    if (formData.AllKeys[i] == "txtNegocio")
                    {
                        if (((!(string.IsNullOrEmpty(formData[formData.AllKeys[i]].ToString().Trim())))))
                        {
                            if (HttpContext.Session["negocioAjustar"].ToString() != formData[formData.AllKeys[i]].ToString().Trim())
                            {
                                Session["AjustesDatos"] = null;
                            }
                            Session["negocioAjustar"] = formData[formData.AllKeys[i]].ToString().Trim();
                            result = new GridModel(ModificarModel.GetAll(decimal.Parse(formData[formData.AllKeys[i]].ToString().Trim())));
                        }
                    }
                }

                return View(result);
            }
            catch (Exception es)
            {

                throw;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _EditRowData(int IdCampo,decimal negocio)
        {
            AjusteDatosNegocioPropiedades data = ModificarModel.OneAjuste(p => p.Negocio == negocio && p.IdCampo == IdCampo);
            TryUpdateModel(data);
            ModificarModel.Update(data);
            decimal negocioSession = decimal.Parse(Session["negocioAjustar"].ToString());
            return View(new GridModel(ModificarModel.GetAll(negocio)));
        }

        #endregion

    }
}
