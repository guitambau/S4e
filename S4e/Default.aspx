<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Cadastro de Empresas e Associados</title>
</head>
<body>
    
    <div>
        <h2>Cadastro de Empresas e Associados</h2>
         <asp:Button ID="btnEmpresa" runat="server" Text="Cadastrar Empresa" OnClick="btnEmpresa_Click" />
        <asp:Button ID="btnAssociado" runat="server" Text="Cadastrar Associado" OnClick="btnAssociado_Click" />
    </div>
    
</body>
</html>

</asp:Content>
