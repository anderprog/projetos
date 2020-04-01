using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using IndraPortal.Helper;


namespace IndraPortal.View.Requisicao
{
    public partial class Requisicao : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                carregaDados();
                carregaNumItens();
                ddlItensPg.SelectedIndex = 10;
                gridDt.PageSize = Int16.Parse(ddlItensPg.SelectedItem.Value);
                txtFiltroVirgula.Attributes.Add("OnKeyDown", "this.value=limpa_string(this.value)");
                txtFiltroVirgula.Attributes.Add("OnKeyUp", "this.value=limpa_string(this.value)");
            }
        }

        protected void ddlItensPg_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridDt.PageSize = Int16.Parse(ddlItensPg.SelectedItem.Value);

            carregaGrid();
        }

        protected void carregaDados()
        {
            string tp_pesquisa = Request.QueryString["tp_pesquisa"];
            string strvalor = "";
            string strvalor2 = "";
            
            if (tp_pesquisa == "P")
            {
                strvalor = Request.QueryString["valor1"];
                strvalor2 = Request.QueryString["valor2"];
            }
            else if (tp_pesquisa == "U")
            {
                strvalor =(String) Session["usuario"];
            }
            else
            {
                strvalor = Request.QueryString["valor1"];
            }

            if (tp_pesquisa == "Q")
            {
                tp_pesquisa = "U";
            }
            DataSet ds = new DataSet();
            string conexao = ConfigurationManager.AppSettings["StringConexao"];
            
            SqlConnection con = new SqlConnection(conexao);

            SqlCommand cmd = new SqlCommand("Spr_Sel_Requisicao", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TipoPesquisa", tp_pesquisa);
            cmd.Parameters.AddWithValue("@Valor", strvalor);
            cmd.Parameters.AddWithValue("@Valor2", strvalor2);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable tabela = new DataTable();

            try
            {
                con.Open();
                da.Fill(tabela);
                Session["DtGrid"] = tabela;
                gridDt.DataSource = tabela;
                gridDt.DataBind();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }



        }
        protected void gridDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[5].Text = string.Format("{0:c2}", Convert.ToDouble(e.Row.Cells[5].Text));
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            }

        }
        

        protected void btnPesquisarCalc_Click(object sender, EventArgs e)
        {
            string requisicao = "";
            //VerificarSelecionados(gridDt, out requisicao);
            requisicao = removerInjection(txtFiltroVirgula.Text);
            requisicao = requisicao.Trim();
            string filtro = "";
            string[] reqs = requisicao.Split(',');
            for (int i = 0; i < reqs.Count(); i++)
            {
                if (reqs[i].Length > 38)
                {
                    filtro = reqs[i].Substring(0, 38) + ",";
                }
                else
                {
                    filtro = filtro + reqs[i].Substring(0, reqs[i].Length ) + ",";
                }
            }
            requisicao = filtro;
            if (requisicao.EndsWith(","))
                {
                requisicao = requisicao.Remove(requisicao.Length - 1);
                }


            if (requisicao != "")
            {
                Response.Redirect("/View/Requisicao/DetalheRequisicao.aspx?requisicao=" + requisicao + "&tp_pesquisa=D");
            }
            else
            {
                Response.Write("<script>alert('Selecione um item');</script>");
            }
        }

        public static string removerInjection(string texto)
        {
            texto = texto.Replace("'", String.Empty);

            texto = texto.Replace("\"", String.Empty);

            texto = texto.Replace("´", String.Empty);

            texto = texto.Replace(";", String.Empty);

            texto = texto.Replace("--", String.Empty);

            texto = texto.Replace("/", String.Empty);


            return texto;
        }

        public void VerificarSelecionados(GridView GridView1, out string req)
        {
            string strReq = "";
            string requisicao = "";
           
            foreach (GridViewRow gvrow in GridView1.Rows)
            {
                CheckBox checkbox = gvrow.FindControl("CheckBox1") as CheckBox;
                if (checkbox.Checked)
                {
                    strReq = strReq + gvrow.Cells[1].Text + ",";
                    
                }
                
            }
            if (strReq != "")
            {
                requisicao = strReq.Substring(0, strReq.Length - 1);
            }
            req = requisicao;
            

        }
        protected void carregaNumItens()
        {
            string conexao = ConfigurationManager.AppSettings["StringConexao"];
            DataTable tb = new DataTable();

            using (SqlConnection con = new SqlConnection(conexao))
            {
                using (SqlCommand cmd = new SqlCommand("Spr_Sel_numero_itens", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        con.Open();
                        DataSet ds = new DataSet();
                        SqlDataAdapter sqlad = new SqlDataAdapter(cmd);
                        sqlad.Fill(ds, "SelecionaNumeroItens");
                        tb = ds.Tables[0];
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    finally
                    {
                        con.Close();

                    }
                }
            }

            foreach (DataRow row in tb.Rows)
            {
                ddlItensPg.Items.Add(new ListItem(Utils.getDataRowField(row, "Numero_itens")));
            }
        }

        protected void gridDt_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridDt.PageIndex = e.NewPageIndex;

            carregaGrid();
        }
        protected void carregaGrid()
        {
            gridDt.DataSource = (DataTable)Session["DtGrid"];
            gridDt.DataBind();
        }

        protected void txtFiltroVirgula_TextChanged(object sender, EventArgs e)
        {

        }
    }
}