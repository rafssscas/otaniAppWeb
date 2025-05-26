using dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class UserNegocio
    {
        //Metodo para listar todos los Users.
        public List<User> listar()
        {
            List<User> lista = new List<User>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Consulta SQL con JOIN para obtener información de las tablas relacionadas.
                //datos.setearConsulta("SELECT Id, email, pass, nombre, apellido, urlImagenPerfil, admin FROM USERS");
                datos.setearConsulta("SELECT Id, email, pass, nombre, apellido, urlImagenPerfil, admin FROM USERS");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    User aux = new User();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Email = datos.Lector["email"] as string;
                    aux.Pass = datos.Lector["pass"] as string;
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = datos.Lector["apellido"] as string;
                    
                    // Verificacion si la columna ImagenUrl es DBNull.
                    if (!(datos.Lector["UrlImagenPerfil"] is DBNull))
                        aux.UrlImagenPerfil = (string)datos.Lector["urlImagenPerfil"];
                    aux.Admin = (bool)datos.Lector["admin"];
                    lista.Add(aux);
                }
                datos.cerrarConexion();
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar usuarios", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        //-------------------------------------------------------
        // Metodo para agregar un nuevo User.
        public void agregar(User nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO USERS (email, pass, nombre, apellido, urlImagenPerfil, admin) VALUES (@email, @pass, @nombre, @apellido, @img, @admin)");
                datos.setearParametro("@email", nuevo.Email);
                datos.setearParametro("@pass", nuevo.Pass);
                datos.setearParametro("@nombre", nuevo.Nombre ?? (object)DBNull.Value);
                datos.setearParametro("@apellido", nuevo.Apellido ?? (object)DBNull.Value);
                datos.setearParametro("@img", nuevo.UrlImagenPerfil ?? (object)DBNull.Value);
                datos.setearParametro("@admin", nuevo.Admin);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar usuario", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        //-------------------------------------------------------
        // Metodo para modificar un User existente.
        public void modificar(User nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE USERS SET email = @email, pass = @pass, nombre = @nombre, apellido = @apellido, urlImagenPerfil = @img, admin = @admin WHERE Id = @id");
                datos.setearParametro("@email", nuevo.Email);
                datos.setearParametro("@pass", nuevo.Pass);
                datos.setearParametro("@nombre", nuevo.Nombre ?? (object)DBNull.Value);
                datos.setearParametro("@apellido", nuevo.Apellido ?? (object)DBNull.Value);
                datos.setearParametro("@img", nuevo.UrlImagenPerfil ?? (object)DBNull.Value);
                datos.setearParametro("@admin", nuevo.Admin);
                datos.setearParametro("@id", nuevo.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar usuario", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        //-------------------------------------------------------
        // Metodo para elminar un User por su ID.
        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                
                datos.setearConsulta("delete from users where Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar usuario", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        //-------------------------------------------------------
        // Método para filtrar los artículos según el criterio y filtro proporcionados.
        public List<User> filtrar(string email = null, string nombre = null, bool? admin = null)
        {
            List<User> lista = new List<User>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT Id, email, pass, nombre, apellido, urlImagenPerfil, admin FROM USERS WHERE 1=1";
                if (!string.IsNullOrEmpty(email))
                    consulta += " AND email LIKE @email";
                if (!string.IsNullOrEmpty(nombre))
                    consulta += " AND nombre LIKE @nombre";
                if (admin.HasValue)
                    consulta += " AND admin = @admin";

                datos.setearConsulta(consulta);

                if (!string.IsNullOrEmpty(email))
                    datos.setearParametro("@email", $"%{email}%");
                if (!string.IsNullOrEmpty(nombre))
                    datos.setearParametro("@nombre", $"%{nombre}%");
                if (admin.HasValue)
                    datos.setearParametro("@admin", admin.Value);

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    User user = new User();
                    user.Id = (int)datos.Lector["Id"];
                    user.Email = datos.Lector["email"] as string;
                    user.Pass = datos.Lector["pass"] as string;
                    user.Nombre = datos.Lector["nombre"] as string;
                    user.Apellido = datos.Lector["apellido"] as string;
                    user.UrlImagenPerfil = datos.Lector["urlImagenPerfil"] as string;
                    user.Admin = (bool)datos.Lector["admin"];
                    lista.Add(user);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al filtrar usuarios", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool Login(User user)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT Id, email, pass, nombre, apellido, urlImagenPerfil, admin FROM USERS WHERE email = @email AND pass = @pass");
                datos.setearParametro("@email", user.Email);
                datos.setearParametro("@pass", user.Pass);
                datos.ejecutarLectura();
                if (datos.Lector.Read())
                {
                    user.Id = (int)datos.Lector["Id"];
                    user.Nombre = datos.Lector["nombre"] as string;
                    user.Apellido = datos.Lector["apellido"] as string;
                    user.UrlImagenPerfil = datos.Lector["urlImagenPerfil"] as string;
                    user.Admin = (bool)datos.Lector["admin"];
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al hacer login", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
