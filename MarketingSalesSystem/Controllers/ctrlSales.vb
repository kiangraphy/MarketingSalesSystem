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

    Sub loadRows()
        Dim dr As DataRow

        Dim skipjack = New String() {"0.300 - 0.499", "0.500 - 0.999", "1.0 - 1.39", "1.4 - 1.79",
                                     "1.8 - 2.49", "2.5 - 3.49", "3.5 - UP"}
        Dim yellowfin = New String() {"0.300 - 0.499", "0.500 - 0.999", "1.0 - 1.49", "1.5 - 2.49",
                                     "2.5 - 3.49", "3.5 - 4.99", "5.0 - 9.99", "10  - UP GOOD", "10 - UP DEFORMED"}
        Dim bigeye = New String() {"0.500 - 0.999", "1.0 - 1.49", "1.5 - 2.49", "2.5 - 3.49", "3.5 - 4.99",
                                   "5.0 - 9.99", "10 -  UP"}
        For Each sj In skipjack
            dr = frmSI.dt.NewRow()
            dr("Class") = "SKIPJACK"
            dr("Size") = sj
            frmSI.dt.Rows.Add(dr)
        Next
        For Each yf In yellowfin
            dr = frmSI.dt.NewRow()
            dr("Class") = "YELLOWFIN"
            dr("Size") = yf
            frmSI.dt.Rows.Add(dr)
        Next

        dr = frmSI.dt.NewRow()
        dr("Class") = "BONITO"
        dr("Size") = "ALL SIZES"
        frmSI.dt.Rows.Add(dr)
        dr = frmSI.dt.NewRow()
        dr("Class") = "FISHMEAL"
        dr("Size") = "ALL SIZES"
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
