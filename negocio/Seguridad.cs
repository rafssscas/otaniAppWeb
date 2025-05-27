using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace negocio
{
    public static class Seguridad
    {
        /// <summary>
        /// Verifica si hay un usuario en sesión y tiene ID válido.
        /// </summary>
        public static bool SesionActiva(object user)
        {
            User usuario = user as User;
            return usuario != null && usuario.Id != 0;
        }

        /// <summary>
        /// Verifica si el usuario de sesión es administrador.
        /// </summary>
        public static bool EsAdmin(object user)
        {
            User usuario = user as User;
            return usuario != null && usuario.Admin;
        }

        /// <summary>
        /// Verifica si el usuario de sesión es cliente (NO admin).
        /// </summary>
        public static bool EsCliente(object user)
        {
            User usuario = user as User;
            return usuario != null && !usuario.Admin;
        }

        /// <summary>
        /// Protege una página que solo debe ver usuarios logueados (cualquier rol).
        /// Si no está logueado, redirige a Login.
        /// </summary>
        public static void ProtegerPaginaUsuario(HttpSessionState session, HttpResponse response)
        {
            if (!SesionActiva(session["user"]))
            {
                response.Redirect("Login.aspx", true);
            }
        }

        /// <summary>
        /// Protege una página que solo debe ver el admin.
        /// Si no es admin, redirige a Home.
        /// </summary>
        public static void ProtegerPaginaAdmin(HttpSessionState session, HttpResponse response)
        {
            if (!SesionActiva(session["user"]) || !EsAdmin(session["user"]))
            {
                response.Redirect("Default.aspx", true);
            }
        }

        /// <summary>
        /// Protege una página solo para cliente (no admin).
        /// </summary>
        public static void ProtegerPaginaCliente(HttpSessionState session, HttpResponse response)
        {
            if (!SesionActiva(session["user"]) || !EsCliente(session["user"]))
            {
                response.Redirect("Default.aspx", true);
            }
        }
    }
}
