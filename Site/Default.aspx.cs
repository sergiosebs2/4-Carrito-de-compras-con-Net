using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClasesdeDominio;
using ClasesDeDominio;
using Negocio;

namespace Site
{
    public partial class Default : System.Web.UI.Page
    {
        public List<Articulo> ListArticulos { get; set; }
        public List<ArticuloCarrito> ListCarrito;
        public float TotalCarrito { get; set; }
        public int cantidadProduc = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            NegocioArticulo negocio = new NegocioArticulo();
            ListArticulos = negocio.listar();

            ListCarrito = Session["Carrito"] != null ? (List<ArticuloCarrito>)Session["Carrito"] : new List<ArticuloCarrito>();
            Session["Carrito"] = ListCarrito;
            cantidadProduc = ListCarrito.Count;

            if (Request.QueryString["id"] != null && Request.QueryString["action"] != null)
            {
                int id = int.Parse(Request.QueryString["id"]);
                int action = int.Parse(Request.QueryString["action"]);

                ArticuloCarrito seleccionado = ListCarrito.Find(x => x.id == id);

                if (action == 1)
                {
                    Articulo articulo = ListArticulos.Find(x => x.id == id);
                    if (articulo != null)
                    {
                        if (seleccionado != null)
                        {
                            seleccionado.cant++;
                        }
                        else
                        {
                            ListCarrito.Add(new ArticuloCarrito(articulo));
                        }
                    }
                }
                else if (seleccionado != null)
                {
                    if (action == 3) // eliminar completamente
                    {
                        ListCarrito.Remove(seleccionado);
                    }
                    else if (action == 4) // aumentar cantidad
                    {
                        seleccionado.cant++;
                    }
                    else if (action == 0) // quitar 1 unidad
                    {
                        seleccionado.cant--;
                        if (seleccionado.cant <= 0)
                        {
                            ListCarrito.Remove(seleccionado);
                        }
                    }
                }

                cantidadProduc = ListCarrito.Count;
                Session["Carrito"] = ListCarrito;
            }

            TotalCarrito = ListCarrito.Sum(articulo => articulo.precio * articulo.cant);

            if (!IsPostBack)
            {
                CargarCategorias();
                CargarMarcas();
            }
        }

        private void CargarCategorias()
        {
            NegocioCategoria negocio = new NegocioCategoria();
            List<Categoria> categorias = negocio.listar();
            foreach (Categoria categoria in categorias)
            {
                ListItem item = new ListItem(categoria.descripcion, categoria.id.ToString());
                CategoryFilter.Items.Add(item);
            }
        }

        private void CargarMarcas()
        {
            NegocioMarca negocio = new NegocioMarca();
            List<Marca> marcas = negocio.listar();
            foreach (Marca marca in marcas)
            {
                ListItem item = new ListItem(marca.descripcion, marca.id.ToString());
                BrandFilter.Items.Add(item);
            }
        }
    }
}
