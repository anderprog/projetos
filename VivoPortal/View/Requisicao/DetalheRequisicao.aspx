<%@ Page Title="" Language="C#" MasterPageFile="~/View/Portal.Master" AutoEventWireup="true" CodeBehind="DetalheRequisicao.aspx.cs" Inherits="IndraPortal.View.Requisicao.DetalheRequisicao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlace" runat="server">
    <div class="header-page">
        <h1>Cálculo de Multas - Detalhe Requisição</h1>
    </div>
    <div class="page">
        <div class="jumbotron pt-3">
            <div class="row">
                <div class="col-md-12 btn-back">
                    <asp:Button runat="server" ID="btnVoltar" class="btn btn-primary float-left" Text="Voltar" OnClientClick="JavaScript: window.history.back(1); return false;"/>
                </div>
            </div>
            <div class="row moldura mt-3">
                <div class="container col-sm-12 col-md-12 col-lg-12 grid">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <div id="gridDetalheReq" class="table-editable">
                            <%--<asp:GridView CssClass="table table-striped table-hover table-sm" runat="server" ID="gridDt" ShowHeaderWhenEmpty="True" HeaderStyle-BackColor="#003366" HeaderStyle-ForeColor="#ffffff" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">--%>
                            <asp:GridView CssClass="table table-striped table-hover table-sm" runat="server" ID="gridDt" ShowHeaderWhenEmpty="True" OnRowDataBound="gridDt_RowDataBound" HeaderStyle-BackColor="#003366" HeaderStyle-ForeColor="#ffffff" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row moldura mt-3 moldura-2">
                <div class="col-sm-12 col-md-6 col-lg-6">
                    <span class="txt">Valor total de cada requisição: </span>
                    <br />
                    <asp:GridView CssClass="table table-striped table-hover table-sm" runat="server" ID="GrdDetalhesRequisicao" HeaderStyle-BackColor="#003366" HeaderStyle-ForeColor="#ffffff" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="GrdDetalhesRequisicao_RowDataBound" RowStyle-HorizontalAlign="Center">
                    </asp:GridView>
                    <br />
                </div>
                <div class="col-sm-12 col-md-6 col-lg-6">
                    <div class="row btns-pd">
                        <div class="col-sm-12 col-md-6 col-lg-6">
                            <div class="row-fluid">
                                <div class="file is-boxed float-left">
                                    <label class="file-label">
                                        <asp:Button ID="btnExcelRequi" CssClass="btn-transparent" runat="server" BorderStyle="None" ForeColor="#3366FF" Text="" OnClick="btnExcelRequi_Click" />
                                        <span class="file-cta span-icon">
                                            <span class="file-icon icon-pf">
                                                <i class="fas fa-file-excel"></i>
                                            </span>
                                            <span class="file-label text-center">Requisição.xlsx
                                            </span>
                                        </span>
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 col-md-6 col-lg-6">
                            <div class="row-fluid">
                                <div class="file is-boxed float-left">
                                    <label class="file-label">
                                        <asp:Button ID="Button1" CssClass="btn-transparent" runat="server" BorderStyle="None" ForeColor="#3366FF" Text="" OnClick="Button1_Click" />
                                        <span class="file-cta span-icon">
                                            <span class="file-icon icon-pf">
                                                <i class="fas fa-file-csv"></i>
                                            </span>
                                            <span class="file-label text-center">Requisição.csv
                                            </span>
                                        </span>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
