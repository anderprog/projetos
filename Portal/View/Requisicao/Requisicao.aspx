<%@ Page Title="" Language="C#" MasterPageFile="~/View/Portal.Master" AutoEventWireup="true" CodeBehind="Requisicao.aspx.cs" Inherits="IndraPortal.View.Requisicao.Requisicao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlace" runat="server">
    <div class="">
        <div class="header-page">
            <h1>Cálculo de Multas - Todas usuario</h1>
        </div>
        <div class="page">
            <div class="jumbotron">
                <div class="row moldura alertaModal">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <div class="form-group">
                            <asp:Label ID="lblNrReq" runat="server" Text="Digite o(s) de requisição que deseja abrir os detalhes, separados por virgula."></asp:Label>
                            <asp:TextBox ID="txtFiltroVirgula" runat="server"  Font-Names="nmNrReq" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-12 col-lg-12 ddl-itens-pg">
                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="txtItensPg">Itens por página</span>
                            </div>
                            <div class="input-group-prepend">
                                <asp:DropDownList ID="ddlItensPg" runat="server" CssClass="btn dropdown-toggle ddl-itens" OnSelectedIndexChanged="ddlItensPg_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <asp:GridView CssClass="table table-striped table-hover table-sm" runat="server" ID="gridDt" ShowHeaderWhenEmpty="True" HeaderStyle-BackColor="#003366" HeaderStyle-ForeColor="#ffffff" HeaderStyle-HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="gridDt_PageIndexChanging" OnRowDataBound="gridDt_RowDataBound" RowStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" BackColor="#003366" ForeColor="White"></HeaderStyle>
                                    <RowStyle HorizontalAlign="Center"></RowStyle>
                        </asp:GridView>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 mt-5">
                        <asp:Button runat="server" ID="btnPesquisarCalc" class="btn btn-primary float-right" Text="Abrir detalhes" OnClick="btnPesquisarCalc_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
    function limpa_string(S){
         // Deixa so' os digitos no numero
         var Digitos = "0123456789,";
         var temp = "";
         var digito = "";
            for (var i=0; i<S.length; i++){
              digito = S.charAt(i);
              if (Digitos.indexOf(digito)>=0){temp=temp+digito}
            }
           return temp
        }
        </script>
</asp:Content>
