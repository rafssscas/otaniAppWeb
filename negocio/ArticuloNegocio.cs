using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;
using System.Data.SqlTypes;

namespace negocio
{
    public class ArticuloNegocio
    {
        //Metodo para listar todos los articulos.
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();      
            try
            {
                // Consulta SQL con JOIN para obtener información de las tablas relacionadas.
                datos.setearConsulta(@"SELECT 
                    A.Id, A.Codigo, A.Nombre, A.Descripcion, 
                    A.IdMarca, M.Descripcion Marca,
                    A.IdCategoria, C.Descripcion Categoria,
                    A.ImagenUrl, A.Precio
                    FROM ARTICULOS A
                    LEFT JOIN MARCAS M ON M.Id = A.IdMarca
                    LEFT JOIN CATEGORIAS C ON C.Id = A.IdCategoria
        ");
                datos.ejecutarLectura();                  

                while(datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    // Creación de objetos Marca y Categoria.
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    // Verificacion si la columna ImagenUrl es DBNull.
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    // Uso de GetSqlMoney para obtener un SqlMoney y luego convertirlo a int.
                    SqlMoney sqlPrecio = datos.Lector.GetSqlMoney(6);
                    aux.Precio = (int)sqlPrecio;
                    lista.Add(aux);
                }
                datos.cerrarConexion();
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar artículos", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }          
        }
        //-------------------------------------------------------
        // Metodo para agregar un nuevo articulo.
        public void agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"INSERT INTO ARTICULOS 
                    (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)
                    VALUES ('" + nuevo.Codigo + "','" + nuevo.Nombre+ "','"+nuevo.Descripcion+"', @idMarca, @idCategoria, @imgUrl, '"+nuevo.Precio+"')");
                datos.setearParametro("@idMarca", nuevo.Marca.Id);
                datos.setearParametro("@idCategoria", nuevo.Categoria.Id);
                datos.setearParametro("@imagenUrl", nuevo.ImagenUrl);
                //datos.setearParametro("@precio", nuevo.Precio);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar artículo", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        //-------------------------------------------------------
        // Metodo para modificar un articulo existente.
        public void modificar(Articulo arti)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"UPDATE ARTICULOS SET Codigo = @codigo,Nombre = @nombre, Descripcion = @desc, IdMarca = @idMarca, IdCategoria = @idCategoria, ImagenUrl = @imgUrl, Precio = @precio WHERE Id = @id");
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
                throw new Exception("Error al modificar artículo", ex);
            }
            finally 
            { 
                datos.cerrarConexion();
            }
        }
        //-------------------------------------------------------
        // Metodo para elminar un articulo por su ID.
        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("delete from ARTICULOS where Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar artículo", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        //-------------------------------------------------------
        // Método para filtrar los artículos según el criterio y filtro proporcionados.
        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "select A.Codigo, A.Nombre, A.Descripcion, A.Marca, M.Descripcion Marca, A.Categoria, C.Descripcion Catretgoria, A.ImagenUrl, A.Precio FROM ARTICULOS A LEFT JOIN MARCAS M ON M.Id = A.IdMarca LEFT JOIN CATEOGRIAS C ON C.Id = A.IdCategoria Where 1=1;
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
                    Articulo aux = new Articulo();
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


