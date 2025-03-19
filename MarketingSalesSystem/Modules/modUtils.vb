Imports DevExpress.XtraTab
Imports DevExpress.XtraGrid.Views.Grid

Module modUtils
    Function AddTab(ByRef tabControl As XtraTabControl, ByVal tabTitle As String) As XtraTabPage
        Dim newTab As New XtraTabPage() With {
            .Text = tabTitle
        }
        tabControl.TabPages.Add(newTab)

        Return newTab
    End Function

    Public Sub gridTransMode(ByRef grid As GridView, Optional editable As Boolean = False)
        Try
            grid.BestFitColumns()
            grid.OptionsBehavior.Editable = editable
            grid.OptionsSelection.EnableAppearanceFocusedRow = True
            grid.Columns(0).Visible = False
            grid.OptionsCustomization.AllowColumnMoving = False
        Catch ex As Exception

        End Try
    End Sub

End Module
