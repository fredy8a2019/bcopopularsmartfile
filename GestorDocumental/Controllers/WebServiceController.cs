using System.Web.Mvc;

namespace GestorDocumental.Controllers
{
    public class WebServiceController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public void llamadoWebService(string rutaXML, string usuario, string negId)
        {
            TransmicionXMLController operacionesSAP = new TransmicionXMLController();
            string respuestaSAP = operacionesSAP.ExecuteWebService(rutaXML);
            string resultadoFinal = operacionesSAP.guardarResultadoSAP(respuestaSAP, usuario, negId);
        }
    }
}
