using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class MobileController : Controller
    {
        MobileModel mobileModel = new MobileModel();
        ConsecutivosModel consecutivosModel = new ConsecutivosModel();
        public ActionResult Index()
        {
            if (Session["CLIENTE"] != null)
            {
                ViewData["Documentos"] = mobileModel.GetDocumentos();
                ViewData["listaChequeo"] = mobileModel.GetListaChequeo(1);
                ViewData["id"] = 1;
                Session["ID_MOVIL"] = ConsecutivosModel.getConsecutivo("RAD_MOB");
                ViewData["NomUsuario"] = ((Usuarios)Session["USUARIO_LOGUEADO"]).NomUsuario;
                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        [GridAction]
        public ActionResult _SelectionClientSide_Lista(string documentoID)
        {
            documentoID = documentoID ?? "1";
            return View(new GridModel
            {
                Data = mobileModel.GetListaChequeo(int.Parse(documentoID))
            });
        }

        public ActionResult _SelectionClientSide_Documentos()
        {
            return View(new GridModel
            {
                Data = mobileModel.GetDocumentos()
            });
        }

        [HttpPost]
        public ActionResult Guardar(FormCollection data)
        {
            List<MobCaptura> newListCaptura = new List<MobCaptura>();
            var idRad = Session["ID_MOVIL"];
            var datos = data["checked"].Split(',');
            foreach (var idListChek in datos)
            {
                MobCaptura capturaList = new MobCaptura();
                capturaList.idListaChequeo = long.Parse(idListChek);
                capturaList.numRad = int.Parse(Session["ID_MOVIL"].ToString());
                capturaList.idUsuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                capturaList.fecha = DateTime.Now;
                newListCaptura.Add(capturaList);
            }
            mobileModel.addMobCaptura(newListCaptura);
            return Content("<script language='javascript' type='text/javascript'>   window.location.href = '/MobDatos/Index';  </script>");
        }
    }
}
