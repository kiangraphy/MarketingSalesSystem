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


End Class
