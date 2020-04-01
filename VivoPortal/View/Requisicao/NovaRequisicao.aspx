<%@ Page Title="" Language="C#" MasterPageFile="~/View/Portal.Master" AutoEventWireup="true" CodeBehind="NovaRequisicao.aspx.cs" Inherits="IndraPortal.View.Requisicao.NovaRequisicao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlace" runat="server">
    <div class="">
        <div class="header-page">
            <h1>Cálculo de Multas - Nova Requisição</h1>
        </div>
        <div class="page">
            <div class="jumbotron pt-4">
                <div class="row">
                    <div class="row btns-pd">
                        
                            <div class="row-fluid">
                                <div class="file is-boxed float-left">
                                    <label class="file-label">
                                        <button type="button" onclick="showModalAdd();"></button>
                                        <span class="file-cta span-icon">
                                            <span class="file-icon icon-pf">
                                                <i class="fas fa-user-plus"></i>
                                            </span>
                                            <span class="file-label text-center">Novo registro
                                            </span>
                                        </span>
                                    </label>
                                </div>
                            </div>
                       
                        
                            <div class="row-fluid">
                                <div class="file is-boxed float-right">
                                    <label class="file-label">
                                        <input class="file-input" type="file" name="resume" id="myFile" runat="server" onchange="fakeupload.value = this.value;" onclick ="verificaGrid();" />
                                        <span class="file-cta span-icon file-btn">
                                            <span class="file-icon icon-pf">
                                                <i class="fas fa-upload"></i>
                                            </span>
                                            <span class="file-label text-center">Carregar Planilha</span>
                                        </span>
                                        <input id="fakeupload﻿" name="fakeupload" class="file-name-btn" type="text" readonly="true" value="" />
                                    </label>
                                </div>
                            </div>
                        
                    </div>
                </div>
                <div class="row moldura mt-3">
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
                    <div class="col-sm-12 col-md-12 col-lg-12 mt-3">
                        <div id="gridDetalheReq" class="table-editable">
                            <asp:GridView CssClass="table table-striped table-hover table-sm" ID="gridDt" ShowHeaderWhenEmpty="True" runat="server"
                                HeaderStyle-BackColor="#003366" HeaderStyle-ForeColor="#ffffff" HeaderStyle-HorizontalAlign="Center"
                                RowStyle-HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="gridDt_PageIndexChanging" OnRowDeleting="gridDt_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Excluir">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnexcluir" runat="server" Text="Excluir Registro" OnClientClick="return confirm('Deseja excluir o registro ?')" CommandName="delete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle HorizontalAlign="Center" BackColor="#003366" ForeColor="White"></HeaderStyle>

                                <RowStyle HorizontalAlign="Center"></RowStyle>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-12 col-lg-12 mt-3">
                        <span class="txt-alert">Atenção: Apenas 1 N.Conta por requisição</span>
                    </div>
                </div>


                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12 pr-0">
                        <asp:Button runat="server" ID="btnEnviar" class="btn btn-primary btn-lg float-right" Text="Enviar" OnClick="btnEnviar_Click" />
                        <asp:LinkButton ID="LnkButton" runat="server" OnClick="LnkButton_Click">Modelo.xlsx</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <%-------------------------------------------------------------------------------------------------------------------%>
        <!-- Modal -->
        <div class="modal fade" id="mdAdd" tabindex="-1" role="dialog" aria-labelledby="mdAddLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Adicionar novo registro</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <div class="form-group">
                                    <asp:Label ID="lblTipo" runat="server" Text="Tipo"></asp:Label>
                                    <asp:DropDownList runat="server" CssClass="form-control focus-modal" ID="ddCalcMultas">
                                        <asp:ListItem Text="Selecione uma opção" Value="Selecione_Opcao" />
                                        <asp:ListItem Text="Número Conta" Value="Conta" />
                                        <asp:ListItem Text="Número Linha" Value="Linha" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <div class="form-group">
                                    <asp:Label ID="lblNrRegistro" runat="server" Text="Registro:"></asp:Label>
                                    <asp:TextBox ID="txtNrRegistro" runat="server" onkeypress="return runScript(event)" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <div class="form-group">
                                    <asp:Label ID="lblPlanoDestino" runat="server" Text="Plano Destino:"></asp:Label>
                                    <asp:TextBox ID="txtPlanoDestino" runat="server"  CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <div class="form-group">
                                    <asp:Label ID="lblPacoteDestino" runat="server" Text="Pacote Destino:"></asp:Label>
                                    <asp:TextBox ID="txtPacoteDestino" runat="server"  CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <div class="form-group">
                                    <asp:Label ID="lblMultaDesconto" runat="server" Text="Multa Desconto:"></asp:Label>
                                    <asp:TextBox ID="txtMultaDesconto" runat="server"   CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSalvar" CssClass="btn btn-default" class="btn btn-primary" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        
        <%--------------%>
        <div class="modal fade" id="mdAlert" tabindex="-1" role="dialog" aria-labelledby="mdAddLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title title-modal">Requisição Aberta com Sucesso!</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <div class="form-group">
                                    <label class="txt-modal">Número da Requisição:</label>
                                    <asp:Label ID="lblNRequisicao" runat="server" Text="" CssClass="txt-modal-nr"></asp:Label>
                                    <%--         <button class="btn-copy float-right mt-1" onclick="copyText();">
                                        <span class="icon-fa-copy">
                                            <i class="fas fa-copy"></i>
                                        </span>
                                    </button>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal" id="btnCloseModal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function showModalAdd() {
            document.querySelector("[name='fakeupload']").value = "";
            //$('#contentPlace_myFile').attr('disabled', 'disabled');
            $('#mdAdd').modal('show');
            $('#mdAdd').focus();
        }

        function showModal() {
            $('#mdAlert').modal('show');
        }

        //Iniciliza a pag com o foco no btn Enviar
            $(document).ready(function () {
            $.maskWatchers = {};
            $(document).keypress(function (e) {
                if (e.which == 13) {
                    if (isVisible = $("#mdAdd").is(":visible")) {
                       

                        var clickButton = document.getElementById("<%= btnSalvar.ClientID %>");
                        clickButton.click();

                        $('#btnSalvar').click();
                        
                        //alert("Enter is pressed Anderson");
                    }
                    else if (isVisible = $("#mdAlert").is(":visible")) {
                        $('#btnCloseModal').click();
                        //alert("fechar");
                    }
                    else {
                        $('#contentPlace_btnEnviar').click();
                        //alert("Enviar");
                    }
                }
            });
        });

        $('#contentPlace_ddCalcMultas').change(function () {
            $("#contentPlace_txtNrRegistro").val('');
        });

        $("#contentPlace_txtNrRegistro").keypress(function () {
            var item = $("#contentPlace_ddCalcMultas option:selected").val();
            var elem = $('#contentPlace_txtNrRegistro');

            if (item === 'Linha') {
                elem.mask("(99) 99999-9999").focusout(function (event) {
                    var target, phone, element;
                    target = (event.currentTarget) ? event.currentTarget : event.srcElement;
                    phone = target.value.replace(/\D/g, '');
                    element = $(target);
                    element.unmask();
                    if (phone.length > 10) {
                        element.mask("(99) 99999-9999");
                    } else {
                        element.mask("(99) 9999-99999");
                    }
                });
            }
            else if (item === 'CNPJ') {
                elem.mask("99.999.999/9999-99");
            } else {
                elem.mask('0000000000');
            }
        });

        function runScript(e) {
        if (e.keyCode == 13) {
            var clickButton = document.getElementById("<%= btnSalvar.ClientID  %>");
            clickButton.click();
            
        }
        }

        function verificaGrid() {
            var qntTr = $('#contentPlace_gridDt tr').length;

            if (qntTr > 1) {
                document.querySelector("[name='fakeupload']").value = "";
                alert('Ja existe uma requisição em andamento, não é possivel importar arquivo.');
                $('#contentPlace_myFile').attr('disabled', 'disabled');
                return false;
            }
           
        };


      

        
        
    

    </script>
</asp:Content>
