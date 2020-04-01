<%@ Page Title="" Language="C#" MasterPageFile="~/View/Portal.Master" AutoEventWireup="true" CodeBehind="FalhaRequisicao.aspx.cs" Inherits="IndraPortal.View.Requisicao.FalhaRequisicao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlace" runat="server">
    <div class="header-page">
        <h1>Cálculo de Multas - Falha de requisição</h1>
    </div>
    <div class="page title-pg">
        <div class="txt-header-pg">
            <h3>Foram encontrados os seguintes erros na sua requisição:</h3>
        </div>
        <div class="jumbotron">
            <div class="row">
                <div class="container">
                    <div class="row moldura">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <div id="gridDetalheReq" class="table-editable">
                                <asp:GridView CssClass="table table-striped table-hover table-sm" runat="server" ID="gridDt" ShowHeaderWhenEmpty="True" HeaderStyle-BackColor="#003366" HeaderStyle-ForeColor="#ffffff" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">
                                    <%--      <Columns>
                                    <asp:BoundField DataField="Campo1" HeaderText="Campo1" />
                                    <asp:BoundField DataField="Campo2" HeaderText="Campo2" />
                                    <asp:BoundField DataField="Campo3" HeaderText="Campo3" />
                                </Columns>
                                <HeaderStyle Font-Size="Smaller" />
                                <PagerStyle Font-Size="Smaller" />
                                <RowStyle Font-Size="Smaller" />
                                <PagerSettings Position="Bottom" Mode="NextPreviousFirstLast"
                                    PreviousPageText="<img src='imagens/seta-esquerda.png' border='0' title='Página Anterior'/>"
                                    NextPageText="<img src='imagens/seta-direita.png' border='0' title='Próxima Página'/>"
                                    FirstPageText="<img src='imagens/seta-esquerda-ultima.png' border='0' title='Primeira Página'/>"
                                    LastPageText="<img src='imagens/seta-direita-ultima.png' border='0' title='Última Página'/>" PageButtonCount="15" />--%>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
        </div>
    </div>
</asp:Content>
