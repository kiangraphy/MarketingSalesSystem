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

        With frmSI
            .cmbBuyer.Enabled = False
            .txtBuyer.Enabled = False
            .lcmbBuyer.Visibility = Utils.LayoutVisibility.Never
            .ltxtBuyer.Visibility = Utils.LayoutVisibility.Never
            .cmbVessel.Enabled = False
            generateCombo()
            .Show()
        End With
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
