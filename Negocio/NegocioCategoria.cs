using ClasesDeDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Negocio
{
    public class NegocioCategoria
    {
        public List<Categoria> listar()
        {

            List<Categoria> lista = new List<Categoria>();
            AccesoDatos datosArticulo = new AccesoDatos();
            try
            {
                datosArticulo.setearConsulta("SELECT Id, Descripcion FROM Categorias");

                datosArticulo.ejecutarLectura();

                while (datosArticulo.Lector.Read())
                {
                    Categoria aux = new Categoria();
                    aux.id = datosArticulo.Lector.GetInt32(0);
                    aux.descripcion = (string)datosArticulo.Lector["Descripcion"];
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
    }
}
