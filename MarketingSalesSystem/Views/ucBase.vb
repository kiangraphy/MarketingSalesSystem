Public Class ucBase
    Public title As String

    Sub New()
        InitializeComponent()
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
End Class
