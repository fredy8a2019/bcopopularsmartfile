using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestorDocumental.Models;

namespace GestorDocumental.Controllers
{
    public class ControlUsuarios
    {
        GestorDocumentalEnt dbo = new GestorDocumentalEnt();

        //Obtiene la parametrizacion de la seguridad para los usuarios, cuando se encuentra habilitada
        //los usuarios tienen un limite de ingreso erroneo de la contraseña si es sobrepasado
        //el usuario queda bloqueado, al estas desabilitado los usuarios no pueden ser bloqueados
        public bool ConsultaSeguridadHabilitada()
        {
            var query = (from a in dbo.Parametros
                         where a.codigo == "CONTROL_USUARIOS"
                         select a).FirstOrDefault();
            if (query.valor.Equals("1"))
            { return true; }
            else
            { return false; }
        }

        //Obtiene la cantidad de meses de habilitada tiene la contraseña antes de que esta pida cambio
        public int ConsultarMesesCaducidad()
        {
            var query = (from a in dbo.Parametros
                         where a.codigo == "FECHA_CADUCIDAD"
                         select a).FirstOrDefault();
            return Convert.ToInt32(query.valor);
        }
        
        //Valida el usuario que esta intentando ingresar al aplicativo
        public bool validarUsuario(string usuario, string contrasena)
        {
            int _usuario = int.Parse(usuario);
            string respuesta = dbo.spValidarUsuario(_usuario, contrasena).First();

            if (respuesta == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Contador de errores que va aumentado cada vez que el usuario ingresa mal una contraseña
        public int contadorErrores(bool resultadoLogin)
        {
            if (resultadoLogin == false)
            {
                return 1;
            }
            return 0;
        }

        //Bloquea el usuario en el aplicativo para que no pueda acceder
        public void bloquearUsuario(string usuario, int intentos)
        {
            try
            {
                UsuariosBloqueados data = new UsuariosBloqueados()
                {
                    IdUsuario = Convert.ToDecimal(usuario),
                    FechaBloqueo = DateTime.Now,
                    Bloqueado = true
                };

                dbo.AddToUsuariosBloqueados(data);
                dbo.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el método de bloquarUsuario: " + ex.Message);
            }
        }

        //Desbloquea los usuarios permitiendoles de nuevo el acceso al sistema
        public void desbloquearUsuario(string usuario)
        {
            try
            {
                decimal _usuario = Convert.ToDecimal(usuario);
                List<UsuariosBloqueados> lista = (from a in dbo.UsuariosBloqueados
                                                  where a.IdUsuario == _usuario
                                                  select a).ToList();

                foreach (UsuariosBloqueados data in lista)
                {
                    dbo.UsuariosBloqueados.DeleteObject(data);
                    dbo.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el método desbloquearUsuario: " + ex.Message);
            }
        }

        //Obtiene la lista de usuarios que se encuentran bloqueados en el sistema
        public bool consultarUsuarioBloqueado(string usuario)
        {
            decimal _usuario = Convert.ToDecimal(usuario);
            List<UsuariosBloqueados> lista = (from a in dbo.UsuariosBloqueados
                                              where a.IdUsuario == _usuario && a.Bloqueado == true
                                              select a).ToList();
            if (lista.Count > 0)
            {
                return true;
            }
            return false;
        }

        //Consulta si el usuario existe en la base de datos
        public bool consultarUsuarioExistente(string usuario)
        {
            decimal _usuario = Convert.ToDecimal(usuario);
            List<Usuarios> lista = (from a in dbo.Usuarios
                                    where a.IdUsuario == _usuario
                                    select a).ToList();
            if (lista.Count > 0)
            {
                return true;
            }
            return false;
        }

        //Consulta la cantidad de contraseñas historicas que el usuario ha utilizado en el sistema
        public bool ConsultarHistoricos(string idUsuario, string passActual)
        {
            decimal _idusuario = Convert.ToDecimal(idUsuario);
            List<UsuariosHistorico> lista = (from a in dbo.UsuariosHistorico
                                             where a.idUsuario == _idusuario && a.PassCodeUsuario == passActual
                                             select a).ToList();
            if (lista.Count > 0)
            { return true; }
            else
            { return false; }
        }

        //Consulta la cantidad total de historicos que el usuario posee en el sistema
        public bool ConsultarTotalHistoricos(string idUsuario)
        {
            decimal _idUsuario = Convert.ToDecimal(idUsuario);
            List<UsuariosHistorico> lista = (from a in dbo.UsuariosHistorico
                                             where a.idUsuario == _idUsuario
                                             select a).ToList();
            if (lista.Count >= 4)
            { return true; }
            else
            { return false; }
        }

        //Borra los historicos mas antiguos de los usuarios para que siempre haya un limite de creacion
        //para cada uno de ellos
        public void actualizarHistoricos(string idUsuario, string passActual)
        {
            decimal _idUsuario = Convert.ToDecimal(idUsuario);
            var query = (from a in dbo.UsuariosHistorico
                         where a.idUsuario == _idUsuario
                         select a).OrderBy(x => x.FechaCaducidad).FirstOrDefault();

            List<UsuariosHistorico> lista = (from b in dbo.UsuariosHistorico
                                             where b.idHistorico == query.idHistorico
                                             select b).ToList();

            //Borrar registro nuevo
            if (lista.Count > 0)
            {
                foreach (UsuariosHistorico data in lista)
                {
                    dbo.UsuariosHistorico.DeleteObject(data);
                    dbo.SaveChanges();
                }
            }

            int mesesCaducidad = ConsultarMesesCaducidad();

            //Crear uno nuevo reemplazando el antiguo
            UsuariosHistorico dataHistorico = new UsuariosHistorico()
            {
                idUsuario = Convert.ToDecimal(idUsuario),
                PassCodeUsuario = passActual,
                FechaCaducidad = DateTime.Now.AddMonths(mesesCaducidad),
                Activo = true
            };

            dbo.AddToUsuariosHistorico(dataHistorico);
            dbo.SaveChanges();
        }

        //Consulta si la fecha de caducidad de la contraseña para el usuario ingresado
        //esta cerca, esta lejos o ya se ha caducado
        public string ConsultarCaducidad(string idUsuario, string passActual)
        {
            decimal _idUsuario = Convert.ToDecimal(idUsuario);
            var query = (from a in dbo.Usuarios
                         where a.IdUsuario == _idUsuario && a.PassCodeUsuario == passActual
                         select a).FirstOrDefault();

            if (query.FechaCaducidad == null)
            {
                return "NO";
            }

            DateTime fechaCaducidad = Convert.ToDateTime(query.FechaCaducidad);
            int dias = ((TimeSpan)(fechaCaducidad - DateTime.Now)).Days;

            if (dias == 0)
            {
                return "Caducada";
            }
            else if (dias <= 3)
            {
                return "Su contraseña Caducará en: " + dias + " dias por favor cambiarla";
            }
            else
            {
                return "NO";
            }
        }
    }
}