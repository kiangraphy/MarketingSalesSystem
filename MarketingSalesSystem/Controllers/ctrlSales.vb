Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout
Imports System.Transactions

Public Class ctrlSales

    Private isNew As Boolean
    Private mdlSR As SalesReport
    Private mdlSRP As SalesReportPrice

    Private frmSI As frm_salesInvoice

    Private mkdb As mkdbDataContext
    Private tpmdb As tpmdbDataContext

    Sub New()
        isNew = True

        mkdb = New mkdbDataContext
        tpmdb = New tpmdbDataContext

        mdlSR = New SalesReport(mkdb)
        mdlSRP = New SalesReportPrice(mkdb)

        frmSI = New frm_salesInvoice(Me)

        initSalesDataTable()
        initSalesDataTableS()

        frmSI.GridControl1.DataSource = frmSI.dt
        frmSI.GridControl2.DataSource = frmSI.dts

        With frmSI
            .dtCreated.Properties.MaxValue = Date.Now
            loadRows()
            .cmbBuyer.Enabled = False
            .txtBuyer.Enabled = False
            .lcmbBuyer.Visibility = Utils.LayoutVisibility.Never
            .ltxtBuyer.Visibility = Utils.LayoutVisibility.Never
            .cmbVessel.Enabled = False
            generateCombo()
            .Show()
        End With
    End Sub

    Sub New(ByVal salesID As Integer)
        isNew = False

        mkdb = New mkdbDataContext
        tpmdb = New tpmdbDataContext

        mdlSR = New SalesReport(salesID, mkdb)
        mdlSRP = New SalesReportPrice(mdlSR.salesReport_ID, mkdb)

        frmSI = New frm_salesInvoice(Me)

        initSalesDataTable()
        initSalesDataTableS()

        frmSI.GridControl1.DataSource = frmSI.dt
        frmSI.GridControl2.DataSource = frmSI.dts

        With frmSI
            .dtCreated.Properties.MaxValue = Date.Now
            loadRows()
            .cmbBuyer.Enabled = False
            .txtBuyer.Enabled = False
            .lcmbBuyer.Visibility = Utils.LayoutVisibility.Never
            .ltxtBuyer.Visibility = Utils.LayoutVisibility.Never
            .cmbVessel.Enabled = False
            generateCombo()

            'Fields
            .dtCreated.EditValue = mdlSR.salesDate
            .cmbST.EditValue = mdlSR.sellingType
            .cmbUVT.EditValue = mdlSR.unloadingType
            .txtSaleNum.EditValue = mdlSR.salesNum

            If CInt(mdlSR.unloadingForeignVessel) = 0 Then .rBT.SelectedIndex = 0 Else .rBT.SelectedIndex = 1
            If CInt(.rBT.EditValue) = 1 Then .txtBuyer.EditValue = mdlSR.buyer Else .cmbBuyer.EditValue = mdlSR.unloadingForeignVessel
            .cmbVessel.EditValue = mdlSR.unloadingVessel_ID
            .txtUSD.EditValue = mdlSR.usdRate
            .txtCDNum.EditValue = mdlSR.catchtDeliveryNum
            .txtCNum.EditValue = mdlSR.contractNum
            .txtRemark.EditValue = mdlSR.remarks

            .Show()
        End With
    End Sub

    Sub initSalesDataTable()
        With frmSI
            .dt = New DataTable()

            .dt.Columns.Add("Class", GetType(String))
            .dt.Columns.Add("Size", GetType(String))
            .dt.Columns.Add("Price", GetType(Double))
            .dt.Columns.Add("AUK_Catcher1", GetType(Double))
            .dt.Columns.Add("AUK_Catcher2", GetType(Double))
            .dt.Columns.Add("AUK_Total", GetType(Double))
            .dt.Columns.Add("AUA_Catcher1", GetType(Double))
            .dt.Columns.Add("AUA_Catcher2", GetType(Double))
            .dt.Columns.Add("AUA_Total", GetType(Double))
            .dt.Columns.Add("SK_Catcher1", GetType(Double))
            .dt.Columns.Add("SK_Catcher2", GetType(Double))
            .dt.Columns.Add("SK_Total", GetType(Double))
            .dt.Columns.Add("SA_Catcher1", GetType(Double))
            .dt.Columns.Add("SA_Catcher2", GetType(Double))
            .dt.Columns.Add("SA_Total", GetType(Double))
            .dt.Columns.Add("NK_Catcher1", GetType(Double))
            .dt.Columns.Add("NK_Catcher2", GetType(Double))
            .dt.Columns.Add("NK_Total", GetType(Double))
            .dt.Columns.Add("NA_Catcher1", GetType(Double))
            .dt.Columns.Add("NA_Catcher2", GetType(Double))
            .dt.Columns.Add("NA_Total", GetType(Double))
        End With
    End Sub

    Sub updateTotal(ByRef r As DataRow)
        Dim Price As Decimal = 0

        'Actual Unloading Kilos
        Dim AUK_Catcher1 As Decimal = 0
        Dim AUK_Catcher2 As Decimal = 0
        Dim AUK_Total As Decimal = 0

        'Actual Unloading Amount
        Dim AUA_Catcher1 As Decimal = 0
        Dim AUA_Catcher2 As Decimal = 0
        Dim AUA_Total As Decimal = 0

        'Spoilage Kilos
        Dim SK_Catcher1 As Decimal = 0
        Dim SK_Catcher2 As Decimal = 0
        Dim SK_Total As Decimal = 0

        'Spoilage Amount
        Dim SA_Catcher1 As Decimal = 0
        Dim SA_Catcher2 As Decimal = 0
        Dim SA_Total As Decimal = 0

        'Net Kilos
        Dim NK_Catcher1 As Decimal = 0
        Dim NK_Catcher2 As Decimal = 0
        Dim NK_Total As Decimal = 0

        'Net Amount
        Dim NA_Catcher1 As Decimal = 0
        Dim NA_Catcher2 As Decimal = 0
        Dim NA_Total As Decimal = 0

        With r
            Price = Price + CDec(.Item("Price"))

            'Actual Unloading Kilos
            AUK_Catcher1 = AUK_Catcher1 + CDec(.Item("AUK_Catcher1"))
            AUK_Catcher2 = AUK_Catcher2 + CDec(.Item("AUK_Catcher2"))
            AUK_Total = AUK_Total + AUK_Catcher1 + AUK_Catcher2

            'Actual Unloading Amount
            AUA_Catcher1 = AUA_Catcher1 + (AUK_Catcher1 * Price)
            AUA_Catcher2 = AUA_Catcher2 + (AUK_Catcher2 * Price)
            AUA_Total = AUA_Total + AUA_Catcher1 + AUA_Catcher2

            'Spoilage Kilos
            SK_Catcher1 = SK_Catcher1 + CDec(.Item("SK_Catcher1"))
            SK_Catcher2 = SK_Catcher2 + CDec(.Item("SK_Catcher2"))
            SK_Total = SK_Total + SK_Catcher1 + SK_Catcher2

            'Spoilage Amount
            SA_Catcher1 = SA_Catcher1 + (SK_Catcher1 * Price)
            SA_Catcher2 = SA_Catcher2 + (SK_Catcher2 * Price)
            SA_Total = SA_Total + SA_Catcher1 + SA_Catcher2

            'Net Kilos
            NK_Catcher1 = NK_Catcher1 + (AUK_Catcher1 - SK_Catcher1)
            NK_Catcher2 = NK_Catcher2 + (AUK_Catcher2 - SK_Catcher2)
            NK_Total = NK_Total + NK_Catcher1 + NK_Catcher2

            'Net Amount
            NA_Catcher1 = NA_Catcher1 + (AUA_Catcher1 - SA_Catcher1)
            NA_Catcher2 = NA_Catcher2 + (AUA_Catcher2 - SA_Catcher2)
            NA_Total = NA_Total + NA_Catcher1 + NA_Catcher2
        End With

        'Set Row Value

        'Actual Unloading
        r("AUK_Total") = AUK_Total
        r("AUA_Catcher1") = AUA_Catcher1
        r("AUA_Catcher2") = AUA_Catcher2
        r("AUA_Total") = AUA_Total

        'Spoilage
        r("SK_Total") = SK_Total
        r("SA_Catcher1") = SA_Catcher1
        r("SA_Catcher2") = SA_Catcher2
        r("SA_Total") = SA_Total

        'Net
        r("NK_Catcher1") = NK_Catcher1
        r("NK_Catcher2") = NK_Catcher2
        r("NK_Total") = NK_Total
        r("NA_Catcher1") = NA_Catcher1
        r("NA_Catcher2") = NA_Catcher2
        r("NA_Total") = NA_Total
    End Sub

    Sub updateAllTotals()
        For Each r As DataRow In frmSI.dt.Rows
            updateTotal(r)
        Next
    End Sub

    Sub savePost()
        Dim srs As New SalesReportSummary(mkdb)

        Using ts As New TransactionScope()
            Try
                Dim sr As New SalesReport(mkdb)

                With frmSI
                    sr.salesDate = CDate(.dtCreated.EditValue)
                    sr.salesNum = .txtSaleNum.Text
                    sr.sellingType = .cmbST.EditValue.ToString
                    sr.unloadingType = .cmbUVT.EditValue.ToString
                    sr.unloadingVessel_ID = CInt(.cmbVessel.EditValue)
                    sr.unloadingForeignVessel = .buyerID.ToString
                    sr.buyer = .buyerName.ToString
                    sr.catchtDeliveryNum = .txtCDNum.Text
                    sr.usdRate = CDec(.txtUSD.EditValue)
                    sr.contractNum = .txtCNum.Text
                    sr.remarks = .txtRemark.Text
                    sr.encodedBy = 1
                    sr.encodedOn = Date.Now
                    sr.approvalStatus = 1
                    sr.Add()
                End With

                setSalesPrice("Price", sr.salesReport_ID)
                setSalesPrice("AUK_Catcher1", sr.salesReport_ID)
                setSalesPrice("AUK_Catcher2", sr.salesReport_ID)
                setSalesPrice("SK_Catcher1", sr.salesReport_ID)
                setSalesPrice("SK_Catcher2", sr.salesReport_ID)
                ts.Complete()
                Debug.WriteLine("saved post...")
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
            End Try
        End Using
    End Sub

    Sub setSalesPrice(rowName As String, ByVal salesReportID As Integer)
        Dim srp As New SalesReportPrice(mkdb)

        With srp
            .salesReport_ID = salesReportID
            .skipjack0_300To0_499 = CDec(frmSI.dt.Rows(0)(rowName))
            .skipjack0_500To0_999 = CDec(frmSI.dt.Rows(1)(rowName))
            .skipjack1_0To1_39 = CDec(frmSI.dt.Rows(2)(rowName))
            .skipjack1_4To1_79 = CDec(frmSI.dt.Rows(3)(rowName))
            .skipjack1_8To2_49 = CDec(frmSI.dt.Rows(4)(rowName))
            .skipjack2_5To3_49 = CDec(frmSI.dt.Rows(5)(rowName))
            .skipjack3_5AndUP = CDec(frmSI.dt.Rows(6)(rowName))
            .yellowfin0_300To0_499 = CDec(frmSI.dt.Rows(7)(rowName))
            .yellowfin0_500To0_999 = CDec(frmSI.dt.Rows(8)(rowName))
            .yellowfin1_0To1_49 = CDec(frmSI.dt.Rows(9)(rowName))
            .yellowfin1_5To2_49 = CDec(frmSI.dt.Rows(10)(rowName))
            .yellowfin2_5To3_49 = CDec(frmSI.dt.Rows(11)(rowName))
            .yellowfin3_5To4_99 = CDec(frmSI.dt.Rows(12)(rowName))
            .yellowfin5_0To9_99 = CDec(frmSI.dt.Rows(13)(rowName))
            .yellowfin10AndUpGood = CDec(frmSI.dt.Rows(14)(rowName))
            .yellowfin10AndUpDeformed = CDec(frmSI.dt.Rows(15)(rowName))
            .bigeye0_500To0_999 = CDec(frmSI.dt.Rows(16)(rowName))
            .bigeye1_0To1_49 = CDec(frmSI.dt.Rows(17)(rowName))
            .bigeye1_5To2_49 = CDec(frmSI.dt.Rows(18)(rowName))
            .bigeye2_5To3_49 = CDec(frmSI.dt.Rows(19)(rowName))
            .bigeye3_5To4_99 = CDec(frmSI.dt.Rows(20)(rowName))
            .bigeye5_0To9_99 = CDec(frmSI.dt.Rows(21)(rowName))
            .bigeye10AndUP = CDec(frmSI.dt.Rows(22)(rowName))
            .bonito = CDec(frmSI.dt.Rows(23)(rowName))
            .fishmeal = CDec(frmSI.dt.Rows(24)(rowName))
            .Add()
        End With
    End Sub


    Sub loadRows()
        Dim fishClasses As New Dictionary(Of String, String()) From {
               {"SKIPJACK", New String() {"0.300 - 0.499", "0.500 - 0.999", "1.0 - 1.39", "1.4 - 1.79", "1.8 - 2.49", "2.5 - 3.49", "3.5 - UP"}},
               {"YELLOWFIN", New String() {"0.300 - 0.499", "0.500 - 0.999", "1.0 - 1.49", "1.5 - 2.49", "2.5 - 3.49", "3.5 - 4.99", "5.0 - 9.99", "10 - UP GOOD", "10 - UP DEFORMED"}},
               {"BIGEYE", New String() {"0.500 - 0.999", "1.0 - 1.49", "1.5 - 2.49", "2.5 - 3.49", "3.5 - 4.99", "5.0 - 9.99", "10 - UP"}},
               {"BONITO", New String() {"ALL SIZES"}},
               {"FISHMEAL", New String() {"ALL SIZES"}}
           }

        Dim columns As String() = {"AUK_Catcher1", "AUK_Catcher2", "AUK_Total",
                                   "AUA_Catcher1", "AUA_Catcher2", "AUA_Total",
                                   "SK_Catcher1", "SK_Catcher2", "SK_Total",
                                   "SA_Catcher1", "SA_Catcher2", "SA_Total",
                                   "NK_Catcher1", "NK_Catcher2", "NK_Total",
                                   "NA_Catcher1", "NA_Catcher2", "NA_Total"}
        If isNew Then

            ' Loop through each fish class and sizes
            For Each fishClass In fishClasses
                For Each size In fishClass.Value
                    Dim dr As DataRow = frmSI.dt.NewRow()
                    dr("Price") = 0
                    dr("Class") = fishClass.Key
                    dr("Size") = size

                    For Each col In columns
                        dr(col) = 0
                    Next

                    frmSI.dt.Rows.Add(dr)
                Next
            Next
        Else

            Dim columnNames As List(Of String) = GetType(trans_SalesReportPrice).GetProperties().Select(Function(t) t.Name).ToList()
            columnNames.RemoveAll(Function(c) c = "salesReportPrice_ID" Or c = "salesReport_ID")
            Dim srpList = mkdb.trans_SalesReportPrices.Where(Function(sp) sp.salesReport_ID = mdlSR.salesReport_ID).ToList()

            Dim count = 0

            For Each fishClass In fishClasses
                For Each size In fishClass.Value

                    Dim columnName As String = columnNames(count)

                    Dim dr As DataRow = frmSI.dt.NewRow()
                    dr("Class") = fishClass.Key
                    dr("Size") = size

                    ' Assign values if properties exist
                    Dim propInfo = GetType(trans_SalesReportPrice).GetProperty(columnName)
                    dr("Price") = CDec(propInfo.GetValue(srpList(0), Nothing))

                    For Each col In columns

                        Select Case col
                            Case "AUK_Catcher1"
                                dr("AUK_Catcher1") = CDec(propInfo.GetValue(srpList(1), Nothing))
                            Case "AUK_Catcher2"
                                dr("AUK_Catcher2") = CDec(propInfo.GetValue(srpList(2), Nothing))
                            Case "SK_Catcher1"
                                dr("SK_Catcher1") = CDec(propInfo.GetValue(srpList(3), Nothing))
                            Case "SK_Catcher2"
                                dr("SK_Catcher2") = CDec(propInfo.GetValue(srpList(4), Nothing))
                            Case Else
                                dr(col) = 0
                        End Select
                    Next

                    frmSI.dt.Rows.Add(dr)
                    count += 1 ' Move to the next column
                Next
            Next
            updateAllTotals()
        End If
    End Sub


    Sub generateCombo()
        Dim item = (From i In tpmdb.ml_SupplierCategories Select i.ml_supCat).ToArray
        frmSI.cmbST.Properties.Items.AddRange(item)

        item = (From i In tpmdb.ml_VesselTypes Select i.ml_vesType).ToArray
        frmSI.cmbUVT.Properties.Items.AddRange(item)
    End Sub

    Sub changeBuyerInput()
        With frmSI
            .cmbBuyer.Enabled = False
            .txtBuyer.Enabled = True
            .lcmbBuyer.Visibility = Utils.LayoutVisibility.Never
            .ltxtBuyer.Visibility = Utils.LayoutVisibility.Always
        End With
    End Sub

    Sub changeBuyerCombo()
        With frmSI
            .cmbBuyer.Enabled = True
            .txtBuyer.Enabled = False
            .lcmbBuyer.Visibility = Utils.LayoutVisibility.Always
            .ltxtBuyer.Visibility = Utils.LayoutVisibility.Never
        End With

        Dim buyers = (From i In tpmdb.ml_Suppliers Select
                      ID = i.ml_SupID,
                      BuyerName = i.ml_Supplier).ToList()
        With frmSI.cmbBuyer.Properties
            .DataSource = buyers
            .DisplayMember = "BuyerName"
            .ValueMember = "ID"
            .NullText = "Select a buyer"
            .ShowHeader = False
            .ShowFooter = False
            .Columns.Clear()
            .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo("BuyerName", "Buyer Name"))
            .BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup
        End With
        frmSI.cmbBuyer.Enabled = True
    End Sub


    Sub createCollectionVessel(ByVal vt As Integer)
        Dim vessels = (From i In tpmdb.ml_Vessels
                       Where i.ml_vtID = vt
                       Select
                       ID = i.ml_vID,
                       VesselName = i.vesselName).ToList()
        With frmSI.cmbVessel.Properties
            .DataSource = vessels
            .DisplayMember = "VesselName"
            .ValueMember = "ID"
            .NullText = "Select a vessel"
            .ShowHeader = False
            .ShowFooter = False
            .Columns.Clear()
            .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo("VesselName", "Vessel Name"))
            .BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup
        End With
        frmSI.cmbVessel.Enabled = True
    End Sub

    Sub initSalesDataTableS()
        With frmSI
            .dts = New DataTable()

            .dts.Columns.Add("Catcher", GetType(String))
            .dts.Columns.Add("T_CatcherPartial", GetType(String))
            .dts.Columns.Add("T_ActualQty", GetType(Double))
            .dts.Columns.Add("K_ActualQty", GetType(Double))
            .dts.Columns.Add("K_Fishmeal", GetType(String))
            .dts.Columns.Add("K_Spoilage", GetType(String))
            .dts.Columns.Add("K_Net", GetType(Double))
            .dts.Columns.Add("A_ActualQty", GetType(Double))
            .dts.Columns.Add("A_Fishmeal", GetType(String))
            .dts.Columns.Add("A_Spoilage", GetType(String))
            .dts.Columns.Add("A_NetUSD", GetType(Double))
            .dts.Columns.Add("A_NetPHP", GetType(Double))
            .dts.Columns.Add("A_AveragePrice", GetType(Double))
        End With
    End Sub

End Class
