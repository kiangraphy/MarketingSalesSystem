Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraEditors
Imports System.Text

Public Class frm_catchActivity

    Public dt As DataTable

    Private ctrlCA As ctrlCatchers

    Sub New(ByRef ctrl As ctrlCatchers)
        InitializeComponent()

        ctrlCA = ctrl

        GridView1.OptionsSelection.MultiSelect = True
        GridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect

        dtCreated.Properties.MaxValue = Date.Now

        txtLat.Properties.Mask.MaskType = Mask.MaskType.Numeric
        txtLat.Properties.Mask.EditMask = "000.000000"
        txtLat.Properties.Mask.UseMaskAsDisplayFormat = True

        txtLong.Properties.Mask.MaskType = Mask.MaskType.Numeric
        txtLong.Properties.Mask.EditMask = "000.000000"
        txtLong.Properties.Mask.UseMaskAsDisplayFormat = True
    End Sub


    Private Sub GridControl1_Load(sender As Object, e As EventArgs) Handles GridControl1.Load
        Dim lookupEdit As New RepositoryItemLookUpEdit()

        Dim dc = New tpmdbDataContext

        Dim vessels = (From i In dc.ml_Vessels Where i.ml_vtID = 1 Select i.ml_vID, i.vesselName).ToList

        lookupEdit.DataSource = vessels
        lookupEdit.DisplayMember = "vesselName"
        lookupEdit.ValueMember = "ml_vID"

        Dim gridColumn As DevExpress.XtraGrid.Columns.GridColumn = GridView1.Columns("Catcher")
        gridColumn.ColumnEdit = lookupEdit
        lookUpTransMode(vessels, lookupEdit, "vesselName", "ml_vID", "Select catcher")

        GridView1.Columns("Catcher").OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False

    End Sub

    Private Sub btnAddCatcher_Click(sender As Object, e As EventArgs) Handles btnAddCatcher.Click
        ctrlCA.addCatcher()
    End Sub

    Private Sub btnSave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnSave.ItemClick
        Dim dateCreated = validateField(dtCreated)
        Dim catchMethod = validateField(cmbMethod)
        Dim longitude = validateField(txtLat)
        Dim latitude = validateField(txtLong)

        Dim missingFields = New StringBuilder()
        If Not dateCreated Then missingFields.AppendLine("Date")
        If Not catchMethod Then missingFields.AppendLine("Method of Catching")
        If Not longitude Then missingFields.AppendLine("Longitude")
        If Not latitude Then missingFields.AppendLine("Latitude")

        Dim countEmpty = 0
        For i As Integer = 0 To GridView1.RowCount - 1
            Dim row As DataRow = GridView1.GetDataRow(i)

            If String.IsNullOrWhiteSpace(row("Catcher").ToString()) Then
                countEmpty = countEmpty + 1
            End If
        Next

        If countEmpty > 0 Then
            missingFields.AppendLine(countEmpty & " Catcher is Empty!")
        End If

        If missingFields.Length > 0 Then
            requiredMessage(missingFields.ToString())
            Return
        End If

        ctrlCA.save()

    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If GridView1.SelectedRowsCount < 1 Then
            XtraMessageBox.Show("Please select row first!", APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim confirmation = XtraMessageBox.Show("Are you sure do you want to delete the selected rows?", APPNAME, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If confirmation = Windows.Forms.DialogResult.Yes Then
            Dim selectedRows As Integer() = GridView1.GetSelectedRows()

            For i As Integer = selectedRows.Length - 1 To 0 Step -1
                Dim rowHandle As Integer = selectedRows(i)
                Dim row As DataRow = GridView1.GetDataRow(rowHandle)

                If row IsNot Nothing Then dt.Rows.Remove(row)
            Next

            GridControl1.DataSource = dt
            GridView1.RefreshData()
            GridView1.ClearSelection()
        End If
    End Sub

End Class