Imports DevExpress.XtraTab
Imports DevExpress.XtraLayout
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors
Imports System.Text

Public Class frm_salesInvoice

    Public dt As DataTable
    Public dts As DataTable
    Private tabControl As XtraTabControl
    Private txtNote As TextBox

    Private ctrlSales As ctrlSales

    Public buyerName As String
    Public checkBuyer As Boolean
    Public buyerID As Integer
    Public isPosted As Boolean = False
    Public rowCount As Integer

    Private hasUnsavedChanges As Boolean = False

    Sub New(ByRef ctrlS As ctrlSales)
        InitializeComponent()
        ctrlSales = ctrlS
    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rBT.SelectedIndexChanged
        If rBT.SelectedIndex = 0 Then
            ctrlSales.changeBuyerInput()
        Else
            ctrlSales.changeBuyerCombo()
        End If
    End Sub


    Private Sub GridControl1_Load(sender As Object, e As EventArgs) Handles GridControl1.Load

    End Sub

    Sub createBands(count As Integer, catcherID As Integer)
        Dim bandClass = AddBand("Class", BandedGridView1)
        Dim bandSize = AddBand("Size", BandedGridView1)
        Dim bandPrice = AddBand("Price", BandedGridView1)
        Dim bandAU = AddBand("Actual Unloading", BandedGridView1)
        Dim bandSpoilage = AddBand("Spoilage", BandedGridView1)
        Dim bandNet = AddBand("Net", BandedGridView1)

        Dim bandAUKilos = AddBand("Kilos", bandAU)
        Dim bandAUAmount = AddBand("Amount", bandAU)
        populateBand("Catcher", bandAUKilos, count, catcherID)
        Dim bandAUKTotal = AddBand("Total", bandAUKilos)
        populateBand("Catcher", bandAUAmount, count, catcherID)
        Dim bandAUATotal = AddBand("Total", bandAUAmount)

        Dim bandSKilos = AddBand("Kilos", bandSpoilage)
        Dim bandSAmount = AddBand("Amount", bandSpoilage)
        populateBand("Catcher", bandSKilos, count, catcherID)
        Dim bandSKTotal = AddBand("Total", bandSKilos)
        populateBand("Catcher", bandSAmount, count, catcherID)
        Dim bandSATotal = AddBand("Total", bandSAmount)

        Dim bandNKilos = AddBand("Kilos", bandNet)
        Dim bandNAmount = AddBand("Amount", bandNet)
        populateBand("Catcher", bandNKilos, count, catcherID)
        Dim bandNKTotal = AddBand("Total", bandNKilos)
        populateBand("Catcher", bandNAmount, count, catcherID)
        Dim bandNATotal = AddBand("Total", bandNAmount)

        With BandedGridView1

            .PopulateColumns()

            .Columns("Class").OwnerBand = bandClass
            .Columns("Size").OwnerBand = bandSize
            .Columns("Price").OwnerBand = bandPrice
            setOwnerBand("AUK_Catcher", bandAUKilos, False, True)
            .Columns("AUK_Total").OwnerBand = bandAUKTotal
            setOwnerBand("AUA_Catcher", bandAUAmount, True)
            .Columns("AUA_Total").OwnerBand = bandAUATotal
            setOwnerBand("SK_Catcher", bandSKilos, False, True)
            .Columns("SK_Total").OwnerBand = bandSKTotal
            setOwnerBand("SA_Catcher", bandSAmount, True)
            .Columns("SA_Total").OwnerBand = bandSATotal
            setOwnerBand("NK_Catcher", bandNKilos, True, True)
            .Columns("NK_Total").OwnerBand = bandNKTotal
            setOwnerBand("NA_Catcher", bandNAmount, True)
            .Columns("NA_Total").OwnerBand = bandNATotal

            .Columns("Class").OptionsColumn.ReadOnly = True
            .Columns("Size").OptionsColumn.ReadOnly = True
            .Columns("AUK_Total").OptionsColumn.ReadOnly = True
            .Columns("AUA_Total").OptionsColumn.ReadOnly = True
            .Columns("SK_Total").OptionsColumn.ReadOnly = True
            .Columns("SA_Total").OptionsColumn.ReadOnly = True
            .Columns("NK_Total").OptionsColumn.ReadOnly = True
            .Columns("NA_Total").OptionsColumn.ReadOnly = True

            bandClass.Fixed = Columns.FixedStyle.Left
            bandSize.Fixed = Columns.FixedStyle.Left

            .OptionsView.ShowFooter = True

            .Columns("Class").AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            .Columns("Size").AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center

            For Each band As DevExpress.XtraGrid.Views.BandedGrid.GridBand In BandedGridView1.Bands
                SetHeaderAlignment(band)
            Next

            For Each col As BandedGridColumn In BandedGridView1.Columns
                col.Width = 100
            Next

            .BestFitColumns()
            .OptionsView.ColumnAutoWidth = False
            .OptionsView.ShowColumnHeaders = False
        End With

    End Sub

    Sub populateBand(caption As String, ByRef parent As GridBand, count As Integer, Optional catcherID As Integer = Nothing)
        For i As Integer = 1 To count
            Dim band = AddBand(caption & " " & i, parent, i, catcherID)
        Next
    End Sub

    Sub setOwnerBand(caption As String, parentBand As GridBand, Optional isReadOnly As Boolean = False, Optional isKilo As Boolean = False)
        Dim countBand = 1
        For Each band As GridBand In parentBand.Children
            If band.Caption IsNot "Total" Then
                With BandedGridView1.Columns(caption & countBand)
                    .OwnerBand = band
                    .UnboundType = DevExpress.Data.UnboundColumnType.Decimal
                    If isReadOnly Then .OptionsColumn.ReadOnly = True
                    .Summary.Add(DevExpress.Data.SummaryItemType.Sum, caption & countBand, "Total: {0}")
                End With
            End If
            countBand = countBand + 1
        Next
    End Sub

    Sub SetHeaderAlignment(ByVal band As DevExpress.XtraGrid.Views.BandedGrid.GridBand)
        band.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center

        For Each subBand As DevExpress.XtraGrid.Views.BandedGrid.GridBand In band.Children.Cast(Of DevExpress.XtraGrid.Views.BandedGrid.GridBand)()
            SetHeaderAlignment(subBand)
        Next
    End Sub

    Function AddBand(ByVal caption As String, ByRef parent As GridBand, Optional index As Integer = 0, Optional catcherID As Integer = 0) As GridBand


        Dim band As New GridBand() With {.Caption = caption}

        If catcherID <> 0 Then
            Dim dc As New mkdbDataContext
            Dim mdc As New tpmdbDataContext

            Dim vesselDict = mdc.ml_Vessels.ToDictionary(Function(v) v.ml_vID, Function(v) v.vesselName)

            ' STEP 2: Fetch the vessel IDs directly using Take(1) to avoid loading all data
            Dim vesselID = (From j In dc.trans_CatchActivityDetails
                            Where j.catchActivity_ID = catcherID
                            Select j.vessel_ID).Distinct().Skip(index - 1).Take(1).FirstOrDefault()

            ' Step 3: Assign vessel name safely
            band.Caption = vesselDict(vesselID)
        End If

        parent.Children.Add(band)
        Return band
    End Function

    Function AddBand(ByVal caption As String, ByRef gridView As BandedGridView) As GridBand
        Dim band As New GridBand() With {.Caption = caption}
        gridView.Bands.Add(band)
        Return band
    End Function

    Private Sub BandedGridView1_CellValueChanged(sender As Object, e As Views.Base.CellValueChangedEventArgs) Handles BandedGridView1.CellValueChanged
        Dim view As BandedGridView = TryCast(sender, DevExpress.XtraGrid.Views.BandedGrid.BandedGridView)
        If view Is Nothing Then
            Return
        End If
        If e.Column.FieldName = "AUK_Total" Or e.Column.FieldName = "AUA_Total" Or e.Column.FieldName = "SK_Total" _
            Or e.Column.FieldName = "SA_Total" Or e.Column.FieldName = "NK_Total" Or e.Column.FieldName = "NA_Total" Then
            Return
        End If

        Dim r As DataRowView = CType(view.GetRow(view.FocusedRowHandle), DataRowView)
        ctrlSales.updateTotal(r.Row)
    End Sub

    Private Sub GridControl2_Load(sender As Object, e As EventArgs) Handles GridControl2.Load
        Dim bandCatcher = AddBand("Catcher", BandedGridView2)
        Dim bandTonnage = AddBand("Tonnage", BandedGridView2)
        Dim bandKilos = AddBand("Kilos", BandedGridView2)
        Dim bandAmount = AddBand("Amount", BandedGridView2)

        Dim bandCCP = AddBand("Catcher Partial", bandTonnage)
        Dim bandCAQ = AddBand("Actual Qty", bandTonnage)

        Dim bandKAQ = AddBand("Actual Qty", bandKilos)
        Dim bandKF = AddBand("FishMeal", bandKilos)
        Dim bandKS = AddBand("Spoilage", bandKilos)
        Dim bandKN = AddBand("Net", bandKilos)

        Dim bandAAQ = AddBand("Actual Qty", bandAmount)
        Dim bandAF = AddBand("FishMeal", bandAmount)
        Dim bandAS = AddBand("Spoilage", bandAmount)
        Dim bandAN_USD = AddBand("Net In USD", bandAmount)
        Dim bandAN_PHP = AddBand("Net In PHP", bandAmount)
        Dim bandAAP = AddBand("Average Price per Catcher", bandAmount)


        With BandedGridView2
            .PopulateColumns()

            .Columns("Catcher").OwnerBand = bandCatcher
            .Columns("T_CatcherPartial").OwnerBand = bandTonnage
            .Columns("T_ActualQty").OwnerBand = bandTonnage
            .Columns("K_ActualQty").OwnerBand = bandKilos
            .Columns("K_Fishmeal").OwnerBand = bandKilos
            .Columns("K_Spoilage").OwnerBand = bandKilos
            .Columns("K_Net").OwnerBand = bandKilos
            .Columns("A_ActualQty").OwnerBand = bandAmount
            .Columns("A_Fishmeal").OwnerBand = bandAmount
            .Columns("A_Spoilage").OwnerBand = bandAmount
            .Columns("A_NetUSD").OwnerBand = bandAmount
            .Columns("A_NetPHP").OwnerBand = bandAmount
            .Columns("A_AveragePrice").OwnerBand = bandAmount

        End With

        BandedGridView2.OptionsView.ShowFooter = True

        For Each band As DevExpress.XtraGrid.Views.BandedGrid.GridBand In BandedGridView2.Bands
            SetHeaderAlignment(band)
        Next

        With BandedGridView2
            .Columns("T_ActualQty").UnboundType = DevExpress.Data.UnboundColumnType.Decimal
            .Columns("K_ActualQty").UnboundType = DevExpress.Data.UnboundColumnType.Decimal
            .Columns("K_Net").UnboundType = DevExpress.Data.UnboundColumnType.Decimal
            .Columns("A_ActualQty").UnboundType = DevExpress.Data.UnboundColumnType.Decimal
            .Columns("A_NetUSD").UnboundType = DevExpress.Data.UnboundColumnType.Decimal
            .Columns("A_NetPHP").UnboundType = DevExpress.Data.UnboundColumnType.Decimal
            .Columns("A_AveragePrice").UnboundType = DevExpress.Data.UnboundColumnType.Decimal

            .BestFitColumns()
            .OptionsView.ColumnAutoWidth = False
            .OptionsView.ShowColumnHeaders = False
        End With
    End Sub

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnSave.ItemClick
        ' Validate fields
        Dim dateCreated = validateField(dtCreated)
        Dim sellType = validateField(cmbST)
        Dim unloadingVessel = validateField(cmbUV)
        Dim salesNum = validateField(txtSaleNum)
        Dim catchDeliveryNum = validateField(txtCDNum)
        Dim usdRate = validateField(txtUSD)
        Dim contactNum = validateField(txtCNum)
        Dim remark = validateField(txtRemark)

        Dim missingFields As New StringBuilder()
        If Not dateCreated Then missingFields.AppendLine("Date Created")
        If Not sellType Then missingFields.AppendLine("Sell Type")
        If Not unloadingVessel Then missingFields.AppendLine("Unloading Vessel")
        If Not salesNum Then missingFields.AppendLine("Sales Number")
        If Not catchDeliveryNum Then missingFields.AppendLine("Catch Delivery Number")
        If Not usdRate Then missingFields.AppendLine("USD Rate")
        If Not contactNum Then missingFields.AppendLine("Contact Number")
        If Not remark Then missingFields.AppendLine("Remarks")

        If CInt(rBT.EditValue) = 1 Then
            If validateField(txtBuyer) Then buyerName = txtBuyer.Text : buyerID = Nothing _
                Else missingFields.AppendLine("Buyer")
        ElseIf CInt(rBT.EditValue) = 2 Then
            If validateField(cmbBuyer) Then buyerName = cmbBuyer.Text : buyerID = CInt(cmbBuyer.GetColumnValue("ID")) _
                Else missingFields.AppendLine("Buyer")
        Else
            missingFields.AppendLine("Select Type of Buyer")
        End If

        If missingFields.Length > 0 Then
            requiredMessage(missingFields.ToString())
            Return
        End If

        ' Save the draft
        ctrlSales.saveDraft()
        SuccessfullySavedMessage() ' Show success message

        ' Set unsaved changes to false after saving
        hasUnsavedChanges = False
    End Sub

    Private Sub txtSaleNum_TextChanged(sender As Object, e As EventArgs) Handles txtSaleNum.TextChanged
        hasUnsavedChanges = True ' Set to true when the user modifies a field
    End Sub

    Private Sub BarButtonItem3_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnPost.ItemClick
        Dim message = XtraMessageBox.Show("Are you sure you want to post this report?", APPNAME, MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If message = Windows.Forms.DialogResult.Yes Then ctrlSales.postedDraft() Else Return
    End Sub

    Private Sub btnDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnDelete.ItemClick
        ctrlSales.deleteSales()
    End Sub

    Private Sub frm_salesInvoice_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If hasUnsavedChanges Then
            If Not ConfirmCloseWithoutSaving() Then
                e.Cancel = True ' Cancel the closing event
            End If
        End If
    End Sub



    Private Sub cmbUV_EditValueChanged(sender As Object, e As EventArgs) Handles cmbUV.EditValueChanged
        BandedGridView1.Bands.Clear()
        Dim catcher = CType(sender, DevExpress.XtraEditors.LookUpEdit)
        'Debug.WriteLine(catcher.EditValue)
        ctrlSales.initSalesDataTable(CInt(catcher.EditValue))
        GridControl1.DataSource = dt
        createBands(rowCount, CInt(catcher.EditValue))
        ctrlSales.loadRows()

        ' Get reference number
        Dim dc As New mkdbDataContext
        Dim getValue = (From i In dc.trans_CatchActivityDetails
                        Join j In dc.trans_CatchActivities On j.catchActivity_ID Equals i.catchActivity_ID
                        Where i.catchActivityDetail_ID = CInt(catcher.EditValue)
                        Select j.catchReferenceNum).Distinct().FirstOrDefault
        txtCDNum.EditValue = getValue
    End Sub
End Class