Imports DevExpress.XtraGrid.Views.Grid
Public Class ucSalesInvc
    Inherits ucBase

    Sub New(ByVal title As String)
        InitializeComponent()

        MyBase.title = title
        LabelControl1.Text = title

        GridView1.OptionsView.ShowFooter = True
        GridView2.OptionsView.ShowFooter = True

    End Sub

End Class
