
Imports System.Data
Imports ASP

Partial Class _Empresa
    Inherits Page
    ' Lista em memória para armazenar os associados vinculados à empresa
    Private associadoManager As New Associado()
    Private empresManager As New Empresa()
    Private empresaAssociado As New EmpresaAssociado()
    Private ReadOnly Property AssociadosSelecionados As List(Of Associado)
        Get
            If Session("AssociadosSelecionados") Is Nothing Then
                Session("AssociadosSelecionados") = New List(Of Associado)()
            End If
            Return DirectCast(ViewState("AssociadosSelecionados"), List(Of Associado))
        End Get
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CarregaAssociadoDDL()
            Session("EmpresaId") = Nothing
            Session("AssociadosSelecionados") = New List(Of Associado)()
        End If
    End Sub

    Protected Sub btnAdicionarAssociado_Click(sender As Object, e As EventArgs) Handles btnAdicionarAssociado.Click
        Dim associadoId As Integer = Convert.ToInt32(ddlAssociados.SelectedValue)
        Dim nome As String = ddlAssociados.SelectedItem.Text
        Dim associadoSelecionado As List(Of Associado) = DirectCast(Session("AssociadosSelecionados"), List(Of Associado))

        Dim exists = associadoSelecionado.Any(Function(assoc) assoc.AssociadoID = associadoId)
        If Not exists Then
            associadoSelecionado.Add(New Associado With {.AssociadoID = associadoId, .NomeAssociado = nome})
        End If

        Session("AssociadosSelecionados") = associadoSelecionado
        gvAssociadosSelecionados.DataSource = associadoSelecionado
        gvAssociadosSelecionados.DataBind()
    End Sub

    Protected Sub gvAssociadosSelecionados_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvAssociadosSelecionados.RowDeleting
        Dim associadoId As Integer = AssociadosSelecionados(e.RowIndex).AssociadoID
        empresaAssociado.ExcluirRelacionamentoAssociadoEmpresa(associadoId, Session("EmpresaId").ToString)
        AssociadosSelecionados.RemoveAt(e.RowIndex)

        ' Atualizar o GridView
        gvAssociadosSelecionados.DataSource = AssociadosSelecionados
        gvAssociadosSelecionados.DataBind()
    End Sub

    Protected Sub btnSalvarEmpresa_Click(sender As Object, e As EventArgs) Handles btnSalvarEmpresa.Click
        Dim cnpj = txtCNPJ.Text
        Dim nome = txtNomeEmpresa.Text
        If cnpj.Length = 0 Then
            ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "showMessage('Cnpj é obrigatorio!');", True)
            Exit Sub
        End If
        If nome.Length = 0 Then
            ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "showMessage('Nome é obrigatorio!');", True)
            Exit Sub
        End If
        If gvAssociadosSelecionados.Rows.Count = 0 Then
            ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "showMessage('Obrigatorio um associado pelo menos a empresa!');", True)
            Exit Sub
        End If
        If FuncoesGlobais.isCNPJ(txtCNPJ.Text) Then
            Try
                If Session("EmpresaId") IsNot Nothing Then
                    empresManager.AtualizarEmpresa(Session("EmpresaId").ToString, txtNomeEmpresa.Text, txtCNPJ.Text)
                Else
                    Session("EmpresaId") = empresManager.AdicionarEmpresa(txtNomeEmpresa.Text, txtCNPJ.Text)
                End If

                For Each row As GridViewRow In gvAssociadosSelecionados.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        Dim associadosId As Integer = Convert.ToInt32(row.Cells(0).Text)
                        empresaAssociado.adicionaRelacionamento(associadosId, Session("EmpresaId").ToString)
                    End If
                Next
                ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "showMessage('Adicionado com Sucesso!');", True)
                Exit Sub
            Catch ex As Exception
                ClientScript.RegisterStartupScript(Me.GetType(), "Popup", $"showMessage('Erro ao adicionar empresa: {ex.Message}');", True)
                Exit Sub
            End Try
        End If
        ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "showMessage('CNPJ Invalido!');", True)
    End Sub


    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim termoBusca As String = txtBusca.Text
        Dim tipoBusca As String = ddlTipoBusca.SelectedValue
        Dim resultados As DataTable

        Select Case tipoBusca
            Case "CNPJ"
                resultados = empresManager.BuscarEmpresa(cnpj:=termoBusca)
            Case "Nome"
                resultados = empresManager.BuscarEmpresa(nome:=termoBusca)
        End Select

        gvResultadosBusca.DataSource = resultados
        gvResultadosBusca.DataBind()
        gvResultadosBusca.Visible = True
    End Sub

    Protected Sub btnCadastrar_Click(sender As Object, e As EventArgs) Handles btnCadastrar.Click
        pnlCadastro.Visible = True
        pnlBusca.Visible = False
    End Sub

    Protected Sub gvResultadosBusca_RowEditing(sender As Object, e As GridViewEditEventArgs)
        Dim index As Integer = e.NewEditIndex
        Dim row As GridViewRow = gvResultadosBusca.Rows(index)

        Session("EmpresaId") = empresManager.BuscarEmpresaIdPorCnpj(row.Cells(2).Text)
        txtNomeEmpresa.Text = row.Cells(1).Text
        txtCNPJ.Text = row.Cells(2).Text

        Dim empresasSelecionadas As List(Of Associado) = DirectCast(Session("EmpresasSelecionadas"), List(Of Associado))


        Dim associadoRelacionado As DataTable = empresManager.BuscaAssociadoRelacionadas(row.Cells(2).Text)
        Dim listaAssociados As List(Of Associado) = ConvertDataTableToList(associadoRelacionado)

        Session("AssociadosSelecionados") = listaAssociados

        gvAssociadosSelecionados.DataSource = associadoRelacionado
        gvAssociadosSelecionados.DataBind()

        pnlCadastro.Visible = True
        pnlBusca.Visible = False

        gvResultadosBusca.EditIndex = -1
    End Sub

    Protected Sub btnCancelarEmpresa_Click(sender As Object, e As EventArgs) Handles btnCancelarEmpresa.Click
        pnlBusca.Visible = True
        pnlCadastro.Visible = False
        Response.Redirect("Empresa.aspx")
    End Sub

    Private Function ConvertDataTableToList(dt As DataTable) As List(Of Associado)
        Dim associados As New List(Of Associado)()

        For Each row As DataRow In dt.Rows
            Dim associado As New Associado() With {
                .AssociadoID = Convert.ToInt32(row("AssociadoID")),
                .NomeAssociado = row("NomeAssociado").ToString()
        }
            associados.Add(associado)
        Next

        Return associados
    End Function
    Private Sub CarregaAssociadoDDL()
        Dim associados As DataTable = associadoManager.BuscarAssociado()
        ddlAssociados.DataSource = associados
        ddlAssociados.DataTextField = "nome" ' O campo da tabela que será exibido no DropDownList
        ddlAssociados.DataValueField = "id" ' O campo da tabela que será o valor de cada item
        ddlAssociados.DataBind()
    End Sub


End Class