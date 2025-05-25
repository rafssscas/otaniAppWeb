using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace negocio
{
    public class AccesoDatos
    {
        //Configuracion inicial de los objetos para establecer
        //una conexión con la DB y ejectuar comandos SQL.
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        public SqlDataReader Lector
        {
            get { return lector; }
        }
        //-------------------------------------------------------
        //En el constructor se define una cadena de conexión que 
        //especifica la ubicacion del servidor y la BD que se utilizarán.
        public AccesoDatos()
        {   
            //Definimos las instancias de SQL para la conexion.
            string connectionString = "server=localhost; database=CATALOGO_DB; integrated security=true";
            conexion = new SqlConnection(connectionString);
            comando = new SqlCommand();
        }
        //-------------------------------------------------------
        //Metodo para establecer una consulta personalizada para la BD.
        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text; 
            comando.CommandText = consulta;
        }
        //-------------------------------------------------------
        //Metodo que ejecuta la consulta en la BD y abre una conexión.
        //Inicia la lectura de datos utilizando el objeto SqlCommand y 
        //almacena el resultado en un obj 'SqlDataReader' lector.
        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }
        //-------------------------------------------------------
        // Método para ejecutar acciones que nos devuelven un
        // conjunto de resultados (INSERT, UPDATE, DELETE, etc.).
        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //-------------------------------------------------------
        // Método para establecer parámetros en el comando SQL.
        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }
        //-------------------------------------------------------
        //Metodo que se utiliza para cerra la conexión con la base de datos y
        //liberar recursos.
        public void cerrarConexion()
        {
            if(lector != null) 
                lector.Close();
            conexion.Close();
        }
    }
}
