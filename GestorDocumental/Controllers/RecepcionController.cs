using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class RecepcionController : Controller
    {
        ConciliacionModel modeloConciliacion = new ConciliacionModel();

        //Index
        public ActionResult Index()
        {
            if (Session["CLIENTE"] != null)
            {
                Session["TITULO"] = "Recepción de documentos";
                Session["CURRENT_FILE"] = "";
                ViewData["menu"] = "";
                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        //LEO EL ARCHIVO CSV Y LO CARGO A LA TABLA
        public void leerCSV(string path, int idRecepcion)
        {
            try
            {
                ConciliacionModel modelo = new ConciliacionModel();
                List<Conciliacion> data = new List<Conciliacion>();

                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    while ((line = readFile.ReadLine()) != null)
                    {
                        row = line.Split(';');

                        int clave = modelo.getConsecutivo();

                        for (int i = 0; i < row.Length; i++)
                        {
                            Conciliacion registro = new Conciliacion();
                            registro.campo = row[i];
                            registro.fechaCargue = DateTime.Now;
                            registro.idRecepcion = idRecepcion;
                            registro.orden = i + 1;
                            registro.consecutivo = clave;
                            data.Add(registro);
                        }


                    }
                }
                modeloConciliacion.insertMultiple(data);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RecepcionController metodo leerCSV " + exception.Message);
                throw;
            }

            //return parsedData;
        }

        public ActionResult GuardarRecepcion(int? DropDownList_SubGrupos, int principales, String observaciones, int? DropDownList_Grupos, int Anexos, int? DropDownList_Oficinas)
        {
            try
            {
                RecepcionModel modelo = new RecepcionModel();
                int gruId = DropDownList_SubGrupos ?? 0;

                if (gruId == 0)
                {
                    gruId = DropDownList_Grupos ?? 0;
                }

                //if (gruId == 0)
                //{
                //    gruId = DropDownList_Clientes;
                //}

                ConsecutivosModel consecutivo = new ConsecutivosModel();
                Recepcion data = new Recepcion();

                data.subgrupo = gruId;
                data.nitCliente = ((GestorDocumental.Models.Clientes)Session["CLIENTE"]).CliNit;
                data.rutaArchConciliacion = Session["CURRENT_FILE"].ToString();
                data.principales = principales;
                data.idUsuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;
                data.activo = true;
                data.estado = 99;
                data.numeroLote = ConsecutivosModel.getConsecutivo("LOTES");
                data.fechaRecepcion = DateTime.Now;
                data.observaciones = observaciones;

                long id_lote = modelo.insert(data);
                if (!String.IsNullOrEmpty(Session["CURRENT_FILE"].ToString()))
                {
                    //INSERTO LOS DATOS EN LA TABLA DE CONCILIACION Y LE PASO EL ID DE RECEPCION PARA LIGAR LOS DATOS
                    this.leerCSV(Session["CURRENT_FILE"].ToString(), modelo.idRecepcion);
                }

                ViewData["numero_lote"] = id_lote;


                Recepcion_Detalle data1 = new Recepcion_Detalle();


                data1.DET_Anexo = Anexos;
                data1.DET_idrecepcion = modelo.idRecepcion;
                data1.DET_Oficina = DropDownList_Oficinas;
                modelo.insert_detalle(data1);

            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RecepcionController metodo GuardarRecepcion " + exception.Message);
                throw;
            }

            //base.Response.Redirect("/ViewsAspx/WebForm2.aspx?subgrupo="+DropDownList_SubGrupos);
            return View();
        }

        public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> attachments)
        {

            try
            {
                // The Name of the Upload component is "attachments" 
                foreach (var file in attachments)
                {
                    // Some browsers send file names with full path. This needs to be stripped.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data/Recepcion"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                    Session["CURRENT_FILE"] = physicalPath;
                }
                // Return an empty string to signify success
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en RecepcionController metodo SaveFile " + e.Message);
                throw;
            }

            return Content("");
        }

        [HttpPost]
        public JsonResult _GetDropDownListProductos(int? DropDownList_Clientes)
        {
            try
            {
                DropDownList_Clientes = (int)((Clientes)Session["CLIENTE"]).CliNit;
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().Grupos.Where(c => c.CliNit == DropDownList_Clientes).ToList(), "GruId", "GruDescripcion"),

                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RecepcionController metodo _GetDropDownListProductos " + exception.Message);
                throw;
            }

        }

        [HttpPost]
        public JsonResult _GetDropDownListSubProductos(int? DropDownList_Grupos)
        {
            try
            {
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().Grupos.Where(c => c.GruIdPadre == DropDownList_Grupos).ToList(), "GruId", "GruDescripcion"),
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RecepcionController metodo _GetDropDownListSubProductos " + exception.Message);
                throw;
            }

        }

        //Modificado Diego Ruiz Inicio
        [HttpPost]
        public JsonResult _GetDropDownList_Oficinas(int? DropDownList_Clientes)
        {
            try
            {
                DropDownList_Clientes = (int)((Clientes)Session["CLIENTE"]).CliNit;
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().P_Oficinas.Where(c => c.OFI_CodNit == DropDownList_Clientes).ToList(), "OFI_IdOficina", "OFI_Nombre"),

                                JsonRequestBehavior.AllowGet);
            }

            catch (Exception exception)
            {
                LogRepository.registro("Error en RecepcionController metodo _GetDropDownList_Oficinas " + exception.Message);
                throw;
            }

        }

        //Modificado Diego Ruiz Fin
        [HttpPost]
        public JsonResult _GetDropDownList_tienda()
        {
            try
            {
                return Json(new SelectList(new GestorDocumental.Models.GestorDocumentalEnt().CodigosCampo.Where(c => c.CampId == 57).OrderBy(c => c.CodDescripcion).ToList(), "CodCampId", "CodDescripcion"),
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en RecepcionController metodo _GetDropDownList_tienda " + exception.Message);
                throw;
            }
        }

        public int? DropDownList_Oficinas { get; set; }
    }
}