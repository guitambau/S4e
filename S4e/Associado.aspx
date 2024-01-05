<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Associado.aspx.vb" Inherits="_Assosiado" %>

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
    <html>
    <head>
        <title>Associados</title>

    </head>
    <body>
        <h1>Associados</h1>
        <asp:Panel ID="pnlBusca" runat="server" Visible="True">

            <div>
                <label for="ddlTipoBusca">Tipo de Busca:</label>
                <asp:DropDownList ID="ddlTipoBusca" runat="server">
                    <asp:ListItem Text="CPF" Value="CPF"></asp:ListItem>
                    <asp:ListItem Text="Nome" Value="Nome"></asp:ListItem>
                    <asp:ListItem Text="Data de Nascimento" Value="DataNascimento"></asp:ListItem>
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
                        <asp:BoundField DataField="cpf" HeaderText="cpf" />
                        <asp:TemplateField HeaderText="Data Nascimento">
                            <ItemTemplate>
                                <asp:Label ID="lblDataNascimento" runat="server" Text='<%# Bind("data_nascimento", "{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" EditText="Editar" />
                    </Columns>

                </asp:GridView>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlCadastro" runat="server" Visible="False">
            <!-- Parte do cadastro -->
            <div>
                <label for="txtNome">Nome:</label>
                <asp:TextBox ID="txtNome" runat="server" MaxLength="200" ></asp:TextBox>
            </div>
            <div>
                <label for="txtCPF">CPF:</label>
                <asp:TextBox ID="txtCPF" runat="server" MaxLength="11" ></asp:TextBox>
            </div>
            <div>
                <label for="txtDataNascimento">Data de Nascimento:</label>
                <asp:TextBox ID="txtDataNascimento" runat="server" TextMode="Date"></asp:TextBox>
            </div>
            <div>
                <label for="ddlEmpresas">Empresas:</label>
                <asp:DropDownList ID="ddlEmpresas" runat="server"></asp:DropDownList>
                <asp:Button ID="btnAdicionarEmpresa" runat="server" Text="Adicionar" OnClick="btnAdicionarEmpresa_Click" />
            </div>

            <!-- GridView para mostrar as empresas selecionadas -->
            <div>
                <asp:GridView ID="gvEmpresasSelecionadas" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="EmpresaID" HeaderText="N°" />
                        <asp:BoundField DataField="NomeEmpresa" HeaderText="Empresa" />
                        <asp:CommandField ShowDeleteButton="True" DeleteText="Remover" />
                    </Columns>
                </asp:GridView>
            </div>
            <div>
                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
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
