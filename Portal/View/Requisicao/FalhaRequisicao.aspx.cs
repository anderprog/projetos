using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
namespace IndraPortal.View.Requisicao
{
    public partial class FalhaRequisicao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["valid"];
            gridDt.DataSource = dt;
            gridDt.DataBind();


        }
    }
}