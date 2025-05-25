using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class MarcaNegocio
    {
        //Metodo para obtener una lista de todas las marcas.
        public List<Marca> listar()
        {
            List<Marca> lista = new List<Marca>();
            // Crear instancia de la clase AccesoDatos para interactuar con la base de datos.
            AccesoDatos datos = new AccesoDatos();
                try
                {
                // Establecer la consulta SQL para obtener las marcas.
                datos.setearConsulta("select Id, Descripcion from MARCAS");
                // Ejecutar la lectura de datos.
                datos.ejecutarLectura();
                // Leer los resultados de la consulta y crear objetos Marca.
                while (datos.Lector.Read())
                {
                    Marca aux = new Marca();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    // Agregar la marca a la lista.
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
