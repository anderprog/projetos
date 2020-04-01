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
using IndraPortal.Helper;
using System.Data.OleDb;

namespace IndraPortal.View.Requisicao
{
    
    public partial class NovaRequisicao : System.Web.UI.Page
    {
        private DataTable dt = new DataTable("tesr");

        private string strusuario ="";
        

        private int numrequisicaoretorno = 0;

        private string strtipo { get; set; }
        private string strvalor { get; set; }
        private string strPlanoDest { get; set; }
        private string strPacDest { get; set; }
        private string strMultaDesc { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {


            strusuario = (String)Session["usuario"];
            //carregaNumItens();

            if (!IsPostBack)
            {
                carregaNumItens();
                //Inicializa dropdown nº de itens com 10 linhas
                DataTable dtvalid = new DataTable("valid");

                dtvalid.Columns.Add("Registro", typeof(string));
                dtvalid.Columns.Add("Erro", typeof(string));

                Session.Add("valid", dtvalid);

                ddlItensPg.SelectedIndex = 10;

                DataTable dt = new DataTable("tesr");

                dt.Columns.Add("Tipo", typeof(string));

                dt.Columns.Add("Registro", typeof(string));

                dt.Columns.Add("Plano Destino", typeof(string));

                dt.Columns.Add("Pacote Destino", typeof(string));

                dt.Columns.Add("Multa Desconto", typeof(string));
                gridDt.DataSource = dt;
                gridDt.DataBind();

                Session.Add("tesr", dt);
                Session["gravar"] = "0";
            }


        }

        protected void GravarDados(GridView grid, out int prequisicao)
        {


            string conexao = ConfigurationManager.AppSettings["StringConexao"];
            int erro = 0;



            SqlConnection con = new SqlConnection(conexao);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            int strRetRequisicao = 0;


            try
            {
                // set up connection and command to do INSERT  


                SqlCommand cmd = new SqlCommand("Spr_Ins_Requisicao", con);
                cmd.Parameters.Add("@Usr_Requisicao", SqlDbType.VarChar, 50).Value = strusuario;
                cmd.Parameters.Add("@TpProcesso", SqlDbType.VarChar, 50).Value = "Calculo de Multas";
                cmd.Parameters.Add("@NumRequisicao", SqlDbType.Int).Direction = ParameterDirection.Output;
                

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                string req = cmd.Parameters["@NumRequisicao"].Value.ToString();
                strRetRequisicao = Int32.Parse(req);
                prequisicao = strRetRequisicao;





                for (int i = 0; i < grid.Rows.Count; i++)
                {
                    //limpo os parâmetros

                    //crio os parâmetro do comando
                    //e passo as linhas do dgvClientes para eles
                    //onde a célula indica a coluna do dgv
                    string strTpRequisicao = grid.Rows[i].Cells[1].Text;
                    string strValorReq = grid.Rows[i].Cells[2].Text;
                    string strmulta = "";
                    string strpacote = "";
                    string strplano="";

                    if (grid.Rows[i].Cells[3].Text == "&nbsp;")
                    {
                        strpacote = "";
                    }
                    else
                    { strpacote = grid.Rows[i].Cells[3].Text; }

                    if (grid.Rows[i].Cells[4].Text == "&nbsp;")
                    {
                        strplano = "";
                    }
                    else
                    { strplano = grid.Rows[i].Cells[4].Text; }

                    if (grid.Rows[i].Cells[5].Text == "&nbsp;")
                    {
                        strmulta = "";
                    }
                    else
                    { strmulta = grid.Rows[i].Cells[5].Text; }
                    
                    
                    string strTp = "";

                    switch (strTpRequisicao.ToUpper())
                    {
                        case "CNPJ":
                            strTp = "C";
                            break;
                        case "CONTA":
                            strTp = "N";
                            break;
                        case "LINHA":
                            strTp = "L";
                            break;
                        case "ARQUIVO":
                            strTp = "A";
                            break;
                        default:
                            Console.WriteLine("Default case");
                            break;
                    }


                    //insere os detalhes da requisicao

                    // set up connection and command to do INSERT  

                    SqlCommand cmdDetalhe = new SqlCommand("Spr_Ins_Calculo_Multa", con);
                    cmdDetalhe.Parameters.Add("@Id_Requisicao", SqlDbType.Int, int.MaxValue).Value = strRetRequisicao;
                    cmdDetalhe.Parameters.Add("@TipoReq", SqlDbType.VarChar, 50).Value = strTp;
                    cmdDetalhe.Parameters.Add("@Valor_Req", SqlDbType.VarChar, 50).Value = strValorReq;
                    cmdDetalhe.Parameters.Add("@Arquivo", SqlDbType.VarChar).Value = "";
                    cmdDetalhe.Parameters.Add("@Usr_Requisicao", SqlDbType.VarChar, 50).Value = strusuario;
                    cmdDetalhe.Parameters.Add("@Pacote_Destino", SqlDbType.VarChar, 150).Value = strpacote;
                    cmdDetalhe.Parameters.Add("@Plano_Destino", SqlDbType.VarChar, 150).Value = strplano;
                    cmdDetalhe.Parameters.Add("@Multa_Desconto", SqlDbType.VarChar, 50).Value = strmulta;


                        cmdDetalhe.Transaction = tran;
                    cmdDetalhe.CommandType = CommandType.StoredProcedure;
                    cmdDetalhe.ExecuteNonQuery();

                }
            }

            catch (Exception ex)
            {
                erro = 1;

                throw ex;
            }
            finally
            {
                if (erro == 1)
                {
                    tran.Rollback();
                }
                else
                {
                    tran.Commit();
                }

                con.Close();

            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        /// <summary>
        /// Inicializa o dropdown de nº de itens com os valores contidos no banco
        /// </summary>
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

        /// <summary>
        /// Utiliza os dados armazenados na Session para recarregar a grid
        /// </summary>
        protected void carregaGrid()
        {
            gridDt.DataSource = (DataTable)Session["DtGrid"];
            gridDt.DataBind();
        }

        #region Métodos auxiliares de validação
        /// <summary>
        /// Limpa campos da modal
        /// </summary>
        private void limpaModal()
        {
            ddCalcMultas.SelectedIndex = 0;
            txtNrRegistro.Text = "";
            txtPlanoDestino.Text = "";
            txtPacoteDestino.Text = "";
            txtMultaDesconto.Text = "";
        }

        private bool validaCamposModal()
        {
            bool cond = true;
            string strmsg = "";
            strmsg = "Preencha todos os campos!";

            strtipo = ddCalcMultas.SelectedItem.Value;
            if (strtipo.Equals("Selecione_Opcao"))
            {
                Response.Write("<script>alert('Selecione um tipo de registro!');</script>");
                return false;
            }

           
            strvalor = txtNrRegistro.Text;
            if (string.IsNullOrEmpty(strvalor))
                cond = false;

            strPlanoDest = txtPlanoDestino.Text;
            //if (string.IsNullOrEmpty(strPlanoDest))
            //    cond = false;

            strPacDest = txtPacoteDestino.Text;
            //if (string.IsNullOrEmpty(strPacDest))
            //cond = false;

            strMultaDesc = txtMultaDesconto.Text;
            //if (string.IsNullOrEmpty(strMultaDesc))
            //cond = false;

            if (strtipo == "Conta" && strvalor.Length!=10)
            {
                strmsg = "A conta deverá conter 10 digitos.";
                cond = false;
            }

            if (!cond)
            {
                Response.Write("<script>alert('" + strmsg + "');</script>");
                return cond;
            }
            else
                return cond;
        }
        protected bool ValidaDados()
        {
            bool retorno = true;
            DataTable dt = (DataTable)Session["valid"];
            dt.Rows.Clear();
            foreach (GridViewRow gvrow in gridDt.Rows)
            {
                
                string strtipo = gvrow.Cells[1].Text;
                string strvalor = gvrow.Cells[2].Text;
                

                if (strtipo.ToUpper() == "CNPJ")
                {
                    if (IndraPortal.Classes.clsValidacoes.validarCNPJ(strvalor) == false)
                    {
                        dt.Rows.Add(strvalor, "Número do CNPJ inválido.");
                        retorno = false;
                    }
                }
                 else if (strtipo.ToUpper() == "LINHA")
                {
                    if (IndraPortal.Classes.clsValidacoes.TelefoneValido(strvalor) == false)
                    {
                        dt.Rows.Add(strvalor, "Formato do Telefone Incorreto.");
                        retorno = false;
                    }
                    
                }
                

            }
            return retorno;

        }

        protected bool ValidaDados(bool parquivo)
        {
            bool retorno = true;
            DataTable dt = (DataTable)Session["valid"];
            int contCNPJ = 0;
            int contCONTA = 0;
            int contColuna = 0;
            bool existeLinha = false;
            foreach (GridViewRow gvrow in gridDt.Rows)
            {
                contColuna = 0;
                for (int i = 0; i <= 2; i++)

                {
                    
                        string strtipo = "";
                        string strvalor = "";

               

                        if (i == 1 && gvrow.Cells[i].Text != "&nbsp;")
                        {
                            strtipo = "CONTA";
                            strvalor = gvrow.Cells[i].Text;
                            contCONTA = contCONTA + 1;
                        }
                        else if (i == 2 && gvrow.Cells[i].Text != "&nbsp;")
                        {
                            strtipo = "LINHA";
                            strvalor = gvrow.Cells[i].Text;
                        }



                        if (strtipo.ToUpper() == "CNPJ")
                        {
                            existeLinha = true;
                            contColuna = contColuna+ 1;
                            if (IndraPortal.Classes.clsValidacoes.validarCNPJ(strvalor) == false)
                            {
                                dt.Rows.Add(strvalor, "Número do CNPJ inválido.");
                                retorno = false;
                            }
                        }
                        else if (strtipo.ToUpper() == "CONTA")
                        {
                            existeLinha = true;
                            contColuna = contColuna + 1;
                            if (strvalor.Length !=10)
                            {
                                dt.Rows.Add(strvalor, "A Conta deverá ter 10 digitos");
                                retorno = false;
                            }

                        }
                        else if (strtipo.ToUpper() == "LINHA")
                        {
                            existeLinha = true;
                            if (contColuna == 0)
                            {
                                contColuna = 1;

                            }
                            else

                            {
                                contColuna = contColuna + 1;
                            }
                        ;
                            if (IndraPortal.Classes.clsValidacoes.TelefoneValido(strvalor) == false)
                            {
                                dt.Rows.Add(strvalor, "Formato do Telefone Incorreto.");
                                retorno = false;
                            }

                        }


                    
                    
                }
                if (contColuna > 1)
                {
                    dt.Rows.Add("ALERTA", "Favor a requisição aceita somente 1 Conta ou Linha(s).");
                    retorno = false;
                }

            }

            if (contCNPJ > 1)
            {
                dt.Rows.Add("ALERTA", "Será permitido apenas um CNPJ por requisição");
                retorno = false;
            }
            if (contCONTA  > 1)
            {
                dt.Rows.Add("ALERTA", "Será permitido apenas uma Conta por requisição");
                retorno = false;
            }

            if (existeLinha == false)
            {   
                dt.Rows.Add("ALERTA", "Não é possivel a importação de uma planilha vazia");
                retorno = false;
            }
            return retorno;

        }
        protected bool VerificaValorInserido()
        {
            string valorGrid = "";
            string valorForm = "";

            foreach (GridViewRow gvrow in gridDt.Rows)
            {
                if (strtipo.ToUpper() == "CNPJ" || strtipo.ToUpper() == "CONTA" )
                {
                    if (gvrow.Cells[1].Text.ToUpper() == "CNPJ" || gvrow.Cells[1].Text.ToUpper() == "CONTA" || gvrow.Cells[1].Text.ToUpper() == "LINHA")
                    { 
                        return false;

                    }
                }
                else

                {
                    if (strtipo.ToUpper() == "LINHA" )
                    {
                        valorGrid = gvrow.Cells[2].Text;
                        valorForm = strvalor;

                        valorGrid = String.Join("", System.Text.RegularExpressions.Regex.Split(valorGrid, @"[^\d]"));
                        valorForm = String.Join("", System.Text.RegularExpressions.Regex.Split(strvalor, @"[^\d]"));

                        if (strtipo == gvrow.Cells[1].Text &&
                            valorGrid == valorForm)
                        {
                            return false;

                        }

                        if (gvrow.Cells[1].Text.ToUpper() == "CNPJ" || gvrow.Cells[1].Text.ToUpper() == "CONTA" )
                        {
                            return false;

                        }
                    }
                }
                
            }
            return true;

        }
        #endregion

        #region Actions
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            var pupload = Request.Form["fakeupload"];
            if (string.IsNullOrEmpty(pupload.ToString()))
                {

                if (validaCamposModal())
                    {
                        if (VerificaValorInserido())
                        {
                            DataTable dt = (DataTable)Session["tesr"];
                        strvalor = String.Join("", System.Text.RegularExpressions.Regex.Split(strvalor, @"[^\d]"));
                        dt.Rows.Add(strtipo, strvalor, strPlanoDest, strPacDest, strMultaDesc);

                            gridDt.PageSize = Int16.Parse(ddlItensPg.SelectedItem.Value);

                            gridDt.DataSource = dt;

                            Session["DtGrid"] = dt;

                            gridDt.DataBind();

                            txtNrRegistro.Text = "";

                            limpaModal();
                        }
                        else
                        {
                            Response.Write("<script>alert('Não é possivel adicionar o tipo (" + ddCalcMultas.Text + ") nesta requisição.');</script>");
                        }
                    }
                
                }
                else
                {
                
                    if (string.IsNullOrEmpty(myFile.PostedFile.FileName.ToString()))
                    {
                        txtNrRegistro.Text = "";
                        limpaModal();
                        Response.Write("<script>alert('Não é possivel abrir uma requisição , quando há um arquivo selecionado.');</script>");
                    }

                }

        }
        protected void ddCalcMultas_Click(object sender, EventArgs e)
        {
            

            


        }
        protected void btnEnviar_Click(object sender, EventArgs e)
        {

            var pupload = Request.Form["fakeupload"];
            if (pupload == "")
            {
                DataTable dt = (DataTable)Session["tesr"];
                if (dt.Rows.Count > 0)
                {


                    if (ValidaDados())
                    {
                        int requisicao = 0;
                        GravarDados(gridDt, out requisicao);

                        Session["gravar"] = "0";
                        gridDt.DataSource = null;
                        gridDt.DataBind();

                        lblNRequisicao.Text = requisicao.ToString();
                        numrequisicaoretorno = requisicao;

                        //Response.Redirect("/View/Requisicao/SucessoRequisicao.aspx?requisicao=" + );
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowModal", " showModal();", true);
                    }
                    else

                    {
                        Response.Redirect("/View/Requisicao/FalhaRequisicao.aspx");

                    }
                }
                else
                {
                    Response.Write("<script>alert('Não é possível a gravação de uma requisição vazia.');</script>");
                }
            }

            else
            {
                this.GravarArquivo();
            }
            


            if (numrequisicaoretorno >0 )
            {
                Response.Redirect("/View/Requisicao/SucessoRequisicao.aspx?requisicao=" + numrequisicaoretorno.ToString());
            }
        }
        protected void PopulateGrid()
        {
            // CHECK IF A FILE HAS BEEN SELECTED.
            if (myFile.PostedFile.FileName != "")
            {
                if (!Convert.IsDBNull(myFile.PostedFile) &
                        myFile.PostedFile.ContentLength > 0)
                {
                    // SAVE THE SELECTED FILE IN THE ROOT DIRECTORY.
                    FileInfo arquivo = new FileInfo (myFile.PostedFile.FileName);

                    myFile.PostedFile.SaveAs(Server.MapPath(".") + "\\" + arquivo.Name );
                    
                    // SET A CONNECTION WITH THE EXCEL FILE.
                    OleDbConnection myExcelConn = new OleDbConnection
                    
                        ("Provider=Microsoft.ACE.OLEDB.12.0; " +
                            "Data Source=" + Server.MapPath(".") + "\\" + arquivo.Name  +
                            ";Extended Properties=Excel 12.0;");
                    
                        try
                    {
                        myExcelConn.Open();
                        DataTable dte = myExcelConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string getExcelSheetName = dte.Rows[0]["Table_Name"].ToString();
                        // GET DATA FROM EXCEL SHEET.
                        OleDbCommand objOleDB =
                            new OleDbCommand(@"SELECT * FROM [" + getExcelSheetName + @"]", myExcelConn);

                        // READ THE DATA EXTRACTED FROM THE EXCEL FILE.
                        OleDbDataReader objBulkReader = null;
                        objBulkReader = objOleDB.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(objBulkReader);

                        // FINALLY, BIND THE EXTRACTED DATA TO THE GRIDVIEW.
                        gridDt.DataSource = dt;
                        gridDt.DataBind();

                        
                        
                    }
                    catch (Exception ex)
                    {
                        // SHOW ERROR MESSAGE, IF ANY.
                        throw ex;
                    }
                    finally
                    {
                        // CLEAR.
                        myExcelConn.Close(); myExcelConn = null;
                        File.Delete(Server.MapPath(".") + "\\" + arquivo.Name );
                    }
                }
            }
        }


        protected void PopulateGridCSV()
        {
            DataTable dtDataSource = new DataTable();
            FileInfo arquivo = new FileInfo(myFile.PostedFile.FileName);

            
            //Read all lines from selected file and assign to string array variable.
            myFile.PostedFile.SaveAs(Server.MapPath(".") + "\\" + arquivo.Name );
            string caminho = Server.MapPath(".") + "\\" + myFile.PostedFile.FileName;
            string[] fileContent = File.ReadAllLines(caminho);
            
            //Checks fileContent count > 0 then we have some lines in the file. If = 0 then file is empty
            if (fileContent.Count() > 0)
            {
                //In CSV file, 1st line contains column names. When you read CSV file, each delimited by ','.
                //fileContent[0] contains 1st line and splitted by ','. columns string array contains list of columns.
                string[] columns = fileContent[0].Split(';');
                for (int i = 0; i < columns.Count(); i++)
                {
                    dtDataSource.Columns.Add(columns[i]);
                }

                //Same logic for row data.
                for (int i = 1; i < fileContent.Count(); i++)
                {
                    string[] rowData = fileContent[i].Split(';');
                    dtDataSource.Rows.Add(rowData);
                }
            }
            File.Delete(Server.MapPath(".") + "\\" + arquivo.Name);
            gridDt.DataSource = dtDataSource;
            gridDt.DataBind();
        }
            

        protected void btnExcelRequi_Click(object sender, EventArgs e)
        {
            ExportExcel(gridDt);
        }

        protected void ExportExcel(GridView grd)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Multas.xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            grd.GridLines = GridLines.Both;
            grd.HeaderStyle.Font.Bold = true;
            grd.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }

        protected void gridDt_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridDt.PageIndex = e.NewPageIndex;

            carregaGrid();
        }
        protected void GravarArquivo()
        {
            if (myFile.PostedFile.FileName != "")
            {

                // File was sent
                var postedFile = myFile.PostedFile;
                int dataLength = postedFile.ContentLength;
                byte[] excelContents = new byte[dataLength];
                postedFile.InputStream.Read(excelContents, 0, dataLength);

                // get all the bytes of the file into memory  
                string extensao = System.IO.Path.GetExtension(myFile.PostedFile.FileName);

                if (extensao.ToUpper() == ".XLS" || extensao.ToUpper() == ".XLSX")
                {
                    this.PopulateGrid();


                    if (ValidaDados(true))
                    {

                        string strBase64 = Convert.ToBase64String(excelContents);
                        string conexao = ConfigurationManager.AppSettings["StringConexao"];

                        DataTable dt1 = excelContents.ToDataTable();
                        gridDt.DataSource = dt1;
                        gridDt.DataBind();

                        string strTpRequisicao = "ARQUIVO";
                        int erro = 0;

                        SqlConnection con = new SqlConnection(conexao);
                        con.Open();
                        SqlTransaction transaction;
                        transaction = con.BeginTransaction("SampleTransaction");

                        try
                        {

                            int strRetRequisicao = 0;


                            // set up connection and command to do INSERT  


                            SqlCommand cmd = new SqlCommand("Spr_Ins_Requisicao", con);
                            cmd.Parameters.Add("@Usr_Requisicao", SqlDbType.VarChar, 50).Value = strusuario;
                            cmd.Parameters.Add("@TpProcesso", SqlDbType.VarChar, 50).Value = strTpRequisicao;
                            cmd.Parameters.Add("@NumRequisicao", SqlDbType.Int).Direction = ParameterDirection.Output;

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Transaction = transaction;


                            cmd.ExecuteNonQuery();


                            string req = cmd.Parameters["@NumRequisicao"].Value.ToString();
                            strRetRequisicao = Int32.Parse(req);
                            numrequisicaoretorno = strRetRequisicao;




                            //insere os detalhes da requisicao

                            // set up connection and command to do INSERT  


                            SqlCommand cmdDetalhe = new SqlCommand("Spr_Ins_Calculo_Multa", con);
                            cmdDetalhe.Parameters.Add("@Id_Requisicao", SqlDbType.Int, int.MaxValue).Value = strRetRequisicao;
                            cmdDetalhe.Parameters.Add("@TipoReq", SqlDbType.VarChar, 50).Value = 'A';
                            cmdDetalhe.Parameters.Add("@Valor_Req", SqlDbType.VarChar, 50).Value = strTpRequisicao;
                            cmdDetalhe.Parameters.Add("@Arquivo", SqlDbType.VarChar).Value = strBase64;
                            cmdDetalhe.Parameters.Add("@Usr_Requisicao", SqlDbType.VarChar, 50).Value = strusuario;
                            cmdDetalhe.Transaction = transaction;
                            cmdDetalhe.CommandType = CommandType.StoredProcedure;

                            cmdDetalhe.ExecuteNonQuery();



                        }
                        catch (Exception ex)
                        {
                            erro = 1;

                            throw ex;
                        }
                        finally
                        {
                            if (erro == 1)
                            {
                                transaction.Rollback();
                            }
                            else
                            {
                                transaction.Commit();
                            }
                            con.Close();
                        }
                    }
                    else
                    {
                        Response.Redirect("/View/Requisicao/FalhaRequisicao.aspx");
                    }
                }
                else
                {
                    Response.Write("<script>alert('O Upload é permitido apenas para arquivos com extensão (.xls ou .xlsx)');</script>");
                }
            }


        }
        protected void ddlItensPg_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridDt.PageSize = Int16.Parse(ddlItensPg.SelectedItem.Value);

            carregaGrid();
        }
        #endregion
        protected void gridDt_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string id = gridDt.Rows[index].Cells[1].Text.ToString();
                Response.Write(id);
            }
        }
        protected void gridDt_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            try
            {
                DataTable dt = (DataTable)Session["tesr"];
                dt.Rows.RemoveAt(e.RowIndex);
                gridDt.DataSource = dt;
                gridDt.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void LnkButton_Click(object sender, EventArgs e)
        {
            string filename = "Modelo.xlsx";
            string Filpath = Server.MapPath("~/Modelo/" + filename);
            DownLoad(Filpath);
        }
        public void DownLoad(string FName)
        {
            string path = FName;
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
              
                Response.ContentType = "application/vnd.ms-excel"; // download […]
                Response.TransmitFile(file.FullName);
                Response.Flush();

            }
        }
    }

}
