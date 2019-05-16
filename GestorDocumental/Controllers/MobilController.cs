using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;
using Telerik.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class MobilController : Controller
    {
        //
        // GET: /Mobil/

        MobilModel mobilModel = new MobilModel();
        ConsecutivosModel consecutivosModel = new ConsecutivosModel();
        public ActionResult Index()
        {
            ViewData["Documentos"] = mobilModel.GetDocumentos();
            ViewData["listaChequeo"] =  mobilModel.GetListaChequeo(1);
            ViewData["id"] = 1;
            Session["ID_MOBIL"] = consecutivosModel.getConsecutivo("RAD_MOB");
            ViewData["NomUsuario"] = ((Usuarios)Session["USUARIO_LOGUEADO"]).NomUsuario;
            return View();
        }

        [GridAction]
        public ActionResult _SelectionClientSide_Lista(string documentoID)
        {
            documentoID = documentoID ?? "1";
            return View(new GridModel
            {
                Data = mobilModel.GetListaChequeo(int.Parse(documentoID))
            });
        }

        public ActionResult _SelectionClientSide_Documentos()
        {
            return View(new GridModel
            {
                Data = mobilModel.GetDocumentos()
            });
        }

        [HttpPost]
        public ActionResult Guardar(FormCollection data) 
        {
            List<MobCaptura> newListCaptura = new List<MobCaptura>();
            var idRad = Session["ID_MOBIL"];
            var datos = data["checked"].Split(',');
            foreach (var idListChek in datos)
            {
                MobCaptura capturaList = new MobCaptura();
                capturaList.idListaChequeo = long.Parse(idListChek);
                capturaList.numRad = int.Parse(Session["ID_MOBIL"].ToString());
                capturaList.idUsuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                capturaList.fecha = DateTime.Now;
                newListCaptura.Add(capturaList);
            }

        //   mobilModel.addMobCaptura(newListCaptura);


            return Content("<script language='javascript' type='text/javascript'>   window.location.href = '/Mobil/Index';  </script>");
        }
    }
}
