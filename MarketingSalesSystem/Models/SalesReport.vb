Public Class SalesReport
    Public salesReport_ID As Integer
    Public salesDate As Date?
    Public salesNum As String
    Public sellingType As String
    Public unloadingType As String
    Public unloadingVessel_ID As Integer?
    Public unloadingForeignVessel As String
    Public buyer As String
    Public catchtDeliveryNum As String
    Public usdRate As Decimal?
    Public contractNum As String
    Public remarks As String
    Public encodedBy As Integer?
    Public encodedOn As Date?
    Public approvalStatus As Integer?

    Private dc As mkdbDataContext

    Sub New(ByRef dc_ As mkdbDataContext)
        dc = dc_
    End Sub

    Sub New(ByVal salesReportID As Integer, ByRef dc_ As mkdbDataContext)
        dc = dc_

        Dim e = From i In dc.trans_SalesReports Where i.salesReport_ID = salesReportID Select i

        For Each i In e
            Me.salesReport_ID = salesReportID
            salesDate = i.salesDate
            salesNum = i.salesNum
            sellingType = i.sellingType
            unloadingType = i.unloadingType
            unloadingVessel_ID = i.unloadingVessel_ID
            unloadingForeignVessel = i.unloadingForeignVessel
            buyer = i.buyer
            catchtDeliveryNum = i.catchtDeliveryNum
            usdRate = i.usdRate
            contractNum = i.contractNum
            remarks = i.remarks
            encodedBy = i.encodedBy
            encodedOn = i.encodedOn
            approvalStatus = i.approvalStatus
        Next
    End Sub

    Sub New(ByRef sr As trans_SalesReport, ByRef dc_ As mkdbDataContext)
        dc = dc_

        Dim i = sr

        Me.salesReport_ID = i.salesReport_ID
        salesDate = i.salesDate
        salesNum = i.salesNum
        sellingType = i.sellingType
        unloadingType = i.unloadingType
        unloadingVessel_ID = i.unloadingVessel_ID
        unloadingForeignVessel = i.unloadingForeignVessel
        buyer = i.buyer
        catchtDeliveryNum = i.catchtDeliveryNum
        usdRate = i.usdRate
        contractNum = i.contractNum
        remarks = i.remarks
        encodedBy = i.encodedBy
        encodedOn = i.encodedOn
        approvalStatus = i.approvalStatus
    End Sub

    Function getRows() As List(Of SalesReport)
        Dim srList As New List(Of SalesReport)

        Dim srs = From item In dc.trans_SalesReports Select item

        For Each sr In srs
            srList.Add(New SalesReport(sr, dc))
        Next

        Return srList
    End Function
End Class
