Imports DevExpress.XtraGrid
Imports DevExpress.XtraPrinting

Public Class ucBase
    Public title As String

    Sub New()
        InitializeComponent()

        Dim dateNow = getServerDate()
        dtFrom.EditValue = dateNow.AddDays(-30)
        dtTo.EditValue = dateNow
    End Sub

    Sub hideDate()
        LayoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        LayoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        LayoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
    End Sub


    Private Sub XtraTabControl1_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs)
        Debug.WriteLine("Switched to: " & e.Page.Text)
    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        Dim ctrl = New ctrlSales()
    End Sub

    Protected Overridable Function GetGridControl() As GridControl
        Return Nothing
    End Function

    Private Sub btn_print_Click(sender As Object, e As EventArgs) Handles btn_print.Click
        Dim activeGrid As GridControl = GetGridControl()

        If activeGrid IsNot Nothing Then
            Dim printSystem As New PrintingSystem()
            Dim printLink As New PrintableComponentLink(printSystem) With {
                .Component = activeGrid,
                .PaperKind = System.Drawing.Printing.PaperKind.A4,
                .Landscape = True
            }

            printLink.CreateDocument()
            printLink.ShowPreviewDialog()
        Else
            MessageBox.Show("No GridControl found. Make sure the data has been loaded correctly.", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub btn_filter_Click(sender As Object, e As EventArgs) Handles btn_filter.Click
        refreshData()
    End Sub

    Overridable Sub refreshData()

    End Sub
End Class
