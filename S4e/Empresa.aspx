<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Empresa.aspx.vb" Inherits="_Empresa" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function showMessage(message) {
            var modal = document.getElementById('messageModal');
            var text = document.getElementById('messageText');
            text.innerHTML = message;
            modal.style.display = 'block';
        }
    </script>
    <!DOCTYPE html>
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title>Cadastro de Empresas</title>
    </head>
    <body>
        <div>
            <h1>Cadastro de Empresa</h1>
        </div>
        <asp:Panel ID="pnlBusca" runat="server" Visible="True">
            <div>
                <label for="ddlTipoBusca">Tipo de Busca:</label>
                <asp:DropDownList ID="ddlTipoBusca" runat="server">
                    <asp:ListItem Text="CNPJ" Value="CNPJ"></asp:ListItem>
                    <asp:ListItem Text="Nome" Value="Nome"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <!-- Campo de Busca e Botão de Buscar -->
            <div>
                <label for="txtBusca">Buscar Associado:</label>
                <asp:TextBox ID="txtBusca" runat="server"></asp:TextBox>
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                <asp:Button ID="btnCadastrar" runat="server" Text="Cadastrar" />
            </div>

            <!-- GridView para Mostrar Resultados da Busca -->
            <div>
                <asp:GridView ID="gvResultadosBusca" runat="server" AutoGenerateColumns="False" OnRowEditing="gvResultadosBusca_RowEditing">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="id" Visible="false" />
                        <asp:BoundField DataField="nome" HeaderText="Nome" />
                        <asp:BoundField DataField="cnpj" HeaderText="cnpj" />
                        <asp:CommandField ShowEditButton="True" EditText="Editar" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlCadastro" runat="server" Visible="False">
            <div>
                <label for="txtNomeEmpresa">Nome da Empresa:</label>
                <asp:TextBox ID="txtNomeEmpresa" runat="server" MaxLength="200" Required="true"></asp:TextBox>
            </div>
            <div>
                <label for="txtCNPJ">CNPJ:</label>
                <asp:TextBox ID="txtCNPJ" runat="server" MaxLength="14" Required="true"></asp:TextBox>
            </div>

            <!-- Aqui você pode inserir um controle para selecionar associados -->
            <div>
                <label for="lstAssociados">Associados:</label>
                <asp:DropDownList ID="ddlAssociados" runat="server"></asp:DropDownList>
                <!-- Botão para adicionar associados -->
                <asp:Button ID="btnAdicionarAssociado" runat="server" Text="Adicionar Associado" />
            </div>

            <!-- GridView para mostrar os associados selecionados -->
            <div>
                <asp:GridView ID="gvAssociadosSelecionados" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="AssociadoID" HeaderText="N°"/>
                        <asp:BoundField DataField="NomeAssociado" HeaderText="Associado" />
                        <asp:CommandField ShowDeleteButton="True" DeleteText="Remover" />
                    </Columns>
                </asp:GridView>
            </div>

            <!-- Botões de ação -->
            <div>
                <asp:Button ID="btnSalvarEmpresa" runat="server" Text="Salvar" OnClick="btnSalvarEmpresa_Click" />
                <asp:Button ID="btnCancelarEmpresa" runat="server" Text="Cancelar" />
            </div>
        </asp:Panel>

        <div id="messageModal" style="display: none; position: fixed; z-index: 100; top: 50%; left: 50%; transform: translate(-50%, -50%); background-color: white; padding: 20px; border: 1px solid black;">
            <span id="messageText"></span>
            <br />
            <button onclick="document.getElementById('messageModal').style.display='none'">Fechar</button>
        </div>
    </body>
    </html>

</asp:Content>
