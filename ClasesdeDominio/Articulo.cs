using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesDeDominio
{
    public class Articulo
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public Marca marca { get; set; } = new Marca();    //CHAT GPT ME RECOMENDO ESTO
        public Categoria categoria { get; set; } = new Categoria();     //CHAT GPT ME RECOMENDO ESTO, INVOCAR AL CONSTRUCTOR

        public List<Imagenes> listImagenes { get; set; } //usamos para cargar mas de 1 imagen.-


        public Imagenes imagenArticulo { get; set; } // se agrega get y set para que se logre Modificar .-D
        public float precio { get; set; }

        public Articulo() {;}

    }
}
