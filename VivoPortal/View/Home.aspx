<%@ Page Title="" Language="C#" MasterPageFile="~/View/Portal.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="IndraPortal.View.Sumario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlace" runat="server">
    <div class="">
        <div class="header-page">
            <h1>Sumário</h1>
        </div>
        <div class="page">
            <div class="jumbotron">
                <div class="row">
                    <div class="col-sm-4 col-md-6 col-lg-6 ">
                        <a href="/View/Requisicao/NovaRequisicao.aspx">
                            <div class="card mb-3 card-menu float-right">
                                <div class="card-header">
                                    <h5 class="card-title">Cálculo de multas</h5>
                                    <p class="card-text text-center">Nova Requisição</p>
                                </div>
                                <div class="card-body text-primary flex-centralizado">
                                    <span class="icon-fa2"><i class="fas fa-edit"></i></span>
                                </div>
                                <p class="card-text text-center">ABRIR</p>
                            </div>
                        </a>
                    </div>
                    <div class="col-sm-4 col-md-6 col-lg-6 ">
                        <a href="/View/Requisicao/FalhaRequisicao.aspx">
                        <a href="/View/Consulta/ConsultarRequisicao.aspx">
                            <div class="card mb-3 card-menu float-left">
                                <div class="card-header">
                                    <h5 class="card-title">Cálculo de multas</h5>
                                    <p class="card-text text-center">Resultado</p>
                                </div>
                                <div class="card-body text-primary flex-centralizado">
                                    <span class="icon-fa2"><i class="fas fa-list-ul"></i></span>
                                </div>
                                <p class="card-text text-center">ABRIR</p>
                            </div>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
