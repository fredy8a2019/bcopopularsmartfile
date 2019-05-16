using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class MenuController : Controller
    {
        private GestorDocumentalEnt gd = new GestorDocumentalEnt();

        public void borrarOpcionesMenu(P_Roles R, Menu M)
        {
            try
            {
                using (this.gd)
                {
                    P_Roles entity = this.gd.P_Roles.SingleOrDefault<P_Roles>(x => x.RolId == R.RolId);
                    this.gd.Menu.SingleOrDefault<Menu>(y => (y.IdMenu == M.IdMenu)).P_Roles.Remove(entity);
                    this.gd.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en MenuController metodo borrarOpcionesMenu " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
        }

        public void InsertarOpcionesMenu(P_Roles R, Menu M)
        {
            try
            {
                using (this.gd)
                {
                    P_Roles entity = this.gd.P_Roles.SingleOrDefault<P_Roles>(x => x.RolId == R.RolId);
                    Menu menu = this.gd.Menu.SingleOrDefault<Menu>(y => y.IdMenu == M.IdMenu);
                    menu.P_Roles.Add(entity);
                    foreach (P_Roles roles2 in menu.P_Roles)
                    {
                        this.gd.ObjectStateManager.ChangeObjectState(menu, EntityState.Unchanged);
                    }
                    this.gd.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en MenuController metodo InsertarOpcionesMenu " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
        }

        public List<Menu> ObtenerHijos(Menu MP)
        {
            List<Menu> list2;
            try
            {

                list2 = (from m in this.gd.Menu
                         where m.IdPadre == MP.IdMenu
                         select m).ToList<Menu>();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en MenuController metodo ObtenerHijos " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<MenuHijos_Result> ObtenerHijos(Usuarios U, Menu M)
        {
            List<MenuHijos_Result> list2;
            try
            {

                list2 = this.gd.MenuHijos(M.IdPadre, U.IdUsuario).ToList();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en MenuController metodo ObtenerHijos " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<spObtenerMenuHijosPerfil_Result> ObtenerHijosPerfil(int IdMenu, int RolId)
        {
            List<spObtenerMenuHijosPerfil_Result> list2;
            try
            {

                list2 = this.gd.spObtenerMenuHijosPerfil(IdMenu, RolId).ToList();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en MenuController metodo ObtenerHijosPerfil " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<spObtenerMenuHijosPerfil_Result> ObtenerHijosPerfil(P_Roles R, Menu M)
        {
            List<spObtenerMenuHijosPerfil_Result> list2;
            try
            {

                list2 = this.gd.spObtenerMenuHijosPerfil(M.IdMenu, R.RolId).ToList();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en MenuController metodo ObtenerHijosPerfil " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<Menu> ObtenerPadres()
        {
            List<Menu> list2;
            try
            {

                list2 = (from m in this.gd.Menu
                         where m.IdPadre == null
                         select m).ToList<Menu>();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en MenuController metodo ObtenerPadres " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<MenuPadre_Result> ObtenerPadres(Usuarios U)
        {
            List<MenuPadre_Result> list2;
            try
            {
                list2 = this.gd.MenuPadre(U.IdUsuario).ToList<MenuPadre_Result>();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en MenuController metodo ObtenerPadres(Usuarios U) " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        public List<MenuPadre_Result> ObtenerPadresMvc(Usuarios U)
        {
            List<MenuPadre_Result> list2;
            try
            {
                //list2 = this.gd.MenuPadre(U.IdUsuario).ToList<MenuPadre_Result>();
                list2 = SessionRepository.AllPadres(U);
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en MenuController metodo ObtenerPadresMvc " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

        [HttpGet]
        public JsonResult ObtenerPadresMvc1(Usuarios U)
        {
            List<MenuPadre_Result> list2;
            try
            {
                U = (GestorDocumental.Models.Usuarios)Session["USUARIO_LOGUEADO"];
                list2 = SessionRepository.AllPadres(U);
                return Json(list2, JsonRequestBehavior.AllowGet);
            }

            catch (Exception exception)
            {
                LogRepository.registro("Error en MenuController metodo ObtenerPadresMvc " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }

        }

    }
}
