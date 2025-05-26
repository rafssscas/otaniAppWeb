using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class CategoriaNegocio
    {
        //Metodo para obtener una lista de todas las categorias.
        public List<Categoria> listar()
        {
			List<Categoria> lista = new List<Categoria>();
            // Crear instancia de la clase AccesoDatos para interactuar con la base de datos.
            AccesoDatos datos = new AccesoDatos();
			try
			{
                // Establecer la consulta SQL para obtener las categorías.
                datos.setearConsulta("select Id, Descripcion from CATEGORIAS");
                // Ejecutar la lectura de datos.
                datos.ejecutarLectura();
                // Leer los resultados de la consulta y crear objetos Categoria.
                while (datos.Lector.Read())
                {
                    Categoria aux = new Categoria();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    // Agregar la categoría a la lista.
                    lista.Add(aux);
                }
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

        public void agregar(Categoria nueva)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO CATEGORIAS (Descripcion) VALUES (@desc)");
                datos.setearParametro("@desc", nueva.Descripcion);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar categoría", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Categoria categoria)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE CATEGORIAS SET Descripcion = @desc WHERE Id = @id");
                datos.setearParametro("@desc", categoria.Descripcion);
                datos.setearParametro("@id", categoria.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar categoría", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM CATEGORIAS WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar categoría", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Categoria> filtrar(string descripcion)
        {
            List<Categoria> lista = new List<Categoria>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Descripcion FROM CATEGORIAS WHERE Descripcion LIKE @desc");
                datos.setearParametro("@desc", $"%{descripcion}%");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Categoria categoria = new Categoria();
                    categoria.Id = (int)datos.Lector["Id"];
                    categoria.Descripcion = datos.Lector["Descripcion"] as string;
                    lista.Add(categoria);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al filtrar categorías", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
