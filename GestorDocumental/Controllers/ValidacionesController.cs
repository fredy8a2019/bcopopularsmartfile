using System.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class ValidacionesController : Controller
    {
        public bool ValidarSeleccion(int index)
        {
            bool flag = false;
            return ((index > 0) || flag);
        }
    }
}
