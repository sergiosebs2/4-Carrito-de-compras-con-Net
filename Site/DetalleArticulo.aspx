<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleArticulo.aspx.cs" Inherits="Site.DetalleArticulo1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Detalle del Artículo</title>

    <link href="Content/Styles.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body { background-color: #f8f9fa; }

        .navbar .cantProd {
            color: white;
            background-color: red;
            padding: 0.2rem 0.7rem;
            border-radius: 50%;
            font-weight: bold;
        }

        .detalle-container {
    display: flex;
    flex-wrap: wrap;
    gap: 2rem;
    justify-content: center; /* centra horizontalmente */
    align-items: flex-start;
    margin-top: 20px;
}


       .detalle-imagen {
    flex: 1 1 300px;
    max-width: 500px;
    height: 442px; 
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: #fff; 
    border: 1px solid #ddd; 
}

.detalle-imagen img {
    max-width: 100%;
    max-height: 100%;
    object-fit: contain;
}


        .detalle-info {
            flex: 1 1 300px;
        }

        .detalle-info p { font-size: 1.1rem; }
        .detalle-info strong { color: #333; }

        footer {
            background-color: #212529;
            color: #ccc;
            padding: 20px 0;
            margin-top: 50px;
        }
        footer a { color: #aaa; text-decoration: none; }
        footer a:hover { color: white; }
    </style>
</head>

<body>
<form id="form1" runat="server">

 <!-- NAVBAR -->
<nav class="navbar navbar-dark bg-dark">
    <div class="container-fluid d-flex justify-content-between align-items-center">
        <a class="navbar-brand" href="/Default.aspx"> Minecart</a>
        <div class="d-flex align-items-center">
            <span class="badge bg-danger me-2 badge-cant"><%= cantidadProduc %></span>

            <!-- BOTON CARRITO -->
            <button type="button"
                    class="btn btn-primary d-flex align-items-center"
                    data-bs-toggle="offcanvas"
                    data-bs-target="#offcanvasCarrito"
                    aria-controls="offcanvasCarrito">
                <img src="/Content/basket.svg" width="20" class="me-2" /> Carrito
            </button>
        </div>
    </div>
</nav>

    <!-- OFFCANVAS DEL CARRITO -->
    <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasCarrito">
        <div class="offcanvas-header">
            <h5>Carrito</h5>
            <button type="button" class="btn-close" data-bs-dismiss="offcanvas"></button>
        </div>
        <div class="offcanvas-body">
            <% if (ListCarrito != null && ListCarrito.Count > 0) {
                foreach (ClasesdeDominio.ArticuloCarrito a in ListCarrito) {
                    string img = "/Content/noimage.jpg";
                    if (a.listImagenes != null && a.listImagenes.Count > 0)
                        img = a.listImagenes[0].urlImagen;
            %>
            <div class="card mb-2">
                <div class="row g-0">
                    <div class="col-4">
                        <img src="<%= img %>" class="img-fluid" />
                    </div>
                    <div class="col-8">
                        <div class="card-body">
                            <h6 class="card-title"><%= a.nombre %></h6>
                            <p class="card-text">Cantidad: <%= a.cant %></p>
                            <p class="card-text">Precio: $<%= a.precio %></p>
                            <a href="DetalleArticulo.aspx?id=<%= a.id %>&action=0" class="btn btn-danger btn-sm mt-1">Eliminar</a>
                        </div>
                    </div>
                </div>
            </div>
            <% } } else { %>
                <p>Carrito vacío</p>
            <% } %>

            <h5 class="mt-3">Total: $<%= TotalCarrito %></h5>
        </div>
    </div>

    <!-- DETALLE DEL ARTICULO -->
    <div class="container">
        <% 
            ClasesDeDominio.Articulo articulo = null;
            int idArticulo = 0;
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out idArticulo)) {
                articulo = CargarDetalleArticulo(idArticulo);
            }
        %>

        <div class="detalle-container">

            <!-- IMAGEN -->
            <div class="detalle-imagen">
                <% if (articulo != null && articulo.listImagenes != null && articulo.listImagenes.Count > 0) { %>
                    <img src="<%= articulo.listImagenes[0].urlImagen %>" />
                <% } else { %>
                    <img src="/Content/noimage.jpg" />
                <% } %>
            </div>

            <!-- INFORMACIÓN -->
            <div class="detalle-info">
                <h1><asp:Label ID="lblNombre" runat="server" /></h1>
                <p><strong>Marca:</strong> <asp:Label ID="lblMarca" runat="server" /></p>
                <p><strong>Categoría:</strong> <asp:Label ID="lblCategoria" runat="server" /></p>
                <p><strong>Descripción:</strong> <asp:Label ID="lblDescripcion" runat="server" /></p>
                <p><strong>Precio:</strong> $<asp:Label ID="lblPrecio" runat="server" /></p>

                <a href="DetalleArticulo.aspx?id=<%= idArticulo %>&action=1" class="btn btn-primary btn-lg me-2 mt-2">Agregar al carrito</a>
                
                <a href="Default.aspx" class="btn btn-secondary btn-lg mt-2">Volver</a>
            </div>

        </div>
    </div>

    <!-- FOOTER -->
    <footer class="mt-5">
        <div class="container text-center">
            <p class="mb-1">© 2024 Minecart</p>
            <small>TP ASP.NET – Equipo 23</small>
        </div>
    </footer>

</form>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
