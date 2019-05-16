namespace BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Transactions;
    using BO;
    using System.Diagnostics;

    public class BLLCargue
    {

        //private string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private string strCon = "";
        private BasdatDataContext bd;
        private EventLog log;
        //data source=12.109.8.55;initial catalog=GestorDocumental;persist security info=True;user id=Bizagi;password=Everis1
        private int COLUMNA_NOMBRE;
        private string EXTENSION_ARCHIVO;

        public BLLCargue(string conexion, EventLog log)
        {
            this.log = log;
            strCon = conexion;
            bd = new BasdatDataContext(conexion);
        }

        /// <summary>
        /// Lotes pendientes por cada uno de los clientes
        /// </summary>
        /// <returns></returns>
        public List<BOClientes> LotesPendientesPorCliente()
        {
            try
            {
                List<BOClientes> listaClientes = new List<BOClientes>();
                using (SqlConnection connection = new SqlConnection(this.strCon))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        connection.Open();
                        command.CommandText = "spLotesPendientesPorClientes";
                        command.CommandType = CommandType.StoredProcedure;

                        SqlDataReader result = command.ExecuteReader();

                        while (result.Read())
                        {
                            BOClientes cliente = new BOClientes();
                            cliente.ContidadLotes = result.GetInt32(result.GetOrdinal("LotesPendientes"));
                            cliente.Cliente = result.GetDecimal(result.GetOrdinal("Nitcliente"));
                            cliente.RutaOrigen = result.GetString(result.GetOrdinal("RutaOrigen"));
                            listaClientes.Add(cliente);
                        }

                        result.Close();
                    }
                    connection.Close();
                }
                return listaClientes;
            }
            catch (Exception exception)
            {
                log.WriteEntry("Error en LotesPendientesPorCliente  " + exception.Message, EventLogEntryType.Error);
                throw exception;
            }
        }

        private void ActualizarCargueARchivos(decimal Negocio)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.strCon))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        connection.Open();
                        command.CommandText = "spActualizarArchivoCargado";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@NegId", SqlDbType.Decimal);
                        command.Parameters["@NegId"].Value = Negocio;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exception)
            {
                log.WriteEntry("Error en BLLCargue  " + exception.Message, EventLogEntryType.Error);
                throw exception;
            }
        }

        private BOCargueLotes cargarNegociosSinArchivo(SqlDataReader Lector)
        {
            return new BOCargueLotes { Negocio = Convert.ToDecimal(Lector["NegId"]), NomArchivo = Lector["NomArchivo"].ToString(), LoteScaner = Lector["LoteScaner"].ToString() };
        }

        public void CraerDirectoriosNegocios(string sourcePath, string destinationPath)
        {
            try
            {
                List<BOCargueLotes> list = this.ObtenerNegociosSinCargueArchivo();
                foreach (BOCargueLotes lotes in list)
                {
                    Directory.CreateDirectory(destinationPath + @"\" + lotes.Negocio.ToString());
                    String origen = (sourcePath + @"\" + lotes.LoteScaner + @"\" + lotes.NomArchivo).Trim(new char[] { '"' });
                    String origenPDF = (sourcePath + @"\" + lotes.LoteScaner + @"\" + this.cabiarExtTifAPdf(lotes.NomArchivo)).Trim(new char[] { '"' });
                    // origen = origen.Substring(0, (origen.Length - 1));
                    //log.WriteEntry("ARCHIVOS DE ORIGEN   " + origen,EventLogEntryType.Information);

                    if (File.Exists(origen))
                    {

                        String destino = string.Concat(new object[] { destinationPath, @"\", lotes.Negocio.ToString(), @"\", lotes.Negocio, ".TIF" });
                        File.Move(origen, destino);
                        this.ActualizarCargueARchivos(lotes.Negocio);

                    }
                    if (File.Exists(origenPDF))
                    {
                        String destinoPDF = string.Concat(new object[] { destinationPath, @"\", lotes.Negocio.ToString(), @"\", lotes.Negocio, ".pdf" });
                        File.Move(origenPDF, destinoPDF);

                    }
                }
                foreach (BOCargueLotes lotes2 in list)
                {
                    if (Directory.Exists(sourcePath + @"\" + lotes2.LoteScaner))
                    {
                        Directory.Delete(sourcePath + @"\" + lotes2.LoteScaner, true);
                    }
                }
            }
            catch (Exception exception)
            {
                log.WriteEntry("Error en BLLCargue  " + exception.Message, EventLogEntryType.Error);
                throw exception;
            }
        }

        public void InsertarCargueLotes(List<BOCargueLotes> Lst)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.strCon))
                {
                    connection.Open();
                    using (TransactionScope scope = new TransactionScope())
                    {
                        foreach (BOCargueLotes lotes in Lst)
                        {
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandText = "spInsertarCargueLotes";
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add("@Cli_Id", SqlDbType.Decimal);
                                command.Parameters["@Cli_Id"].Value = lotes.Cliente;
                                command.Parameters.Add("@Lote", SqlDbType.VarChar);
                                command.Parameters["@Lote"].Value = lotes.Lote;
                                command.Parameters.Add("@ConsecutivoLote", SqlDbType.Int);
                                command.Parameters["@ConsecutivoLote"].Value = lotes.ConsecutivoLote;
                                command.Parameters.Add("@LoteFecha", SqlDbType.DateTime);
                                command.Parameters["@LoteFecha"].Value = lotes.LoteFecha;
                                command.Parameters.Add("@Paginas", SqlDbType.Int);
                                command.Parameters["@Paginas"].Value = lotes.Paginas;

                                command.Parameters.Add("@CodBarras", SqlDbType.NVarChar);
                                command.Parameters["@CodBarras"].Value = lotes.CodBarras;

                                command.Parameters.Add("@idRecepcion", SqlDbType.Int);
                                command.Parameters["@idRecepcion"].Value = lotes.idRecepcion;
                                command.Parameters.Add("@prioridad", SqlDbType.Int);
                                command.Parameters["@prioridad"].Value = 1;

                                command.Parameters.Add("@FechaCargue", SqlDbType.DateTime);
                                command.Parameters["@FechaCargue"].Value = lotes.FechaCargue;
                                command.Parameters.Add("@Usuario", SqlDbType.Decimal);
                                command.Parameters["@Usuario"].Value = lotes.Usuario;
                                command.Parameters.Add("@LoteScaner", SqlDbType.VarChar);
                                command.Parameters["@LoteScaner"].Value = lotes.LoteScaner;
                                command.Parameters.Add("@NomArchivo", SqlDbType.VarChar);
                                command.Parameters["@NomArchivo"].Value = lotes.NomArchivo;
                                command.ExecuteNonQuery();
                            }
                        }
                        scope.Complete();
                    }
                }
            }
            catch (Exception exception)
            {
                log.WriteEntry("Error en BLLCargue  " + exception.Message, EventLogEntryType.Error);
                throw exception;
            }
        }

        public void InsertarEtapaInicial(BOAsignacionTareas at)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.strCon))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        connection.Open();
                        command.CommandText = "spInsertarEtapainicial";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@Negocio", SqlDbType.Decimal);
                        command.Parameters["@Negocio"].Value = at.Negocio;
                        command.Parameters.Add("@Etapa", SqlDbType.Int);
                        command.Parameters["@Etapa"].Value = at.Etapa;
                        command.Parameters.Add("@Estado", SqlDbType.Int);
                        command.Parameters["@Estado"].Value = at.Estado;
                        command.Parameters.Add("@HoraInicio", SqlDbType.DateTime);
                        command.Parameters["@HoraInicio"].Value = DateTime.Now;
                        command.Parameters.Add("@HoraTerminacion", SqlDbType.DateTime);
                        command.Parameters["@HoraTerminacion"].Value = DateTime.Now;
                        command.Parameters.Add("@Usuario", SqlDbType.Decimal);
                        command.Parameters["@Usuario"].Value = at.Usuario;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exception)
            {
                log.WriteEntry("Error en BLLCargue  " + exception.Message, EventLogEntryType.Error);
                throw exception;
            }
        }

        public void ObtenerArchivoCSV(string Location, decimal nitCli, decimal idUser)
        {
            try
            {
                //EN CASO DE QUE OMITA LA RECEPCION ESTO SON LOS DATOS POR DEFAULT
                decimal nitCliente = nitCli;
                decimal idUsuario = idUser;

                DirectoryInfo info = new DirectoryInfo(Location);
                foreach (DirectoryInfo info2 in info.GetDirectories())
                {

                    long idRecepcion = 0;
                    decimal numeroLote = 0;

                    //SI ENCUENTRA UNA CARPETA CON STRINGS LA OMITE
                    try
                    {
                        numeroLote = Convert.ToDecimal(info2.Name);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    int omiteRecepcion = Int16.Parse(bd.Parametros.Where(c => c.codigo == "OMITE_RECEP").First().valor);

                    //OMITO EL MODULO DE RECEPCION
                    if (omiteRecepcion == 0)
                    {
                        int reg_recep = bd.Recepcion.Where(c => c.numeroLote == numeroLote && c.estado == 0 && c.activo == true).Count();
                        //BUSCO QUE EL LOTE HAYA SIDO RECEPCIONADO DE LO CONTRARIO NO LO CARGA
                        if (reg_recep > 0)
                        {
                            Recepcion reg_recepcion = bd.Recepcion.Where(c => c.numeroLote == numeroLote && c.estado == 0 && c.activo == true).First();
                            idRecepcion = reg_recepcion.id;

                            reg_recepcion.estado = 1;
                            reg_recepcion.fechaCargue = DateTime.Now;

                            nitCliente = (decimal)reg_recepcion.nitCliente;
                            idUsuario = (decimal)reg_recepcion.idUsuario;

                            bd.SubmitChanges();
                        }
                        else
                        {
                            continue;
                        }

                    }

                    foreach (FileInfo info3 in info2.GetFiles("*.CSV"))
                    {
                        var source = from b in this.ObtenerRegistrosArchivoCSV(info3.FullName, Path.GetFileName(info3.Name), nitCliente, info2.Name, idUsuario)
                                     group b by new { NomArchivo = b.NomArchivo, lfecha = b.LoteFecha.ToString("MM/dd/yyyy"), Lote = b.Lote, ConsecutivoLote = b.ConsecutivoLote, CodBarras = b.CodBarras } into g
                                     select new { NomArchivo = g.Key.NomArchivo, lfecha = g.Key.lfecha, Lote = g.Key.Lote, ConsecutivoLote = g.Key.ConsecutivoLote, cuenta = g.Count<BOCargueLotes>(), CodBarras = g.Key.CodBarras };
                        List<BOCargueLotes> lst = new List<BOCargueLotes>();
                        foreach (var type in source.ToList())
                        {
                            BOCargueLotes item = new BOCargueLotes
                            {
                                NomArchivo = type.NomArchivo,
                                Lote = type.Lote,
                                Paginas = type.cuenta,
                                ConsecutivoLote = type.ConsecutivoLote,
                                FechaCargue = DateTime.Now,
                                Usuario = idUsuario,
                                idRecepcion = idRecepcion,
                                CodBarras = type.CodBarras,
                                //LoteFecha = Convert.ToDateTime(type.lfecha),
                                LoteFecha = DateTime.Now,
                                Cliente = nitCliente,
                                LoteScaner = info2.Name
                            };
                            lst.Add(item);
                        }
                        this.InsertarCargueLotes(lst);
                    }
                }
            }
            catch (Exception exception)
            {
                log.WriteEntry("Error en BLLCargue  " + exception.Message, EventLogEntryType.Error);
                throw;
            }

        }

        private int ObtenerConsecutivoLote(string Lote)
        {
            int num2;
            try
            {
                using (SqlConnection connection = new SqlConnection(this.strCon))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        connection.Open();
                        command.CommandText = "spObtenerConsecutivoLote";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@Lote", SqlDbType.VarChar);
                        command.Parameters["@Lote"].Value = Lote;
                        num2 = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception exception)
            {
                log.WriteEntry("Error en BLLCargue  " + exception.Message, EventLogEntryType.Error);
                throw exception;
            }
            return num2;
        }

        public List<BOCargueLotes> ObtenerNegociosSinCargueArchivo()
        {
            List<BOCargueLotes> list2;
            try
            {
                List<BOCargueLotes> list = new List<BOCargueLotes>();
                SqlDataReader lector = null;
                using (SqlConnection connection = new SqlConnection(this.strCon))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        connection.Open();
                        command.CommandText = "spObtenerLotesSinCargueArchivo";
                        command.CommandType = CommandType.StoredProcedure;
                        lector = command.ExecuteReader();
                        while (lector.Read())
                        {
                            list.Add(this.cargarNegociosSinArchivo(lector));
                        }
                    }
                }
                list2 = list;
            }
            catch (Exception exception)
            {
                log.WriteEntry("Error en BLLCargue  " + exception.Message, EventLogEntryType.Error);
                throw exception;
            }
            return list2;
        }

        public List<BOCargueLotes> ObtenerRegistrosArchivoCSV(string path, string fileName, decimal Cliente, string LoteScaner, decimal idUsuario)
        {


            List<BOCargueLotes> list2;
            try
            {
                StreamReader reader = new StreamReader(path);
                //Obtenemos la informacion de los parametros de :  columna donde se encuentra el nombre del archivo  y la exttension que se usara al escanear
                COLUMNA_NOMBRE = int.Parse((bd.Parametros.Where(c => c.codigo == "COLUMNA_NOMBRE").First().valor));
                EXTENSION_ARCHIVO = (bd.Parametros.Where(c => c.codigo == "EXTENSION_ARCHIVO").First().valor).ToString();

                List<BOCargueLotes> list = new List<BOCargueLotes>();
                string lote = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                int num2 = this.ObtenerConsecutivoLote(lote);
                while (!reader.EndOfStream)
                {
                    string[] strArray = reader.ReadLine().Split(new char[] { ';' });
                    BOCargueLotes item = new BOCargueLotes
                    {
                        //LoteFecha = Convert.ToDateTime(strArray[0].ToString())
                        LoteFecha = Convert.ToDateTime(DateTime.Now)
                    };
                    //int upperBound = strArray[1].ToString().Split(new char[] { '\\' }).GetUpperBound(0);
                    //item.NomArchivo = strArray[1].ToString().Split(new char[] { '\\' }).GetValue(upperBound).ToString();
                    item.NomArchivo = strArray[COLUMNA_NOMBRE].Trim(new char[] { '"' }) + EXTENSION_ARCHIVO;
                    //item.NomArchivo = strArray[2] + ".tif";
                    //item.Paginas = Convert.ToInt32(strArray[2].ToString());

                    //ITEM DEL CODIGO DE BARRAS
                    item.CodBarras = (strArray[5]).ToString().Replace('"', ' ').Trim();
                    item.Paginas = 5;
                    item.FechaCargue = DateTime.Now;
                    item.Lote = lote;
                    item.Usuario = idUsuario;
                    item.ConsecutivoLote = num2;
                    item.Cliente = Cliente;
                    item.LoteScaner = LoteScaner;
                    list.Add(item);
                }
                reader.Close();
                list2 = list;
            }
            catch (Exception exception)
            {
                log.WriteEntry("Error en BLLCargue  " + exception.Message, EventLogEntryType.Error);
                throw exception;
            }

            return list2;
        }

        private void TraceService(string content)
        {
            FileStream stream = new FileStream(@"C:\ScheduledService.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.BaseStream.Seek(0L, SeekOrigin.End);
            writer.WriteLine(content);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Cambia le extencion del archivo a .PDF
        /// </summary>
        /// <param name="nameFile"> nombre del archivo</param>
        /// <returns>archivo renombrado</returns>
        protected string cabiarExtTifAPdf(string nameFile)
        {
            try
            {
                var arryString = nameFile.Split('.');
                return arryString[0] + ".pdf";
            }
            catch (Exception e)
            {
                log.WriteEntry("Error en RecepcionController metodo RenombrarArchivo " + e.Message + " stack trace " + e.StackTrace);
                throw;
            }

        }
    }
}

