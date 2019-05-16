using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    /// <summary>
    /// Autor: Elena Parra 
    /// Fecha: 21 - 10 - 2013 
    /// Descripcion: Controla la interfaz del cruce de identidad 
    /// </summary>
    public class CruceIdentidadController : Controller
    {
        public ActionResult Index()
        {
            if (Session["CLIENTE"] != null)
            {
                Session["TITULO"] = "Recepción cruce de identidad";
                ViewData["menu"] = "";
                return View();
            }
            else
            {
                Response.Redirect("../Seguridad/Login");
                return null;
            }
        }

        public ActionResult GuardarArchivo(DateTime? fechaRecepcion, String observaciones)
        {
            try
            {
                Verif_DataModelo verifDataModelo = new Verif_DataModelo();
                Verif_Datos_Maestra VerifDatosMaestra = new Verif_Datos_Maestra();

                VerifDatosMaestra.VDM_FechaProceso = fechaRecepcion ?? DateTime.Now;
                VerifDatosMaestra.VDM_Observaciones = observaciones;
                VerifDatosMaestra.VDM_IdUsuario = ((Usuarios)Session["USUARIO_LOGUEADO"]).IdUsuario;

                var idVDM = verifDataModelo.AddVerif_Datos_Maestra(VerifDatosMaestra);

                var cantidadRegistros = LeerArchivo((int)idVDM, verifDataModelo);

                verifDataModelo.UpdateVerif_Datos_Maestra(idVDM, cantidadRegistros);

                verifDataModelo.CruceDeIdentidad();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CruceIdentidadController metodo GuardarArchivo" + exception.Message + " stack " + exception.StackTrace);
                throw;
            }


            return View();
        }

        /// <summary>
        /// Guarda el archivo en una ruta especifica para porder leerlo 
        /// </summary>
        /// <param name="attachments"></param>
        /// <returns></returns>
        public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> attachments)
        {

            try
            {

                // The Name of the Upload component is "attachments" 
                foreach (var file in attachments)
                {
                    // Some browsers send file names with full path. This needs to be stripped.
                    var fileName = Path.GetFileName(file.FileName);
                    var reNameFile = RenombrarArchivo(fileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data/CargeIdentidad"), reNameFile);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                    Session["CURRENT_FILE"] = physicalPath;
                }
                // Return an empty string to signify success
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en RecepcionController metodo SaveFile " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }

            return Content("");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdVDM"></param>
        public long LeerArchivo(int IdVDM, Verif_DataModelo verifDataModelo)
        {
            try
            {
                string path = Session["CURRENT_FILE"].ToString();
                long contador = 0;
                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    while ((line = readFile.ReadLine()) != null)
                    {
                        row = line.Split(';');

                        Verif_Datos verifDatos = new Verif_Datos();
                        verifDatos.VD_IdVDM = IdVDM;
                        verifDatos.VD_TipoDocumento = row[0] == null || row[0] == string.Empty ? "" : row[0].ToLower();
                        verifDatos.VD_NumDocumento = row[1] == null || row[1] == string.Empty ? "" : row[1].ToLower();
                        verifDatos.VD_PNombre = row[2] == null || row[2] == string.Empty ? "" : row[2].ToLower();
                        verifDatos.VD_SNombre = row[3] == null || row[3] == string.Empty ? "" : row[3].ToLower();
                        verifDatos.VD_PApellido = row[4] == null || row[4] == string.Empty ? "" : row[4].ToLower();
                        verifDatos.VD_SApellido = row[5] == null || row[5] == string.Empty ? "" : row[5].ToLower();
                        verifDatos.VD_FechaNacimiento = row[6] == null || row[6] == string.Empty ? "" : row[6];
                        verifDatos.VD_Genero = row[7] == null || row[7] == string.Empty ? "" : row[7].ToLower();
                        verifDatos.VD_Llave1 = verifDatos.VD_PNombre + verifDatos.VD_PApellido;
                        verifDatos.VD_Llave2 = verifDatos.VD_PNombre + verifDatos.VD_SApellido;
                        verifDatos.VD_Llave3 = verifDatos.VD_SNombre + verifDatos.VD_SApellido;
                        verifDatos.VD_Llave4 = verifDatos.VD_SNombre + verifDatos.VD_PApellido;

                        verifDataModelo.AddVerif_Datos(verifDatos);
                        contador++;
                    }
                }

                return contador;
            }
            catch (Exception e) 
            {

                LogRepository.registro("Error en RecepcionController metodo LeerArchivo " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }

           

        }

        /// <summary>
        /// Re nombre el archivo  y mantiene su extencion 
        /// </summary>
        /// <param name="nameFile"> nombre del archivo</param>
        /// <returns>archivo renombrado</returns>
        protected string RenombrarArchivo(string nameFile)
        {
            try
            {
                ConsecutivosModel consecutivo = new ConsecutivosModel();
                int consecutivoArchivo = ConsecutivosModel.getConsecutivo("CARGA_IDENTIDAD");
                var arryString = nameFile.Split('.');
                return arryString[0] + "_" + consecutivoArchivo + "." + arryString[1];
            }
            catch (Exception e)
            {
                LogRepository.registro("Error en RecepcionController metodo RenombrarArchivo " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }
           
        }
    }
}
