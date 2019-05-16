using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace GestorDocumental.Models
{
    public class LogUsuarios
    {
        GestorDocumentalEnt db = new GestorDocumentalEnt();
        public void CierraSesion(decimal Usuario)
        {
            db.spCierraLoginUser(Usuario);
        }

        public bool ValidaSesion(decimal Usuario)
        {
            //string hostName = Dns.GetHostName();
            //Console.WriteLine(hostName);
            //string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            string myIP = GetUserIP();
            var i = db.spValidaLoginUser(Usuario, myIP).FirstOrDefault();
            if (i == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ActualizaSesion(decimal Usuario)
        {
            //string hostName = Dns.GetHostName();
            //Console.WriteLine(hostName);
            //string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            string myIP = GetUserIP();
            db.spActualizaLogin(Usuario, myIP);
        }

        private string GetUserIP()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }


    }
}