
Partial Class _Default
    Inherits Page


    Protected Sub btnEmpresa_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/Empresa.aspx")
    End Sub

    Protected Sub btnAssociado_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/Associado.aspx")
    End Sub
End Class