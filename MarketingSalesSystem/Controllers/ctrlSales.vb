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
            generateCombo()
            .Show()
        End With
    End Sub

    Sub generateCombo()
        Dim item = (From i In tpmdb.ml_SupplierCategories Select i.ml_supCat).ToArray
        frmSI.txtST.Properties.Items.AddRange(item)

        item = (From i In tpmdb.ml_VesselTypes Select i.ml_vesType).ToArray
        frmSI.txtUVT.Properties.Items.AddRange(item)
    End Sub

End Class
