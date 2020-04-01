using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Configuration;

namespace IndraPortal.View
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            string conexaoAD = ConfigurationManager.AppSettings["StringLDAP"];

            if (!string.IsNullOrEmpty(txtusuario.Text) && !string.IsNullOrEmpty(txtsenha.Text))
            {
                string usuario = txtusuario.Text;
                string senha = txtsenha.Text;

                try
                {
                    DirectoryEntry directoryEntry = new DirectoryEntry(conexaoAD, usuario, senha);
                    DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);
                    directorySearcher.Filter = "(SAMAccountName=" + usuario + ")";
                    SearchResult searchResult = directorySearcher.FindOne();
                    if ((Int32)searchResult.Properties["userAccountControl"][0] == 512)
                    {
                        //string grupos = ClasseUsuario.Instance.GetADUserGroups(usuario, senha);
                        //string[] groups = ClasseUsuario.Instance.GetGroups(directoryEntry, conexaoAD, senha);
                        var grupos1 = ClasseUsuario.Instance.GetUserGroupsOld(usuario, senha);
                        var depart = ClasseUsuario.Instance.GetDepartment(usuario);
                        //var ptoken = ClasseUsuario.Instance.GetNestedGroupMembershipsByTokenGroup(usuario, senha);
                        Session.Add("usuario", usuario);
                        
                        Response.Redirect("/View/Home.aspx");
                    }
                    else
                    {
                        Console.WriteLine("ERRO: Usuário/Senha Inválido!");
                    }
                }


                catch (Exception ex)
                {
                    Response.Write("<script>alert('ERRO: Usuário/Senha Inválido!');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Favor digitar nome de usuário e/ou senha.');</script>");
            }
        }
    }
}