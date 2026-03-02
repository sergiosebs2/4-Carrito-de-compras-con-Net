using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ClasesDeDominio;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Security.Claims;

namespace Negocio
{
    public class NegocioArticulo
    {
        //private string ruta = "Data Source=localhost\\sqlexpress; Initial Catalog = CATALOGO_P3_DB; Integrated Security = True";
        //   public NegocioArticulo()
        // public string GetRuta() { return ruta; }
        public List<Articulo> listar()
        {   

            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datosArticulo = new AccesoDatos();
            try
            {
                //datosArticulo.setearConsulta("SELECT A.Id,Codigo,Nombre,A.Descripcion, M.Descripcion Marca,C.Descripcion Categoria,Precio,I.ImagenUrl FROM Articulos A, Categorias C, Marcas M, IMAGENES I WHERE A.Id = C.Id AND M.Id = A.Id AND I.Id=A.Id");
                // datosArticulo.setearConsulta("SELECT A.Id,Codigo,Nombre,A.Descripcion,A.Descripcion, A.IdMarca , A.Precio,I.ImagenUrl FROM Articulos A, IMAGENES I WHERE A.Id = I.Id");

                //Se Modifica consulta
                datosArticulo.setearConsulta("SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, M.Descripcion AS Marca, C.Descripcion AS Categoria, A.Precio, M.Id, C.Id FROM ARTICULOS A LEFT JOIN Categorias C ON A.IdCategoria = C.Id LEFT JOIN Marcas M ON A.IdMarca = M.Id");
                datosArticulo.ejecutarLectura();

                while (datosArticulo.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.id = datosArticulo.Lector.GetInt32(0);
                    aux.codigo = (string)datosArticulo.Lector["Codigo"];
                    aux.nombre = (string)datosArticulo.Lector["Nombre"];
                    aux.descripcion = (string)datosArticulo.Lector["Descripcion"];
                    //aux.marca = new Marca { id = int.Parse(textbox) };
                    //aux.categoria = new Categoria { descripcion = (string)datosArticulo.Lector["Categoria"] };
                    aux.marca = new Marca { descripcion = (string)datosArticulo.Lector["Marca"], id = datosArticulo.Lector.GetInt32(7) };
                    string categoria = string.Empty;
                    if (!datosArticulo.Lector.IsDBNull(datosArticulo.Lector.GetOrdinal("Categoria")))
                    {
                        //categoria = (string)datosArticulo.Lector["Categoria"];
                        aux.categoria = new Categoria { descripcion = (string)datosArticulo.Lector["Categoria"], id = datosArticulo.Lector.GetInt32(8) };

                    }
                    else
                    {
                        // Si la categoría es NULL en la base de datos, asigna un valor predeterminado o un mensaje alternativo.
                        categoria = "Sin categoría";
                        aux.categoria = new Categoria { descripcion = categoria };
                    }
                    
                    aux.precio = (float)datosArticulo.Lector.GetDecimal(6);
                    //corregimos para que acepte las imagenes 
                    // if (!(datosArticulo.Lector["ImagenUrl"] is DBNull))
                    //     aux.imagenArticulo = new Imagenes { urlImagen = (string)datosArticulo.Lector["ImagenUrl"] };

                    aux.listImagenes = GetImagenes(aux.id); //guardamos en la lista todas las fotos del producto .-



                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw ex;
            }
            finally
            {
                datosArticulo.CerrarConexion();
            }
        }
        public List<Imagenes>  GetImagenes(int idArti)
        {
            List<Imagenes> lista = new List<Imagenes>();
            AccesoDatos datosArticulo = new AccesoDatos();
            try
            {

                /*datosArticulo.setearConsulta("select IdArticulo , ImagenUrl from IMAGENES");
                datosArticulo.ejecutarLectura();
                while (datosArticulo.Lector.Read())
                {
                    int idArticulo = datosArticulo.Lector.GetInt32(0);
                    string urlImagen = (string)datosArticulo.Lector["ImagenUrl"];

                    Imagenes imagen = new Imagenes(idArticulo, urlImagen);
                    if (idArti == idArticulo)
                    {
                        lista.Add(imagen);
                    }

                }
                return lista;*/
                datosArticulo.setearConsulta("SELECT ImagenUrl FROM IMAGENES WHERE IdArticulo = @IdArticulo");
                datosArticulo.setearParametro("@IdArticulo", idArti);
                datosArticulo.ejecutarLectura();

                while (datosArticulo.Lector.Read())
                {
                    string urlImagen = (string)datosArticulo.Lector["ImagenUrl"];
                    Imagenes imagen = new Imagenes(idArti, urlImagen);
                    lista.Add(imagen);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /* public void agregar(Articulo aux)
         {
             AccesoDatos datos = new AccesoDatos();
             try
             {

                 datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) VALUES (" + aux.codigo + ", '" + aux.nombre + "', '" + aux.descripcion + "', " + aux.marca.id + ", " + aux.categoria.id + ", " + aux.precio + ")");
                 datos.ejecutarAccion();
             }        
             catch (Exception ex)
             {

                 throw ex;
             }
             finally
             {
                 datos.CerrarConexion();
             }
         }
       
        public void agregar(Articulo aux, string urlImagen)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Paso 1: Insertar el artículo en la tabla ARTICULOS
                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) VALUES ('" + aux.codigo + "', '" + aux.nombre + "', '" + aux.descripcion + "', " + aux.marca.id + ", " + aux.categoria.id + ", " + aux.precio + ")");
                datos.ejecutarAccion();
                datos.CerrarConexion();

                // Paso 2: Obtener el ID autonumérico del artículo recién insertado
                datos.setearConsulta("SELECT SCOPE_IDENTITY()");
                datos.ejecutarLectura();
                int idArticulo = 0;
                if (datos.Lector.Read())
                {
                    idArticulo = datos.Lector.IsDBNull(0) ? 0 : Convert.ToInt32(datos.Lector[0]);
                }
                datos.Lector.Close();
                datos.CerrarConexion();

                // Paso 3: Insertar la URL de la imagen junto con el ID del artículo en la tabla IMAGENES
                datos.setearConsulta("INSERT INTO IMAGENES (Id, ImagenUrl) VALUES (" + idArticulo + ", '" + urlImagen + "')");
                datos.ejecutarAccion();
                datos.CerrarConexion();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
         */
        public void agregar(Articulo aux, string urlImagen)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
               /* //datos.setearConsulta("SELECT MAX(Id) FROM ARTICULOS");
                datos.setearConsulta("SELECT IDENT_CURRENT('ARTICULOS')");
                datos.ejecutarAccion();

                int idArticulo = 1 + Convert.ToInt32(datos.ejecutarScalar());
                // Paso 1: Insertar el artículo en la tabla ARTICULOS
                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) VALUES ('" + aux.codigo + "', '" + aux.nombre + "', '" + aux.descripcion + "', " + aux.marca.id + ", " + aux.categoria.id + ", " + aux.precio + ");SELECT SCOPE_IDENTITY(); INSERT INTO IMAGENES (ImagenUrl, IdArticulo) VALUES ('" + urlImagen + "', " + idArticulo + ")");
                datos.ejecutarAccion();*/

                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) VALUES ('" + aux.codigo + "', '" + aux.nombre + "', '" + aux.descripcion + "', " + aux.marca.id + ", " + aux.categoria.id + ", " + aux.precio + ")");
                datos.ejecutarAccion();

                datos.setearConsulta("SELECT IDENT_CURRENT('ARTICULOS')");
                int idArticulo = Convert.ToInt32(datos.ejecutarScalar());

                datos.setearConsulta("INSERT INTO IMAGENES (ImagenUrl, IdArticulo) VALUES ('" + urlImagen + "', " + idArticulo + ")");
                datos.ejecutarAccion();



                // datos.setearConsulta("INSERT INTO IMAGENES (ImagenUrl, IdArticulo) VALUES ('" + urlImagen + "', " + idArticulo + ")");
                //datos.ejecutarAccion();
                // agregarImagen(idArticulo, urlImagen);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void modificar(Articulo articulo)
        {

            AccesoDatos datos = new AccesoDatos();


            try
            {

                datos.setearConsulta(@" 
                        UPDATE ARTICULOS 
                        SET 
                            Codigo = @numero, 
                            Nombre = @NombreA, 
                            Descripcion = @DescripcionA, 
                            IdMarca = @MarcaA, 
                            IdCategoria = @IdcategoriaA, 
                            Precio = @PrecioA  
                        WHERE 
                            id = @IdArticulo;

                        UPDATE IMAGENES 
                        SET 
                            IdArticulo = @IdarticuloI, 
                            ImagenUrl = @imURLI 
                        WHERE 
                            IdArticulo = @IdArticulo;

                        UPDATE MARCAS 
                        SET 
                            Descripcion = @DescripcionM 
                        WHERE 
                            id = @IdMarca;

                        UPDATE CATEGORIAS 
                        SET 
                            Descripcion = @DescripcionC 
                        WHERE 
                            id = @IdCategoria");
                //datos.setearConsulta(" UPDATE ARTICULOS set Codigo = @numero, Nombre = @NombreA, Descripcion = @DescripcionA, IdMarca = @MarcaA , IdCategoria = @IdcategoriaA, Precio = @PrecioA  Where id=" + articulo.id +"  ") ;
                //datos.setearConsulta(" UPDATE IMAGENES set IdArticulo = @IdarticuloI, ImagenUrl = @imURLI where id =" + articulo.imagenArticulo.idArticulo + " ");
                //datos.setearConsulta(" UPDATE MARCAS set Descripcion = @DescripcionM where id = " + articulo.marca.id + " ");
                //datos.setearConsulta(" UPDATE CATEGORIAS set Descripcion = @DescripcionC where id = " + articulo.categoria.id + " ");

                datos.setearParametro("@IdArticulo", articulo.id);
                datos.setearParametro("@IdImagen", articulo.imagenArticulo.id);
                datos.setearParametro("@IdMarca", articulo.marca.id);
                datos.setearParametro("@IdCategoria", articulo.categoria.id);
                datos.setearParametro("@numero", articulo.codigo.ToString());
                datos.setearParametro("@NombreA", articulo.nombre);
                datos.setearParametro("@DescripcionA", articulo.descripcion);
                datos.setearParametro("@MarcaA", articulo.marca.id);
                datos.setearParametro("@IdcategoriaA", articulo.categoria.id);
                datos.setearParametro("@PrecioA", articulo.precio);
                datos.setearParametro("@IdarticuloI", articulo.imagenArticulo.idArticulo);
                //datos.setearParametro("@imURLI", urlImagen);
                datos.setearParametro("@imURLI", articulo.imagenArticulo.urlImagen);
                datos.setearParametro("@DescripcionM ", articulo.marca.descripcion);
                datos.setearParametro("@DescripcionC ", articulo.categoria.descripcion);

                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }

        }


        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from ARTICULOS where id= @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        //creamos metodo filtrar
        public List<Articulo> filtrar(string categoria, string precio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, M.Descripcion AS Marca, C.Descripcion AS Categoria, A.Precio, I.ImagenUrl FROM Articulos A LEFT JOIN Categorias C ON A.IdCategoria = C.Id LEFT JOIN Marcas M ON A.IdMarca = M.Id LEFT JOIN IMAGENES I ON A.Id = I.IdArticulo where ";
                if(categoria == "Celulares")
                {
                    switch (precio)
                    {
                        case "Mayor a":
                            consulta += "A.Precio > " + filtro + "and c.Descripcion = 'Celulares'";
                            break;
                        case "Menor a":
                            consulta += "A.Precio < " + filtro + "and c.Descripcion = 'Celulares'";
                            break;
                        default:
                            consulta += "A.Precio = " + filtro + "and c.Descripcion = 'Celulares'";
                            break;
                    }
                }
                else if(categoria == "Televisores")
                {
                    switch (precio)
                    {
                        case "Mayor a":
                            consulta += "A.Precio > " + filtro + "and c.Descripcion = 'Televisores'";
                            break;
                        case "Menor a":
                            consulta += "A.Precio < " + filtro + "and c.Descripcion = 'Televisores'";
                            break;
                        default:
                            consulta += "A.Precio = " + filtro + "and c.Descripcion = 'Televisores'";
                            break;
                    }

                }
                else if (categoria == "Media")
                {
                    switch (precio)
                    {
                        case "Mayor a":
                            consulta += "A.Precio > " + filtro + "and c.Descripcion = 'Media'";
                            break;
                        case "Menor a":
                            consulta += "A.Precio < " + filtro + "and c.Descripcion = 'Media'";
                            break;
                        default:
                            consulta += "A.Precio = " + filtro + "and c.Descripcion = 'Media'";
                            break;
                    }

                }
                else 
                {
                    switch (precio)
                    {
                        case "Mayor a":
                            consulta += "A.Precio > " + filtro + "and c.Descripcion = 'Audio'";
                            break;
                        case "Menor a":
                            consulta += "A.Precio < " + filtro + "and c.Descripcion = 'Audio'";
                            break;
                        default:
                            consulta += "A.Precio = " + filtro + "and c.Descripcion = 'Audio'";
                            break;
                    }

                }


                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.id = datos.Lector.GetInt32(0);
                    aux.codigo = (string)datos.Lector["Codigo"];
                    aux.nombre = (string)datos.Lector["Nombre"];
                    aux.descripcion = (string)datos.Lector["Descripcion"];
                    //aux.marca = new Marca { id = int.Parse(textbox) };
                    //aux.categoria = new Categoria { descripcion = (string)datosArticulo.Lector["Categoria"] };
                    aux.marca = new Marca { descripcion = (string)datos.Lector["Marca"] };
                    string categori = string.Empty;
                    if (!datos.Lector.IsDBNull(datos.Lector.GetOrdinal("Categoria")))
                    {
                        categori = (string)datos.Lector["Categoria"];
                    }
                    else
                    {
                        // Si la categoría es NULL en la base de datos, asigna un valor predeterminado o un mensaje alternativo.
                        categori = "Sin categoría";
                    }
                    aux.categoria = new Categoria { descripcion = categori };
                    aux.precio = (float)datos.Lector.GetDecimal(6);
                    //corregimos para que acepte las imagenes 
                    //if (!(datos.Lector["ImagenUrl"] is DBNull))
                        //aux.imagenArticulo = new Imagenes { urlImagen = (string)datos.Lector["ImagenUrl"] };


                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex )
            {

                throw ex;
            }
        }
        /*private void agregarImagen(int idArticulo, string urlImagen)
{
   AccesoDatos datos = new AccesoDatos();
   try
   {
       datos.setearConsulta("INSERT INTO IMAGENES (ImagenUrl, IdArticulo) VALUES ('" + urlImagen + "', " + idArticulo + ")");
       datos.ejecutarAccion();
   }
   catch (Exception ex)
   {
       MessageBox.Show(ex.ToString());
       throw ex;
   }
   finally
   {
       datos.CerrarConexion();
   }
}
*/
        public Articulo obtenerPorId(int id)
        {
            Articulo articulo = null;
            AccesoDatos datosArticulo = new AccesoDatos();
            try
            {
                datosArticulo.setearConsulta("SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, M.Descripcion AS Marca, C.Descripcion AS Categoria, A.Precio, M.Id, C.Id FROM ARTICULOS A LEFT JOIN Categorias C ON A.IdCategoria = C.Id LEFT JOIN Marcas M ON A.IdMarca = M.Id WHERE A.Id = @Id");
                datosArticulo.setearParametro("@Id", id);
                datosArticulo.ejecutarLectura();

                if (datosArticulo.Lector.Read())
                {
                    articulo = new Articulo();
                    articulo.id = datosArticulo.Lector.GetInt32(0);
                    articulo.codigo = (string)datosArticulo.Lector["Codigo"];
                    articulo.nombre = (string)datosArticulo.Lector["Nombre"];
                    articulo.descripcion = (string)datosArticulo.Lector["Descripcion"];
                    articulo.marca = new Marca { descripcion = (string)datosArticulo.Lector["Marca"], id = datosArticulo.Lector.GetInt32(7) };
                    string categoria = string.Empty;
                    if (!datosArticulo.Lector.IsDBNull(datosArticulo.Lector.GetOrdinal("Categoria")))
                    {
                        articulo.categoria = new Categoria { descripcion = (string)datosArticulo.Lector["Categoria"], id = datosArticulo.Lector.GetInt32(8) };
                    }
                    else
                    {
                        categoria = "Sin categoría";
                        articulo.categoria = new Categoria { descripcion = categoria };
                    }
                    articulo.precio = (float)datosArticulo.Lector.GetDecimal(6);
                    articulo.listImagenes = GetImagenes(id); // Guarda en la lista todas las fotos del producto.
                }

                return articulo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw ex;
            }
            finally
            {
                datosArticulo.CerrarConexion();
            }
        }
    }
}      