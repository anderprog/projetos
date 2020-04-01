using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using IndraPortal.Helper;

namespace IndraPortal.View.Consulta
{
    public partial class ConsultarRequisicao : System.Web.UI.Page
    {
        private string strusuario = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                
                //this.txtNrReq.Attributes.Add("autocomplete", "OFF");
                strusuario = (String)Session["usuario"];
                CarregaDdlPesquisa();
            }
        }


        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (ddlPesquisaTipo.Text.Equals("P"))
            {

                if (Request.Form["nmDtIni"] != null && Request.Form["nmDtIni"] != "")
                {
                    string DtIni = Request.Form["nmDtIni"].ToString();
                    string DtFim = Request.Form["nmDtFim"].ToString();
                    string tp_pesquisa = "P";
                    Response.Redirect("/View/Requisicao/Requisicao.aspx?valor1=" + DtIni + "&valor2=" + DtFim + "&tp_pesquisa=" + tp_pesquisa);
                }
                //else
               //{
               //     string tp_pesquisa = "U";
               //     Response.Redirect("/View/Requisicao/Requisicao.aspx?tp_pesquisa=" + tp_pesquisa + "&usuario=" + strusuario);
               // }

            }
            else
            {
                string varpar = removerInjection(HttpUtility.HtmlDecode(txtNrReq.Text));

                string tp = ddlPesquisaTipo.Text.ToString();

                if(tp=="R" || tp == "C" || tp == "N" || tp == "L" )
                {
                        varpar = String.Join("", System.Text.RegularExpressions.Regex.Split(varpar, @"[^\d]"));

                }

                Response.Redirect("/View/Requisicao/Requisicao.aspx?&valor1=" + varpar + "&tp_pesquisa=" + ddlPesquisaTipo.SelectedValue);
            }

        }

        protected void ddlPesquisaTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlPesquisaTipo.SelectedItem.Value =="P")
            {
                opCk1.Visible = false;
                divDtIni.Visible = true;
                divDtFim.Visible = true;
                divNrReq.Visible = false;
            }
            else if (ddlPesquisaTipo.SelectedItem.Value.Equals("U"))
            {
                opCk1.Visible = false;
                divDtIni.Visible = false;
                divDtFim.Visible = false;
                divNrReq.Visible = false;
            }
            else
            {
                opCk1.Visible = false;
                divDtIni.Visible = false;
                divDtFim.Visible = false;
                divNrReq.Visible = true;

            }

            switch (ddlPesquisaTipo.SelectedItem.Value)
            {
                case "C":
                    lblNrReq.Text = "Digite o número do CNPJ.";
                    break;
                case "R":
                    lblNrReq.Text = "Digite o número da Requisição.";
                    break;
                case "N":
                    lblNrReq.Text = "Digite o número da Conta.";
                    break;
                case "Z":
                    lblNrReq.Text = "Digite a Razão Social.";
                    break;
                case "L":
                    lblNrReq.Text = "Digite o número da Linha.";
                    break;
                case "P":
                    lblNrReq.Text = "Digite o período de Abertura da Requisição.";
                    break;
                case "S":
                    lblNrReq.Text = "Digite o status da Requisição.";
                    break;
                case "Q":
                    lblNrReq.Text = "Digite o nome do usuário para pesquisa.";
                    break;
                case "A":
                    lblNrReq.Text = "Digite o nome da Área para pesquisa.";
                    break;

                default:
                    lblNrReq.Text = "";
                    break;
            }




        }

        protected void CarregaDdlPesquisa()
        {
            string conexao = ConfigurationManager.AppSettings["StringConexao"];
            DataTable tb = new DataTable();

            using (SqlConnection con = new SqlConnection(conexao))
            {
                using (SqlCommand cmd = new SqlCommand("Spr_Sel_Filtro_consulta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sqlad = new SqlDataAdapter(cmd);
                    sqlad.Fill(ds, "SelecionaNumeroItens");
                    tb = ds.Tables[0];
                }
                con.Close();
            }

            foreach (DataRow row in tb.Rows)
            {
                ddlPesquisaTipo.Items.Add(new ListItem(Utils.getDataRowField(row, "Nm_filtro"), Utils.getDataRowField(row, "Sl_Sigla")));
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
            texto = texto.Replace("*", String.Empty);
            texto = texto.Replace("-", String.Empty);
            texto = texto.Replace("+", String.Empty);
            texto = texto.Replace("&", String.Empty);
            texto = texto.Replace("*@", String.Empty);


            return texto;
        }

        protected void txtNrReq_TextChanged(object sender, EventArgs e)
        {

        }
    }

}