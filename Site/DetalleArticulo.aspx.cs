using ClasesDeDominio;
using ClasesdeDominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site
{
    public partial class DetalleArticulo1 : System.Web.UI.Page
    {
        public float TotalCarrito { get; set; }
        public int idArticulo { get; set; }
        public List<ArticuloCarrito> ListCarrito;
        public List<Articulo> ListArticulos { get; set; }
        public int cantidadProduc = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            NegocioArticulo neg = new NegocioArticulo();
            ListArticulos = neg.listar();

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
                    Articulo artSeleccionado = ListArticulos.Find(x => x.id == id);
                    if (artSeleccionado != null)
                    {
                        if (seleccionado != null)
                            seleccionado.cant++;
                        else
                            ListCarrito.Add(new ArticuloCarrito(artSeleccionado));
                    }
                }
                else if (action == 0)
                {
                    if (seleccionado != null)
                        ListCarrito.Remove(seleccionado);
                }
                else if (action == 3)
                {
                    if (seleccionado != null)
                    {
                        seleccionado.cant--;
                        if (seleccionado.cant <= 0)
                            ListCarrito.Remove(seleccionado);
                    }
                }
                else if (action == 4)
                {
                    if (seleccionado != null)
                        seleccionado.cant++;
                }

                Session["Carrito"] = ListCarrito;
                cantidadProduc = ListCarrito.Count;
                Response.Redirect("DetalleArticulo.aspx?id=" + id, false);
            }

            TotalCarrito = ListCarrito.Sum(art => art.precio * art.cant);

            if (Request.QueryString["id"] != null)
            {
                idArticulo = int.Parse(Request.QueryString["id"]);
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int articuloId;
                    if (int.TryParse(Request.QueryString["id"], out articuloId))
                    {
                        CargarDetalleArticulo(articuloId);
                    }
                }
            }
        }

        public Articulo CargarDetalleArticulo(int articuloId)
        {
            NegocioArticulo negocio = new NegocioArticulo();
            Articulo articulo = negocio.obtenerPorId(articuloId);

            if (articulo != null)
            {
                if (articulo.listImagenes == null || articulo.listImagenes.Count == 0)
                    articulo.listImagenes = new List<Imagenes> { new Imagenes(0, "/Content/noimage.jpg") };

                lblNombre.Text = articulo.nombre;
                lblMarca.Text = articulo.marca.descripcion;
                lblCategoria.Text = articulo.categoria.descripcion;
                lblDescripcion.Text = articulo.descripcion;
                lblPrecio.Text = articulo.precio.ToString("F3");
            }

            return articulo;
        }
    }
}
