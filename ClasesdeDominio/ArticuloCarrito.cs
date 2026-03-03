using ClasesDeDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesdeDominio
{
    public class ArticuloCarrito : Articulo
    {
        public int cant { get; set; }


        public ArticuloCarrito(Articulo articulo)
        {
            this.id = articulo.id;
            this.nombre = articulo.nombre;
            this.categoria = articulo.categoria;
            this.codigo = articulo.codigo;
            this.listImagenes = articulo.listImagenes;
            this.precio = articulo.precio;
            this.descripcion = articulo.descripcion;
            this.marca = articulo.marca;
            this.cant = 1;
        }

    }
}
