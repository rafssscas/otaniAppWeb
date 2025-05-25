using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Articulo
    {
        /*
         * Los datos mínimos con los que deberá contar el artículo son los siguientes:
            Código de artículo.
            Nombre.
            Descripción.
            Marca (seleccionable de una lista desplegable).
            Categoría (seleccionable de una lista desplegable.
            Imagen.
            Precio.
            Agrego Id porque necesito este atributo para decidir si estoy agregando un 
            articulo nuevo o modificando un articulo existente en la BD.
         * 
         */
        public int Id { get; set; }
        [DisplayName("Código")]
        public string Codigo {  get; set; }
        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        public Marca Marca { get; set; }
        
        [DisplayName("Categoría")]
        public Categoria Categoria { get; set; }
        public string ImagenUrl { get; set; }
        public int Precio { get; set; }

        public override string ToString()
        {
            return Descripcion;
        }

    }
}
