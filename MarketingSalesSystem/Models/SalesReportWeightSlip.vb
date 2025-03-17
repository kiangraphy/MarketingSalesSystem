Public Class SalesReportWeightSlip
    Public salesReportWeightSlip_ID As Integer
    Public salesReport_ID As Integer?
    Public weightSlipDetail_ID As Integer?

    Private dc As mkdbDataContext

    Sub New(ByRef dc_ As mkdbDataContext)
        dc = dc_
    End Sub

    Sub New(ByVal salesReportWeightSlipID As Integer, ByRef dc_ As mkdbDataContext)
        dc = dc_

        Dim e = From i In dc.trans_SalesReportWeightSlips Where i.salesReportWeightSlip_ID = salesReportWeightSlipID Select i

        For Each i In e
            Me.salesReportWeightSlip_ID = i.salesReportWeightSlip_ID
            salesReport_ID = i.salesReport_ID
            weightSlipDetail_ID = i.weightSlipDetail_ID
        Next
    End Sub
End Class
