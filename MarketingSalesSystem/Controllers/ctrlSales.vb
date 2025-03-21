Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout

Public Class ctrlSales

    Private isNew As Boolean
    Private mdlSR As SalesReport
    Private mdlSRP As SalesReportPrice
    Private mdlSRS As SalesReportSummary

    Private frmSI As frm_salesInvoice

    Private mkdb As mkdbDataContext
    Private tpmdb As tpmdbDataContext

    Sub New()
        isNew = True

        mkdb = New mkdbDataContext
        tpmdb = New tpmdbDataContext

        mdlSR = New SalesReport(mkdb)
        mdlSRP = New SalesReportPrice(mkdb)
        mdlSRS = New SalesReportSummary(mkdb)

        frmSI = New frm_salesInvoice(Me)

        initSalesDataTable()

        frmSI.GridControl1.DataSource = frmSI.dt

        With frmSI
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

    Sub loadRows()
        Dim fishClasses As New Dictionary(Of String, String()) From {
            {"SKIPJACK", New String() {"0.300 - 0.499", "0.500 - 0.999", "1.0 - 1.39", "1.4 - 1.79", "1.8 - 2.49", "2.5 - 3.49", "3.5 - UP"}},
            {"YELLOWFIN", New String() {"0.300 - 0.499", "0.500 - 0.999", "1.0 - 1.49", "1.5 - 2.49", "2.5 - 3.49", "3.5 - 4.99", "5.0 - 9.99", "10 - UP GOOD", "10 - UP DEFORMED"}},
            {"BIGEYE", New String() {"0.500 - 0.999", "1.0 - 1.49", "1.5 - 2.49", "2.5 - 3.49", "3.5 - 4.99", "5.0 - 9.99", "10 - UP"}},
            {"BONITO", New String() {"ALL SIZES"}},
            {"FISHMEAL", New String() {"ALL SIZES"}}
        }

        ' Loop through each fish class and sizes
        For Each fishClass In fishClasses
            For Each size In fishClass.Value
                AddFishRow(fishClass.Key, size)
            Next
        Next
    End Sub

    Sub AddFishRow(fishClass As String, size As String)
        Dim dr As DataRow = frmSI.dt.NewRow()
        dr("Class") = fishClass
        dr("Size") = size
        dr("Price") = 0

        ' Set all catcher and total columns to 0 dynamically
        Dim columns As String() = {"AUK_Catcher1", "AUK_Catcher2", "AUK_Total",
                                   "AUA_Catcher1", "AUA_Catcher2", "AUA_Total",
                                   "SK_Catcher1", "SK_Catcher2", "SK_Total",
                                   "SA_Catcher1", "SA_Catcher2", "SA_Total",
                                   "NK_Catcher1", "NK_Catcher2", "NK_Total",
                                   "NA_Catcher1", "NA_Catcher2", "NA_Total"}

        For Each col In columns
            dr(col) = 0
        Next

        frmSI.dt.Rows.Add(dr)
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

End Class
