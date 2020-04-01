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
    public partial class Portal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (Session["usuario"] == null)
            {
                string usuario = "";
                string senha = "";
                string conexaoAD = ConfigurationManager.AppSettings["StringLDAP"];



                string autenticar = ConfigurationManager.AppSettings["autenticar"];


                if (autenticar == "true")
                {
                    if (Request.Form["valor"] != null)
                    {
                        if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(senha))
                        {
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
                                    Session["usuario"] = usuario;

                                    Console.WriteLine("Usuário Autenticado!");
                                }
                                else
                                {
                                    Console.WriteLine("ERRO: Usuário/Senha Inválido!");
                                }
                            }


                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                    } //if Request.Form["valor"] != null
                    else
                    {
                        Response.Redirect("/View/Login.aspx");
                    }
                }
                else
                {
                    usuario = "rpabp";
                    senha = "Vivo@2019";
                    Session.Add("usuario", usuario);
                    
                }
            }

            
        }

        protected void btnRandom_Click(object sender, EventArgs e)
        {
            
            Session.RemoveAll();
            Session.Abandon();
            Response.Redirect("/Default.aspx");
        }

    }
}