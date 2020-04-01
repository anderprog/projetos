<%@ Page Title="" Language="C#" MasterPageFile="~/View/Portal.Master" AutoEventWireup="true" CodeBehind="SucessoRequisicao.aspx.cs" Inherits="IndraPortal.View.Requisicao.SucessoRequisicao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlace" runat="server">
    <div class="">
        <div class="header-page">
            <h1>Cálculo de Multas - Nova Requisição</h1>
        </div>
        <div class="page">
            <div class="jumbotron">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <asp:Label ID="Label1" runat="server" Text="Requisição aberta com sucesso !" Width="800px" Font-Size="X-Large" ForeColor="#3333CC"></asp:Label>
                    </div>

                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <asp:Label ID="lblReq" runat="server" Text="Label" Width="800px" Font-Size="X-Large" ForeColor="#3333CC"></asp:Label>
                    </div>
              
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 mt-5">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>