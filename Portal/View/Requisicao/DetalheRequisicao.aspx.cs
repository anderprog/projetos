using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Text;
using ClosedXML.Excel;

namespace IndraPortal.View.Requisicao

{
    public partial class DetalheRequisicao : System.Web.UI.Page
    {
        public override void VerifyRenderingInServerForm(Control control)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strrequisicao = Request.QueryString["requisicao"];

                carregaDados();
                somavalores(strrequisicao);
            }
        }
       
        protected void carregaDados()
        {

            string strrequisicao = Request.QueryString["requisicao"];
            string tppesquisa = Request.QueryString["tp_pesquisa"];


            DataSet ds = new DataSet();
            string conexao = ConfigurationManager.AppSettings["StringConexao"];


            SqlConnection con = new SqlConnection(conexao);

            SqlCommand cmd = new SqlCommand("Spr_Sel_Calculo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TipoPesquisa", tppesquisa);
            cmd.Parameters.AddWithValue("@Valor", strrequisicao);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable tabela = new DataTable();

            try
            {
                con.Open();

                da.Fill(ds);
                DataTable dte = new DataTable();
                Session.Add("excel", dte);


                gridDt.DataSource = ds;
                gridDt.DataBind();
                Session.Add("detalhes", ds);


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

        protected void gridDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            DataRow dr = null;
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataTable dte = (DataTable)Session["excel"];
                
                var pvalor = e.Row.Cells[20].Text;
                var nconta = e.Row.Cells[4].Text;
                if (pvalor== "&nbsp;")
                {
                    pvalor = "0";
                 }
                if (nconta == "&nbsp;")
                {
                    nconta = "''";
                }
                else
                {
                    nconta = "'" + nconta + "'";
                }
                
                
                e.Row.Cells[20].Wrap = false;
                e.Row.Cells[12].Wrap = false;
                e.Row.Cells[13].Wrap = false;
                
                e.Row.Cells[20].Text = string.Format("{0:c2}", Convert.ToDouble(pvalor));
               
                e.Row.Cells[20].HorizontalAlign = HorizontalAlign.Right;
                //e.Row.Cells[4].Text = nconta.ToString();
                e.Row.Cells[4].Attributes["style"] = "mso-number-format:\\@" ;
                e.Row.Cells[2].Attributes["style"] = "mso-number-format:\\@";

                dr = dte.NewRow();
                for (int i = 0; i <= e.Row.Cells.Count - 1; i++)
                {
                    string texto = removerAcentos(HttpUtility.HtmlDecode(e.Row.Cells[i].Text));
                    dr[i] = texto;
                }
                dte.Rows.Add(dr);
                Session.Add("excel", dte);

                e.Row.Cells[14].Wrap = false;
                string str = e.Row.Cells[14].Text;
                str = str.Replace("//", "<br/>");
                e.Row.Cells[14].Text = str;

            }
            else
            {

                DataTable dt = new DataTable();
                for (int i = 0; i<= e.Row.Cells.Count-1; i++)
                {
                    
                    string texto = removerAcentos(HttpUtility.HtmlDecode(e.Row.Cells[i].Text));
                    if (texto.Trim() != "")
                    {
                        dt.Columns.Add(texto);
                    }
                    e.Row.Cells[i].Wrap = false;
                }
                
                DataTable dtteste = (DataTable)Session["excel"];
                if (dtteste.Columns.Count  == 0 )
                {
                    Session.Add("excel", dt);
                }
            }
            
        }

        protected void GrdDetalhesRequisicao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
             
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var pvalor = e.Row.Cells[1].Text;
                if (pvalor == " & nbsp;")
                {
                    pvalor = "0";
                }

                e.Row.Cells[1].Text = string.Format("{0:c2}", Convert.ToDouble(pvalor));
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                
            }

        }

        public void VerificarSelecionados(GridView GridView1, out string req)
        {
            string strReq = "";
            string requisicao = "";
            foreach (GridViewRow gvrow in GridView1.Rows)
            {
                
                    strReq = strReq + gvrow.Cells[0].Text + ",";

            }
            if (strReq != "")
            {
                requisicao = strReq.Substring(0, strReq.Length - 1);
            }
            req = requisicao;


        }


        protected void somavalores(string req)
        {

            DataSet ds = new DataSet();
            string conexao = ConfigurationManager.AppSettings["StringConexao"];


            SqlConnection con = new SqlConnection(conexao);

            SqlCommand cmd = new SqlCommand("Spr_Sel_Soma_Requisicao_Calculo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Valor", req);
            

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable tabela = new DataTable();

            try
            {
                con.Open();

                da.Fill(ds);

                GrdDetalhesRequisicao.DataSource = ds;
                GrdDetalhesRequisicao.DataBind();
                
            }
            catch 
            {
                
            }
            finally
            {
                con.Close();
            }
        }


        protected void ExportExcel(GridView grd)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Multas.xlsx";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            Response.Charset = "";
            grd.GridLines = GridLines.Both;
            grd.HeaderStyle.Font.Bold = true;
            grd.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }

        protected void ExportExcelXLSX(GridView grd)
        {


           
            DataTable dtExcel = (DataTable)Session["excel"];
            dtExcel.TableName = "Requisições";
            DataSet dt = new DataSet();
            dt.Tables.Add(dtExcel);
            using (XLWorkbook wb = new XLWorkbook())
            {
                
                wb.Worksheets.Add(dt);


                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Charset = "";
                string FileName = "Requisições.xlsx";
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";  
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                Response.Charset = "";
                
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }


        }

        private void ExportGridToCSV(GridView grd)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Requisições.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            grd.AllowPaging = false;
            //grd.DataBind();
            
            StringBuilder columnbind = new StringBuilder();
            
            for (int k = 0; k < grd.HeaderRow.Cells.Count; k++)
            {
                string texto = removerAcentos(HttpUtility.HtmlDecode(grd.HeaderRow.Cells[k].Text).ToString());
                if (k == grd.HeaderRow.Cells.Count - 1)
                {
                    columnbind.Append(texto );
                }
                else
                {
                    columnbind.Append(texto + ';');
                }
            }

            columnbind.Append("\r\n");

            foreach (GridViewRow mail in grd.Rows)
            {

                for (int k = 0; k < mail.Cells.Count; k++)
                {
                    if (mail.Cells[k].Text != "&nbsp;")
                    {
                        string texto = removerAcentos(HttpUtility.HtmlDecode(mail.Cells[k].Text).ToString());
                        if (k == mail.Cells.Count - 1)
                        {
                            columnbind.Append( texto );
                        }
                        else
                        {
                            columnbind.Append( texto.ToString() + ';');
                        }
                    }
                    else
                    {
                        if (k == mail.Cells.Count - 1)
                        {
                            columnbind.Append(" ");
                        }
                        else
                        {
                            columnbind.Append(" ;");
                        }
                        
                    }
                }
                columnbind.Append("\r\n");
            }


            Response.Write(columnbind.ToString());
            Response.Flush();
            Response.End();
            
        }

        

        protected void btnExcelRequi_Click(object sender, EventArgs e)
        {
            //ExportExcel(gridDt);
            ExportExcelXLSX(gridDt);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ExportGridToCSV(gridDt);
        }
        public static string removerAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }
    }
}