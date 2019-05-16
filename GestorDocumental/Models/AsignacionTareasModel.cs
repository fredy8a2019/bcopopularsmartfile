using System;
using System.Linq;
using GestorDocumental.Controllers;

namespace GestorDocumental.Models
{
    public class AsignacionTareasModel
    {
        GestorDocumentalEnt basdat = new GestorDocumentalEnt();
        public int idImagenGuardada = 0;
        public static string error = "";

        public void cierraEtapa(int negId, int etapa)
        {
            AsignacionTareas tarea = basdat.AsignacionTareas.Where(c => c.NegId == negId && c.IdEtapa == etapa).First();
            tarea.HoraTerminacion = DateTime.Now;
            tarea.IdEstado = 30;

            basdat.SaveChanges();
        }

        public AsignacionTareas getTareasLoteo(int etapa, Usuarios usuario)
        {
            try
            {
                AsignacionesController bAsig = new AsignacionesController();
                AsignacionTareas tareas = null;
                P_Etapas etapas = new P_Etapas
                {
                    IdEtapa = etapa
                };

                Captura n = new Captura();
                if (bAsig.ObtenerNegociosXEntrada(usuario, etapas) == 0M)
                {

                    error = "<br>No existen negocios disponibles para esta etapa";
                }
                else
                {
                    n.NegId = bAsig.ObtenerNegociosXEntrada(usuario, etapas);
                    AsignacionTareas a = new AsignacionTareas
                    {
                        NegId = n.NegId,
                        IdEtapa = etapas.IdEtapa,
                        Usuario = usuario.IdUsuario,
                        HoraInicio = DateTime.Now,
                        IdEstado = 10
                    };
                    //ASIGNO A ESA TAREA EL NEGOCIO
                    tareas = a;
                    if (!bAsig.ExisteEtapa(a))
                    {
                        bAsig.insertarAsignacion(a);

                        Parametros param = new GestorDocumentalEnt().Parametros.First(c => c.codigo == "PATH_TOTAL");
                        LoteoImagen img = new LoteoImagen();
                        img.NegId = n.NegId;
                        img.rutaImagen = param.valor + n.NegId + @"\" + n.NegId + ".tif";
                        img.imagenLoteada = n.NegId + ".tif";
                        img.procesado = true;

                        idImagenGuardada = new LoteoImagenModel().insertImagen(img);
                    }
                    else
                    {
                        idImagenGuardada = (int)basdat.LoteoImagen.Where(c => c.NegId == n.NegId).First().id;
                    }
                }

                //AsignacionTareas tareas = basdat.AsignacionTareas.Where(c => c.IdEtapa == 60 && (c.IdEstado == 10 || c.IdEstado == 20)).First();

                return tareas;
            }
            catch (System.Exception e)
            {
                // error = error + "<br>" + e.Message;
                return null;
                //throw;
            }

        }

        public AsignacionTareas getTareas(int etapa, Usuarios usuario)
        {
            try
            {
                AsignacionesController bAsig = new AsignacionesController();
                AsignacionTareas tareas = null;
                P_Etapas etapas = new P_Etapas
                {
                    IdEtapa = etapa
                };

                Captura n = new Captura();
                if (bAsig.ObtenerNegociosXEntrada(usuario, etapas) == 0M)
                {

                    error = "<br>No existen negocios disponibles para esta etapa";
                }
                else
                {
                    n.NegId = bAsig.ObtenerNegociosXEntrada(usuario, etapas);
                    AsignacionTareas a = new AsignacionTareas
                    {
                        NegId = n.NegId,
                        IdEtapa = etapas.IdEtapa,
                        Usuario = usuario.IdUsuario,
                        HoraInicio = DateTime.Now,
                        IdEstado = 10
                    };
                    //ASIGNO A ESA TAREA EL NEGOCIO
                    tareas = a;
                    if (!bAsig.ExisteEtapa(a))
                    {
                        //bAsig.insertarAsignacion(a);

                        //Parametros param = new GestorDocumentalEnt().Parametros.First(c => c.codigo == "PATH_TOTAL");
                        //LoteoImagen img = new LoteoImagen();
                        //img.NegId = n.NegId;
                        //img.rutaImagen = param.valor + n.NegId + @"\" + n.NegId + ".tif";
                        //img.imagenLoteada = n.NegId + ".tif";
                        //img.procesado = true;

                        //idImagenGuardada = new LoteoImagenModel().insertImagen(img);
                    }
                    else
                    {
                        //idImagenGuardada = (int)basdat.LoteoImagen.Where(c => c.NegId == n.NegId).First().id;
                    }
                }

                //AsignacionTareas tareas = basdat.AsignacionTareas.Where(c => c.IdEtapa == 60 && (c.IdEstado == 10 || c.IdEstado == 20)).First();

                return tareas;
            }
            catch (System.Exception e)
            {
                // error = error + "<br>" + e.Message;
                return null;
                //throw;
            }

        }

        /// <summary>
        /// Altera el estado de la tabla con respacto al estado y la etapa
        /// </summary>
        /// <param name="etapa">Etapa</param>
        /// <param name="estadoActual">estado actiual del negocio</param>
        /// <param name="NegId">Negocio id</param>
        /// <param name="estadoNuevo">Estado  a pasar</param>
        public void UpdateEstapaYEstado(int etapa, int estadoActual, int NegId, int estadoNuevo)
        {
            AsignacionTareas nuevo = basdat.AsignacionTareas.First<AsignacionTareas>(x => x.NegId == NegId && x.IdEtapa == etapa && x.IdEstado == estadoActual);
            nuevo.IdEstado = estadoNuevo;
            nuevo.HoraTerminacion = DateTime.Now;
            nuevo.Usuario = 0;
            basdat.SaveChanges();
        }

        public AsignacionTareas getTareas(int etapa, Usuarios usuario, int estado, int negId)
        {
            return basdat.AsignacionTareas.First<AsignacionTareas>(x => x.IdEtapa == etapa && x.IdEstado == estado && x.Usuario == usuario.IdUsuario && x.NegId == negId);
        }

        /// <summary>
        /// Crea una nueve asignacion de tareas
        /// </summary>
        /// <param name="nueva"></param>
        public void Add(AsignacionTareas nueva)
        {
            basdat.AddToAsignacionTareas(nueva);
            basdat.SaveChanges();
        }
    }
}