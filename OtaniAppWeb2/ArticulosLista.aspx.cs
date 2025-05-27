using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OtaniAppWeb2
{
    public partial class ArticulosLista : System.Web.UI.Page
    {
        public bool FiltroAvanzado
        {
            get { return chkAvanzado.Checked; }
            set { chkAvanzado.Checked = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // UX: Mensaje claro si no es admin, y uso de clase Seguridad actualizada
            if (!Seguridad.EsAdmin(Session["user"]))
            {
                Session.Add("error", "Se requiere permisos de admin para acceder a esta pantalla");
                Response.Redirect("Error.aspx");
            }

            if (!IsPostBack)
            {
                cargarArticulos();
                txtFiltro.Enabled = !FiltroAvanzado;
            }
        }
        private void cargarArticulos()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            List<Articulo> lista = negocio.listar();
            Session["listaArticulos"] = lista;
            dgvArticulos.DataSource = lista;
            dgvArticulos.DataBind();
        }

        protected void dgvArticulos_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            dgvArticulos.PageIndex = e.NewPageIndex;
            dgvArticulos.DataSource = Session["listaArticulos"];
            dgvArticulos.DataBind();
        }

        protected void dgvArticulos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = dgvArticulos.SelectedDataKey.Value.ToString();
            Response.Redirect("FormularioArticulo.aspx?id=" + id);
        }

        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> lista = (List<Articulo>)Session["listaArticulos"];
            // UX: búsqueda por nombre, insensible a mayúsculas
            List<Articulo> listaFiltrada = lista.FindAll(x => x.Nombre != null && x.Nombre.ToUpper().Contains(txtFiltro.Text.ToUpper()));
            dgvArticulos.DataSource = listaFiltrada;
            dgvArticulos.DataBind();
        }

        protected void chkAvanzado_CheckedChanged(object sender, EventArgs e)
        {
            txtFiltro.Enabled = !chkAvanzado.Checked;
        }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCriterio.Items.Clear();
            // Adaptar a los campos de Articulo
            switch (ddlCampo.SelectedItem.Text)
            {
                case "Precio":
                    ddlCriterio.Items.Add("Igual a");
                    ddlCriterio.Items.Add("Mayor a");
                    ddlCriterio.Items.Add("Menor a");
                    break;
                default:
                    ddlCriterio.Items.Add("Contiene");
                    ddlCriterio.Items.Add("Comienza con");
                    ddlCriterio.Items.Add("Termina con");
                    break;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                string campo = ddlCampo.SelectedItem.Text;
                string criterio = ddlCriterio.SelectedItem.Text;
                string filtro = txtFiltroAvanzado.Text;
                // Estado solo si implementás borrado lógico, si no, podés dejar un string vacío.
                string estado = ""; // O ddlEstado.SelectedItem.Text

                dgvArticulos.DataSource = negocio.filtrar(
                    campo,
                    criterio,
                    filtro
                    
                );
                dgvArticulos.DataBind();
            }
            catch (Exception ex)
            {
                // UX: Mejor feedback de error
                Session["error"] = "Ocurrió un error al aplicar el filtro: " + ex.Message;
                Response.Redirect("Error.aspx");
            }
        }
    }
}