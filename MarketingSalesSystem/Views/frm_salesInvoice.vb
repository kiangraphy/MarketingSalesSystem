Imports DevExpress.XtraTab
Imports DevExpress.XtraLayout
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Public Class frm_salesInvoice

    Private tabControl As XtraTabControl
    Private txtNote As TextBox

    Private ctrlSales As ctrlSales

    Sub New(ByRef ctrlS As ctrlSales)

        InitializeComponent()

        ctrlSales = ctrlS

    End Sub

    Private Sub cmbUVT_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbUVT.SelectedIndexChanged
        ctrlSales.createCollectionVessel(CInt(cmbUVT.SelectedIndex) + 1)
    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If RadioGroup1.SelectedIndex = 1 Then
            ctrlSales.changeBuyerInput()
        Else
            ctrlSales.changeBuyerCombo()
        End If
    End Sub
End Class