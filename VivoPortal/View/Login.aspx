<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IndraPortal.View.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vivo Portal</title>

    <%--------------- Css <%---------------%>

    <link href="../Fonts/fontawesome-free-5.8.1-web/css/all.css?v=2019-04-27a" rel="stylesheet" type="text/css" />
    <link href="../Css/bootstrap/bootstrap.min.css?v=2019-04-27a" rel="stylesheet" />
    <%--Css principal da pagina--%>
    <link href="../Css/style.css?v=2019-05-05e" rel="stylesheet" />
    <%-------------------------------------%>


    <%----------------- JS ----------------%>
    <script src="../../Scripts/jquery-1.9.0.js?v=2019-04-27a"></script>
    <script src="../../Scripts/bootstrap/popper.min.js?v=2019-04-27a"></script>
    <script src="../../Scripts/bootstrap/bootstrap.min.js?v=2019-04-27a"></script>
    <script src="../../Scripts/bootstrap/bootstrap.js?v=2019-04-27a"></script>
    <%-------------------------------------%>
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-lg main-menu"></nav>
    </header>
    <form id="form1" runat="server">
        <div class="container">
            <div class="card-login">
                <div class="card-header login-header">
                    <h3 class="fnt">Login</h3>
                </div>
                <div class="card-body login-body">
                    <div class="input-group form-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fas fa-user"></i></span>
                        </div>
                        <asp:TextBox ID="txtusuario" runat="server" placeholder="username" Width="403px"></asp:TextBox>
                    </div>
                    <div class="input-group form-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fas fa-key"></i></span>
                        </div>
                        <asp:TextBox ID="txtsenha" runat="server" TextMode="Password" placeholder="password" Width="403px"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnEntrar" runat="server" Text="Login" CssClass="btn float-right login_btn" OnClick="btnEntrar_Click" />
                    </div>
                </div>
                <div class="card-footer">
                </div>
            </div>
        </div>
    </form>
    <footer class=" font-small main-footer">
        <div class="footer-copyright text-right py-3">
            ©  Indra 2013 - 2019. Todos os direitos reservados.
        </div>
    </footer>
</body>
</html>
