using ClasesDeDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Negocio
{
    public class NegocioMarca
    {
        public List<Marca> listar()
        {

            List<Marca> lista = new List<Marca>();
            AccesoDatos datosArticulo = new AccesoDatos();
            try
            {
                datosArticulo.setearConsulta("SELECT Id, Descripcion FROM MARCAS");

                datosArticulo.ejecutarLectura();

                while (datosArticulo.Lector.Read())
                {
                    Marca aux = new Marca();
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
