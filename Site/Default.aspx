<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Site.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Minecart</title>

    <link href="Content/Styles.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet"/>

    <style>
        /* FOOTER */
        footer {
            background: #212529;
            color: #ccc;
            padding: 20px 0;
            margin-top: 40px;
        }
        footer a { color: #aaa; text-decoration: none; }
        footer a:hover { color: white; }

        /* SIDEBAR */
        .sidebar-label { font-weight:bold; margin-top:10px; display:block; }
        #sidebar {
            width: 300px;
            flex-shrink: 0;
            height: 100vh;
            position: sticky;
            top: 0;
            overflow-y: auto;
        }

        /* ARTICLE CARDS */
        .article-card .card {
            transition: transform 0.3s, box-shadow 0.3s;
        }
        .article-card .card:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 20px rgba(0,0,0,0.2);
        }

        /* IMAGES */
        .article-card img {
            width: 100%;
            height: 200px;
            object-fit: contain;
            background-color: #f5f5f5; 
            border-bottom: 1px solid #ddd;
            transition: transform 0.3s ease;
        }
        .article-card img:hover {
            transform: scale(1.05); 
        }

        /* NAVBAR */
        .badge-cant { position:relative; top:-2px; }

        /* PRICE OUTPUT */
        #priceOutput { font-weight:bold; display:block; text-align:center; margin-top:5px; }

        /* MAIN FLEX */
        #main-content {
            flex-grow: 1;
            padding: 20px;
            overflow-x: hidden;
        }
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
<div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasCarrito" aria-labelledby="offcanvasCarritoLabel">
    <div class="offcanvas-header">
        <h5 class="offcanvas-title" id="offcanvasCarritoLabel">Carrito</h5>
        <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
    </div>
    <div class="offcanvas-body">
        <% if (ListCarrito.Count == 0) { %>
            <div class="text-center">
                <img src="/Content/emptycart.png" class="img-fluid" alt="Carrito vacío" />
                <p>El carrito está vacío.</p>
            </div>
        <% } else { %>
            <% foreach (ClasesdeDominio.ArticuloCarrito articulo1 in ListCarrito) { %>
                <div class="card mb-3">
                    <div class="row g-0">
                        <div class="col-4">
                            <img src="<%= !string.IsNullOrEmpty(articulo1.listImagenes[0].urlImagen) ? articulo1.listImagenes[0].urlImagen : "/Content/noimage.png" %>"
                                 class="img-fluid" alt="..." />
                        </div>
                        <div class="col-8">
                            <div class="card-body">
                                <h6 class="card-title"><%= articulo1.nombre %></h6>
                                <p class="card-text">Cantidad: <%= articulo1.cant %></p>
                                <p class="card-text">Precio: $<%= articulo1.precio %></p>
                                <!-- BOTÓN PARA ELIMINAR -->
                                <a href="Default.aspx?id=<%= articulo1.id %>&action=0" class="btn btn-danger btn-sm mt-2">Eliminar</a>
                            </div>
                        </div>
                    </div>
                </div>
            <% } %>
            <div class="d-flex justify-content-between mt-3">
                <h5>Total: $<%= TotalCarrito %></h5>
                <a href="#" class="btn btn-primary">Finalizar compra</a>
            </div>
        <% } %>
    </div>
</div>


<!-- CONTENEDOR PRINCIPAL -->
<div class="d-flex">

    <!-- SIDEBAR -->
    <div id="sidebar" class="bg-light p-3">
        <h5>Filtros</h5>

        <label class="sidebar-label">Categoría</label>
        <select id="CategoryFilter" runat="server" class="form-select mb-2">
            <option value="">Seleccionar todo</option>
        </select>

        <label class="sidebar-label">Marca</label>
        <select id="BrandFilter" runat="server" class="form-select mb-2">
            <option value="">Seleccionar todo</option>
        </select>

        <label class="sidebar-label">Precio máximo</label>
        <input type="range" id="price-range" min="0" max="100000" step="100" class="form-range mb-1" />
        <output id="priceOutput">100000</output>
    </div>

    <!-- LISTADO ARTÍCULOS -->
    <div id="main-content" class="flex-grow-1">
        <div class="row row-cols-1 row-cols-md-3 g-4">
            <% foreach (ClasesDeDominio.Articulo articulo in ListArticulos) { %>
            <div class="col article-card"
                 data-price="<%= articulo.precio %>"
                 data-category="<%= articulo.categoria.id %>"
                 data-brand="<%= articulo.marca.id %>">
                <div class="card h-100">
                    <img src="<%= articulo.listImagenes.Count > 0 ? articulo.listImagenes[0].urlImagen : "/Content/noimage.png" %>"
                         class="card-img-top" onerror="this.src='/Content/noimage.png'" />
                    <div class="card-body">
                        <h5 class="card-title"><%= articulo.nombre %></h5>
                        <p class="card-text">$<%= articulo.precio %></p>
                    </div>
                    <div class="card-footer text-center">
                        <a href="DetalleArticulo.aspx?id=<%= articulo.id %>" class="btn btn-secondary btn-sm me-1">Detalle</a>
                        <a href="Default.aspx?id=<%= articulo.id %>&action=1" class="btn btn-primary btn-sm">Agregar</a>
                    </div>
                </div>
            </div>
            <% } %>
        </div>
    </div>

</div>


<!-- FOOTER -->
<footer>
    <div class="container text-center">
        <p class="mb-1">© 2024 Minecart</p>
        <small>TP ASP.NET</small>
    </div>
</footer>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
<script>
    const category = document.getElementById('<%= CategoryFilter.ClientID %>');
    const brand = document.getElementById('<%= BrandFilter.ClientID %>');
    const priceRange = document.getElementById('price-range');
    const priceOutput = document.getElementById('priceOutput');

    category.addEventListener('change', filtrar);
    brand.addEventListener('change', filtrar);
    priceRange.addEventListener('input', filtrar);

    function filtrar() {
        priceOutput.textContent = priceRange.value;
        document.querySelectorAll('.article-card').forEach(card => {
            const p = parseFloat(card.dataset.price);
            const c = card.dataset.category;
            const b = card.dataset.brand;

            const okPrice = p <= priceRange.value;
            const okCat = !category.value || c === category.value;
            const okBrand = !brand.value || b === brand.value;

            card.style.display = (okPrice && okCat && okBrand) ? 'block' : 'none';
        });
    }
</script>

</form>
</body>
</html>
