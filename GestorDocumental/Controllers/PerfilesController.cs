using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class PerfilesController : Controller
    {
        private GestorDocumentalEnt gd;

        public void InsetarRol(P_Roles R)
        {
            try
            {
                using (this.gd = new GestorDocumentalEnt())
                {
                    if (this.gd.P_Roles.Any<P_Roles>(O => O.RolId == R.RolId))
                    {
                        this.gd.P_Roles.First<P_Roles>(i => (i.RolId == R.RolId)).DescRol = R.DescRol;
                        this.gd.SaveChanges();
                    }
                    else
                    {
                        this.gd.AddToP_Roles(R);
                        this.gd.SaveChanges();
                    }
                }
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en PerfilesController metodo InsetarRol " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
        }

        public List<P_Roles> obtenerRoles()
        {
            List<P_Roles> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                list2 = (from r in this.gd.P_Roles select r).ToList<P_Roles>();
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en PerfilesController metodo obtenerRoles " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }
    }
}
