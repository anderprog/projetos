<%@ Page Title="" Language="C#" MasterPageFile="~/View/Portal.Master" AutoEventWireup="true" CodeBehind="ConsultarRequisicao.aspx.cs" Inherits="IndraPortal.View.Consulta.ConsultarRequisicao" %>

<asp:Content ID="Content2" ContentPlaceHolderID="contentPlace" runat="server">
    <div class="">
        <div class="header-page">
            <h1>Cálculo de Multas - Consultar Requisições</h1>
        </div>
        <div class="page">
            <div class="jumbotron">

                <div class="row" runat="server">
                    <div class="col-sm-12 col-md-6 col-lg-6">
                        <div class="form-group">
                            <asp:Label ID="lblPesquisicaTipo" runat="server" Text="Pesquisar Por:"></asp:Label>
                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlPesquisaTipo" OnSelectedIndexChanged="ddlPesquisaTipo_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="Selecione uma opção" Value="1" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-6 col-lg-6" id="divNrReq" runat="server" visible="false">
                        <div class="form-group">
                            <asp:Label ID="lblNrReq" runat="server" Text="Digite o nº da requisição desejada:"></asp:Label>
                            <asp:TextBox ID="txtNrReq" runat="server"  Font-Names="nmNrReq" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-sm-12 col-md-3 col-lg-3" id="divDtIni" runat="server" visible="false">
                        <div class="form-group">
                            <asp:Label ID="lblIniJanela" runat="server" Text="Inicio da Janela:"></asp:Label>
                            <input id="inicioJanela" name="nmDtIni" class="form-control" type="date" max="6999-12-31"/>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-3 col-lg-3" id="divDtFim" runat="server" visible="false">
                        <div class="form-group">
                            <asp:Label ID="lblFimJanela" runat="server" Text="Fim da Janela:"></asp:Label>
                            <input id="fimJanela" name="nmDtFim" class="form-control" type="date" max="6999-12-31" />
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-6 ml-1" runat="server" id="opCk1" visible="false">
                        <div class="custom-control custom-checkbox">
                            <input type="checkbox" class="custom-control-input" id="ckBxDt" checked="checked" data-toggle="toggle" onchange="changeRadio(this)" />
                            <label class="custom-control-label txt" for="ckBxDt">Filtrar por data</label>
                        </div>
                    </div>
                </div>
                <%-------------------------------------------%>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button runat="server" ID="btnPesquisar" class="btn btn-primary float-right" Text="Pesquisar" OnClick="btnPesquisar_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>

        function changeRadio(radio) {
            var dateInicio = document.querySelector('#inicioJanela');
            var dateFim = document.querySelector('#fimJanela');



            if (!radio.checked) {
                dateInicio.disabled = true;
                dateFim.disabled = true;
            } else {
                dateInicio.disabled = false;
                dateFim.disabled = false;
            }
        }
    </script>
</asp:Content>
