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

        public void agregar(Marca nueva)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO MARCAS (Descripcion) VALUES (@desc)");
                datos.setearParametro("@desc", nueva.Descripcion);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar marca", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Marca marca)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE MARCAS SET Descripcion = @desc WHERE Id = @id");
                datos.setearParametro("@desc", marca.Descripcion);
                datos.setearParametro("@id", marca.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar marca", ex);
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
                datos.setearConsulta("DELETE FROM MARCAS WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar marca", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Marca> filtrar(string descripcion)
        {
            List<Marca> lista = new List<Marca>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Descripcion FROM MARCAS WHERE Descripcion LIKE @desc");
                datos.setearParametro("@desc", $"%{descripcion}%");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Marca marca = new Marca();
                    marca.Id = (int)datos.Lector["Id"];
                    marca.Descripcion = datos.Lector["Descripcion"] as string;
                    lista.Add(marca);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al filtrar marcas", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
