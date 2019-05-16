using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    /// <summary>
    /// CREADOR
    /// Autor:  
    /// Fecha:
    /// Descripcion:
    /// MODIFICAR
    /// Actor:      Elena Parra
    /// Fecha:      20-12-2013     
    /// Descripcion:Cree el metodo que consulta la ruta de origen del cada cliente
    /// </summary>
    public class CargueController : Controller
    {
        private GestorDocumentalEnt gd;

        public void ActualizarLotesCargados(CargueLotes lotes)
        {
            try
            {
                this.gd = new GestorDocumentalEnt();
                this.gd.spActualizarLotesCargados(lotes.Lote, new int?(lotes.ConsecutivoLote), new decimal?(lotes.CliId));
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CargueController metodo ActualizarLotesCargados " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
        }

        public void borrarArchivosDirectorio(DirectoryInfo directory)
        {
            try
            {
                foreach (FileInfo info in directory.GetFiles())
                {
                    info.Delete();
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CargueController metodo borrarArchivosDirectorio " + exception.Message + " stack trace " + exception.StackTrace);
                throw;
            }
            
        }

        public int? ContarArchivosLote(CargueLotes lotes)
        {
            int? nullable2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                //nullable2 = this.gd.spObtenerCatidadArchivosLote(lotes.Lote, new int?(lotes.ConsecutivoLote), new decimal?(lotes.CliId)).First<int?>();
                nullable2 = this.gd.spObtenerCatidadArchivosLote(lotes.Lote, new int?(lotes.ConsecutivoLote), new decimal?(lotes.CliId));
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CargueController metodo ContarArchivosLote " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return nullable2;
        }

        public void CrearCarpetaNegocio(string pathO, string pathD, CargueLotes L)
        {
            try
            {
                IQueryable<CargueLotes> queryable = from l in this.gd.CargueLotes
                                                    where (((l.Lote == L.Lote) && (l.ConsecutivoLote == L.ConsecutivoLote)) && (l.CliId == L.CliId)) && !l.ArchivoCargado
                                                    select l;
                foreach (CargueLotes lotes in queryable)
                {
                    DirectoryInfo info = new DirectoryInfo(pathD + @"\" + lotes.NegId.ToString());
                    if (!info.Exists)
                    {
                        info.Create();
                    }
                    System.IO.File.Move(pathO + "/" + lotes.NomArchivo, pathD + "/" + lotes.NegId.ToString() + "/" + lotes.NegId.ToString() + ".tif");
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CargueController metodo CrearCarpetaNegocio " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
        }

        public void InsertarCargueLotes(List<CargueLotes> lstCargue)
        {
            try
            {
                this.gd = new GestorDocumentalEnt();
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (CargueLotes lotes in lstCargue)
                    {
                        this.gd.AddToCargueLotes(lotes);
                        this.gd.SaveChanges();
                    }
                    scope.Complete();
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CargueController metodo InsertarCargueLotes " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
        }

        public List<CargueLotes> obtenerArchivoCSV(string path, Usuarios U, Clientes C)
        {
            List<CargueLotes> list3;
            try
            {
                this.gd = new GestorDocumentalEnt();
                List<CargueLotes> list = new List<CargueLotes>();
                StreamReader reader = new StreamReader(path);
                string lote = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                int? nullable = (from l in this.gd.CargueLotes
                                 where (l.Lote == lote) && (l.CliId == C.CliNit)
                                 select l.ConsecutivoLote).Max() + 1;
                int num2 = Convert.ToInt32(nullable);
                while (!reader.EndOfStream)
                {
                    string[] strArray = reader.ReadLine().Split(new char[] { ';' });
                    CargueLotes item = new CargueLotes
                    {
                        LoteFecha = Convert.ToDateTime(strArray[0].ToString())
                    };
                    int upperBound = strArray[1].ToString().Split(new char[] { '\\' }).GetUpperBound(0);
                    item.NomArchivo = strArray[1].ToString().Split(new char[] { '\\' }).GetValue(upperBound).ToString();
                    item.Paginas = Convert.ToInt32(strArray[2].ToString());
                    item.FechaCargue = DateTime.Now;
                    item.Lote = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                    item.Usuario = U.IdUsuario;
                    item.ConsecutivoLote = (num2 == 0) ? 1 : num2;
                    item.CliId = C.CliNit;
                    list.Add(item);
                }
                var source = from b in list
                             group b by new { NomArchivo = b.NomArchivo, lfecha = b.LoteFecha.ToString("MM/dd/yyyy"), Lote = b.Lote, ConsecutivoLote = b.ConsecutivoLote } into g
                             select new { NomArchivo = g.Key.NomArchivo, lfecha = g.Key.lfecha, Lote = g.Key.Lote, ConsecutivoLote = g.Key.ConsecutivoLote, cuenta = g.Count<CargueLotes>() };
                List<CargueLotes> list2 = new List<CargueLotes>();
                foreach (var type in source.ToList())
                { 
                    CargueLotes lotes2 = new CargueLotes
                    {
                        NomArchivo = type.NomArchivo,
                        Lote = type.Lote,
                        Paginas = type.cuenta,
                        ConsecutivoLote = type.ConsecutivoLote,
                        FechaCargue = DateTime.Now,
                        Usuario = U.IdUsuario,
                        LoteFecha = Convert.ToDateTime(type.lfecha),
                        CliId = C.CliNit
                    };
                    list2.Add(lotes2);
                }
                list3 = list2;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CargueController metodo obtenerArchivoCSV " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list3;
        }

        public List<CargueLotes> obtenerLoteCargado(CargueLotes cl)
        {
            List<CargueLotes> list2;
            List<CargueLotes> list = new List<CargueLotes>();
            try
            {
                this.gd = new GestorDocumentalEnt();
                var queryable = from c in this.gd.CargueLotes
                                group c by new { Lote = c.Lote, ConsecutivoLote = c.ConsecutivoLote, LoteFecha = c.LoteFecha, NomArchivo = c.NomArchivo } into g
                                select new { Lote = g.Key.Lote, ConsecutivoLote = g.Key.ConsecutivoLote, LoteFecha = g.Key.LoteFecha, NomArchivo = g.Key.NomArchivo, Paginas = g.Count<CargueLotes>() };
                list2 = list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CargueController metodo obtenerLoteCargado " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public string ValidarNombresArchivos(string path, CargueLotes L)
        {
            string str2;
            try
            {
                string str = "";
                IQueryable<string> queryable = from l in this.gd.CargueLotes
                                               where ((l.Lote == L.Lote) && (l.ConsecutivoLote == L.ConsecutivoLote)) && (l.CliId == L.CliId)
                                               select l.NomArchivo;
                using (IEnumerator<string> enumerator = queryable.GetEnumerator())
                {
                    Func<FileInfo, bool> predicate = null;
                    string n;
                    while (enumerator.MoveNext())
                    {
                        n = enumerator.Current;
                        DirectoryInfo info = new DirectoryInfo(path);
                        if (predicate == null)
                        {
                            predicate = i => i.Name == n;
                        }
                        if ((from i in info.GetFiles("*.*", SearchOption.AllDirectories).Where<FileInfo>(predicate) select i.Name).ToArray<string>().Count<string>() == 0)
                        {
                            return n.ToString();
                        }
                    }
                }
                str2 = str;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CargueController metodo ValidarNombresArchivos " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return str2;
        }

        /// <summary>
        /// Obtiene la ruta las carpetas de los lotes de cada cliente
        /// </summary>
        /// <param name="usuarioLog">Usuario que esta logueado</param>
        public string DirectorioLotes(Clientes cliente) 
        {
            try
            {
                gd = new GestorDocumentalEnt();
                return gd.Clientes.Where(x => x.CliNit == cliente.CliNit).First().RutaOrigen;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en CargueController metodo DirectorioLotes " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            
        }
    }
}
