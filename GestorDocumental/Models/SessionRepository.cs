using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections;
using GestorDocumental.Controllers;

namespace GestorDocumental.Models
{
    public static class SessionRepository
    {
        private static GestorDocumentalEnt gd = new GestorDocumentalEnt();

        public static List<MenuPadre_Result> AllPadres(Usuarios U)
        {
            List<MenuPadre_Result> result =
                (List<MenuPadre_Result>)HttpContext.Current.Session["PADRES_MENU_MVC"];
            if (result == null)
            {
                HttpContext.Current.Session["PADRES_MENU_MVC"] = result = gd.MenuPadre(U.IdUsuario).ToList<MenuPadre_Result>();
            }

            return result;
        }

        public static IEnumerable dataRepEstado(DateTime FechaInicio, DateTime FechaFin)
        {   
            IEnumerable result =
               (IEnumerable)HttpContext.Current.Session["DATA_REPORTE"];
            if (result == null)
            {
                HttpContext.Current.Session["DATA_REPORTE"] = result = ReportController.GetSalesPerson(FechaInicio, FechaFin);
            }
            
            return result;
        }

        public static void setAtributo(string nombre, object obj)
        {
            HttpContext.Current.Session[nombre] = obj;
        }

        public static object getAtributo(string nombre)
        {
            object obj = HttpContext.Current.Session[nombre];
            return obj;
        }
    }
}