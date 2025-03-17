Public Class SalesReportSummary
    Public salesReportSummary_ID As Integer
    Public salesReport_ID As Integer?
    Public vessel_ID As Integer?
    Public catchersPartialQty As Decimal?
    Public catchersPartialUnloadedQty As Decimal?
    Public catchersActualUnloadedQty As Decimal?
    Public actualQtyInKilos As Decimal?
    Public fishmealInKilos As Decimal?
    Public spoilageInKilos As Decimal?
    Public actualQtyInAmount As Decimal?
    Public fishmealInAmount As Decimal?
    Public spoilageInAmount As Decimal?

    Private dc As mkdbDataContext

    Sub New(ByRef dc_ As mkdbDataContext)
        dc = dc_
    End Sub

    Sub New(ByVal salesReportSummaryID As Integer, ByRef dc_ As mkdbDataContext)
        dc = dc_

        Dim e = From i In dc.trans_SalesReportSummaries Where i.salesReportSummary_ID = salesReportSummaryID Select i

        For Each i In e
            Me.salesReportSummary_ID = salesReportSummaryID
            salesReport_ID = i.salesReport_ID
            vessel_ID = i.vessel_ID
            catchersPartialQty = i.catchersPartialQty
            catchersPartialUnloadedQty = i.catchersPartialUnloadedQty
            catchersActualUnloadedQty = i.catchersActualUnloadedQty
            actualQtyInKilos = i.actualQtyInKilos
            fishmealInKilos = i.fishmealInKilos
            spoilageInKilos = i.spoilageInKilos
            actualQtyInAmount = i.actualQtyInAmount
            fishmealInAmount = i.fishmealInAmount
            spoilageInAmount = i.spoilageInAmount
        Next
    End Sub
End Class
