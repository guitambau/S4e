
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Ajax.Utilities

Partial Class _Assosiado
    Inherits Page
    Private associadoManager As New Associado()
    Private funcGlobal As New FuncoesGlobais()
    Private empresaAssociado As New EmpresaAssociado()
    Private empresaManager As New Empresa()

    Private ReadOnly Property EmpresasSelecionadas As List(Of Empresa)
        Get
            If Session("EmpresasSelecionadas") Is Nothing Then
                Session("EmpresasSelecionadas") = New List(Of Empresa)()
            End If
            Return DirectCast(Session("EmpresasSelecionadas"), List(Of Empresa))
        End Get
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CarregaEmpersaDDl()
            Session("EmpresasSelecionadas") = New List(Of Empresa)()
        End If
    End Sub

    ' Chamado quando o botão de adicionar empresa é clicado
    Protected Sub btnAdicionarEmpresa_Click(sender As Object, e As EventArgs) Handles btnAdicionarEmpresa.Click
        Dim empresaId As Integer = Convert.ToInt32(ddlEmpresas.SelectedValue)
        Dim empresaNome As String = ddlEmpresas.SelectedItem.Text
        Dim empresasSelecionadas As List(Of Empresa) = DirectCast(Session("EmpresasSelecionadas"), List(Of Empresa))

        Dim exists = empresasSelecionadas.Any(Function(emp) emp.EmpresaID = empresaId)
        If Not exists Then
            empresasSelecionadas.Add(New Empresa With {.EmpresaID = empresaId, .NomeEmpresa = empresaNome})
        End If

        Session("EmpresasSelecionadas") = empresasSelecionadas
        gvEmpresasSelecionadas.DataSource = empresasSelecionadas
        gvEmpresasSelecionadas.DataBind()
    End Sub

    ' Evento para remover uma empresa da GridView
    Protected Sub gvEmpresasSelecionadas_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvEmpresasSelecionadas.RowDeleting

        Dim empresaId As Integer = EmpresasSelecionadas(e.RowIndex).EmpresaID
        empresaAssociado.ExcluirRelacionamentoAssociadoEmpresa(Session("AssociadoId").ToString, empresaId)
        ' Remover a empresa selecionada da lista em memória
        EmpresasSelecionadas.RemoveAt(e.RowIndex)

        ' Atualizar o GridView
        gvEmpresasSelecionadas.DataSource = EmpresasSelecionadas
        gvEmpresasSelecionadas.DataBind()
    End Sub


    Protected Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Dim dataNascimento As String = txtDataNascimento.Text
        Dim cpf = txtCPF.Text
        Dim nome = txtNome.Text

        If cpf.Length = 0 Then
            ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "showMessage('CPF é obrigatorio!');", True)
            Exit Sub
        End If
        If nome.Length = 0 Then
            ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "showMessage('Nome é obrigatorio!');", True)
            Exit Sub
        End If

        If gvEmpresasSelecionadas.Rows.Count = 0 Then
            ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "showMessage('Obrigatorio uma empresa pelo menos ao associado!');", True)
            Exit Sub
        End If

        If FuncoesGlobais.isCPF(cpf) Then
            Try
                If Session("AssociadoId") IsNot Nothing Then
                    associadoManager.AtualizarAssociado(Session("AssociadoId").ToString, txtNome.Text, cpf, DateTime.Parse(dataNascimento))
                Else
                    Session("AssociadoId") = associadoManager.AdicionarAssociado(txtNome.Text, cpf, DateTime.Parse(dataNascimento))
                End If

                For Each row As GridViewRow In gvEmpresasSelecionadas.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        Dim empresaId As Integer = Convert.ToInt32(row.Cells(0).Text)
                        empresaAssociado.adicionaRelacionamento(Session("AssociadoId").ToString, empresaId)
                    End If
                Next
                clearCampos()
                ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "showMessage('Adicionado com Sucesso!');", True)
                Exit Sub
            Catch ex As Exception
                ClientScript.RegisterStartupScript(Me.GetType(), "Popup", $"showMessage('Erro ao adicionar associado: {ex.Message}');", True)
                Exit Sub
            End Try
        End If
        ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "showMessage('CPF Invalido!');", True)

    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim termoBusca As String = txtBusca.Text
        Dim tipoBusca As String = ddlTipoBusca.SelectedValue
        Dim resultados As DataTable

        Select Case tipoBusca
            Case "CPF"
                resultados = associadoManager.BuscarAssociado(cpf:=termoBusca)
            Case "Nome"
                resultados = associadoManager.BuscarAssociado(nome:=termoBusca)
            Case "DataNascimento"
                resultados = associadoManager.BuscarAssociado(dataNascimento:=termoBusca)
        End Select

        gvResultadosBusca.DataSource = resultados
        gvResultadosBusca.DataBind()
        gvResultadosBusca.Visible = True
    End Sub

    Protected Sub btnCadastrar_Click(sender As Object, e As EventArgs) Handles btnCadastrar.Click
        pnlCadastro.Visible = True
        pnlBusca.Visible = False
    End Sub

    Protected Sub gvEmpresasSelecionadas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvEmpresasSelecionadas.SelectedIndexChanged
        'Deletar empresa 
    End Sub

    Protected Sub gvResultadosBusca_RowEditing(sender As Object, e As GridViewEditEventArgs)
        Dim index As Integer = e.NewEditIndex
        Dim row As GridViewRow = gvResultadosBusca.Rows(index)
        Session("AssociadoId") = associadoManager.BuscarAssociadoIdPorCpf(row.Cells(2).Text)
        txtNome.Text = row.Cells(1).Text
        txtCPF.Text = row.Cells(2).Text
        Dim lblDataNascimento As Label = DirectCast(row.FindControl("lblDataNascimento"), Label)
        Dim dataNascimentoTexto As String = lblDataNascimento.Text

        If Not String.IsNullOrEmpty(dataNascimentoTexto) Then
            Dim dataNascimento As DateTime
            If DateTime.TryParseExact(dataNascimentoTexto, "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, dataNascimento) Then
                txtDataNascimento.Text = dataNascimento.ToString("yyyy-MM-dd")
            End If
        End If

        Dim empresasSelecionadas As List(Of Empresa) = DirectCast(Session("EmpresasSelecionadas"), List(Of Empresa))


        Dim empresasRelacionadas As DataTable = associadoManager.BuscaEmpresasRelacionadas(row.Cells(2).Text)
        Dim listaEmpresas As List(Of Empresa) = ConvertDataTableToList(empresasRelacionadas)

        Session("EmpresasSelecionadas") = listaEmpresas

        gvEmpresasSelecionadas.DataSource = empresasRelacionadas
        gvEmpresasSelecionadas.DataBind()
        pnlCadastro.Visible = True
        pnlBusca.Visible = False


        gvResultadosBusca.EditIndex = -1
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        pnlBusca.Visible = True
        pnlCadastro.Visible = False
        gvResultadosBusca.Visible = False
        Response.Redirect("Associado.aspx")
    End Sub

    Private Sub CarregaEmpersaDDl()
        Dim empresas As DataTable = empresaManager.BuscarEmpresa()
        ddlEmpresas.DataSource = empresas
        ddlEmpresas.DataTextField = "nome"
        ddlEmpresas.DataValueField = "id"
        ddlEmpresas.DataBind()
    End Sub

    Private Function ConvertDataTableToList(dt As DataTable) As List(Of Empresa)
        Dim empresas As New List(Of Empresa)()

        For Each row As DataRow In dt.Rows
            Dim empresa As New Empresa() With {
                .EmpresaID = Convert.ToInt32(row("EmpresaID")),
                .NomeEmpresa = row("NomeEmpresa").ToString()
        }
            empresas.Add(empresa)
        Next
        Return empresas
    End Function

    Private Sub clearCampos()
        txtCPF.Text = ""
        txtDataNascimento.Text = ""
        txtNome.Text = ""
        pnlCadastro.Visible = False
        pnlBusca.Visible = True
        gvResultadosBusca.Visible = False
    End Sub

End Class