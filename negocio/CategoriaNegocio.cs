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
    }
}
