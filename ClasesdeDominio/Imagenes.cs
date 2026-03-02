using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClasesDeDominio
{
    public class Imagenes
    {
        public int id { get; set; }
        public string urlImagen { get; set; }
        public int idArticulo { get; set; }

        public Imagenes(int idA, string urlImag) { 
            this.idArticulo = idA;
            this.urlImagen = urlImag;
        }
        public override string ToString()
        {
            return urlImagen;
        }

    }
}