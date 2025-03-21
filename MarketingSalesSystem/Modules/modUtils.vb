Imports DevExpress.XtraTab
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraEditors

Module modUtils
    Function getServerDate() As Date
        Return Date.Now
    End Function

    Function AddTab(ByRef tabControl As XtraTabControl, ByVal tabTitle As String) As XtraTabPage
        Dim newTab As New XtraTabPage() With {
            .Text = tabTitle
        }
        tabControl.TabPages.Add(newTab)

        Return newTab
    End Function

    Sub gridTransMode(ByRef grid As GridView, Optional editable As Boolean = False)
        Try
            grid.BestFitColumns()
            grid.OptionsBehavior.Editable = editable
            grid.OptionsSelection.EnableAppearanceFocusedRow = True
            grid.Columns(0).Visible = False
            grid.OptionsCustomization.AllowColumnMoving = True
        Catch ex As Exception

        End Try
    End Sub

    Function validateField(ByRef control As DateEdit) As Boolean
        Return (Not control.EditValue Is Nothing)
    End Function

    Function validateField(ByRef control As TextEdit) As Boolean
        Return (Not control.EditValue Is Nothing)
    End Function

    Function validateField(ByRef control As LookUpEdit) As Boolean
        Return (Not control.EditValue Is Nothing)
    End Function

    Function validateField(ByRef control As ComboBoxEdit) As Boolean
        Return (Not control.EditValue Is Nothing)
    End Function

    Sub requiredMessage(ByVal fields As String)
        XtraMessageBox.Show("Required Fields: " + vbNewLine + fields, APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub
    
    Public Sub gvCount(ByRef gridview As GridView)
        If gridview.RowCount > 0 Then
            Dim col = gridview.Columns(1)
            col.Summary.Add(DevExpress.Data.SummaryItemType.Count, col.FieldName, "Count:{0}")
        End If
    End Sub

End Module
