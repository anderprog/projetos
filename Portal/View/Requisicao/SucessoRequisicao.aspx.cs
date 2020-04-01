using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IndraPortal.View.Requisicao
{
    public partial class SucessoRequisicao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string requisicao = Request.QueryString["requisicao"];
            lblReq.Text = "O Numero da Requisicao é: " + requisicao;
        }
    }
}