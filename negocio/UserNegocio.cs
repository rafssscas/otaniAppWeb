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
                datos.setearConsulta("select Codigo, Nombre, A.Descripcion , M.Descripcion Marca, C.Descripcion Categoria, ImagenUrl, Precio, M.Id IdMarca, C.Id IdCategoria, A.Id from UserS A, CATEGORIAS C, MARCAS M where C.Id = A.IdCategoria and M.Id = A.IdMarca");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    User aux = new User();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    // Verificacion si la columna ImagenUrl es DBNull.
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    // Uso de GetSqlMoney para obtener un SqlMoney y luego convertirlo a int.
                    SqlMoney sqlPrecio = datos.Lector.GetSqlMoney(6);
                    aux.Precio = (int)sqlPrecio;
                    // Creación de objetos Marca y Categoria.
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    lista.Add(aux);
                }
                datos.cerrarConexion();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
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
                datos.setearConsulta("INSERT INTO UserS(Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) values ('" + nuevo.Codigo + "', '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', @idMarca, @idCategoria , @imagenUrl," + nuevo.Precio + ")");
                datos.setearParametro("@idMarca", nuevo.Marca.Id);
                datos.setearParametro("@idCategoria", nuevo.Categoria.Id);
                datos.setearParametro("@imagenUrl", nuevo.ImagenUrl);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        //-------------------------------------------------------
        // Metodo para modificar un User existente.
        public void modificar(User arti)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update UserS set Codigo = @codigo, Nombre = @nombre, Descripcion = @desc, IdMarca = @idMarca, IdCategoria = @idCategoria, ImagenUrl = @img, Precio = @precio where Id = @id ");
                datos.setearParametro("@codigo", arti.Codigo);
                datos.setearParametro("@nombre", arti.Nombre);
                datos.setearParametro("@desc", arti.Descripcion);
                datos.setearParametro("@idMarca", arti.Marca.Id);
                datos.setearParametro("@idCategoria", arti.Categoria.Id);
                datos.setearParametro("@img", arti.ImagenUrl);
                datos.setearParametro("@precio", arti.Precio);
                datos.setearParametro("@id", arti.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
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
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from UserS where Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //-------------------------------------------------------
        // Método para filtrar los artículos según el criterio y filtro proporcionados.
        public List<User> filtrar(string campo, string criterio, string filtro)
        {
            List<User> lista = new List<User>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "select Codigo, Nombre, A.Descripcion , M.Descripcion Marca, C.Descripcion Categoria, ImagenUrl, Precio, M.Id IdMarca, C.Id IdCategoria, A.Id from UserS A, CATEGORIAS C, MARCAS M where C.Id = A.IdCategoria and M.Id = A.IdMarca and ";
                // Agregar condiciones según el campo y criterio.               
                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Precio < " + filtro;
                            break;
                        default:
                            consulta += "Precio = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "A.Descripcion like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "A.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    User aux = new User();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    SqlMoney sqlPrecio = datos.Lector.GetSqlMoney(6);
                    aux.Precio = (int)sqlPrecio;
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




    }
}
