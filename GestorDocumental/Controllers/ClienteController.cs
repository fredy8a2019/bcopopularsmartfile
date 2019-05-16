using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class ClienteController : Controller
    {
        private GestorDocumentalEnt gd;

        public List<Clientes> obtenerClientes()
        {
            List<Clientes> list2;
            try
            {
                this.gd = new GestorDocumentalEnt();
                List<Clientes> list = (from p in this.gd.Clientes select p).ToList<Clientes>();
                Clientes item = new Clientes
                {
                    CliNit = 0M,
                    CliNombre = "Seleccione..."
                };
                list.Insert(0, item);
                list2 = list;
            }
            catch (Exception exception)
            {
                LogRepository.registro("Error en ClienteController metodo obtenerClientes " + exception.Message + " stack trace " + exception.StackTrace);
                throw exception;
            }
            return list2;
        }

    }
}
