Public Class WeightSlip
    Public weightSlip_ID As Integer
    Public weightSlipDate As Date?
    Public fishWeightSlipNum As String
    Public sellingType As String
    Public buyer As String
    Public unloadingType As String
    Public unloadingVessel_ID As Integer?
    Public unloadingForeignVessel As String
    Public catcherVessel_ID As Integer?
    Public companyChecker_ID As Integer?
    Public buyersChecker As String
    Public contractNum As String
    Public remarks As String
    Public encodedBy As Integer?
    Public encodedOn As Date?

    Private dc As mkdbDataContext

    Sub New(ByRef dc_ As mkdbDataContext)
        dc = dc_
    End Sub

    Sub New(ByVal weightSlipID As Integer, ByRef dc_ As mkdbDataContext)
        dc = dc_

        Dim e = From i In dc.trans_WeightSlips Where i.weightSlip_ID = weightSlipID Select i

        For Each i In e
            Me.weightSlip_ID = weightSlipID
            weightSlipDate = i.weightSlipDate
            fishWeightSlipNum = i.fishWeightSlipNum
            sellingType = i.sellingType
            buyer = i.buyer
            unloadingType = i.unloadingType
            unloadingVessel_ID = i.unloadingVessel_ID
            unloadingForeignVessel = i.unloadingForeignVessel
            catcherVessel_ID = i.catcherVessel_ID
            companyChecker_ID = i.companyChecker_ID
            buyersChecker = i.buyersChecker
            contractNum = i.contractNum
            remarks = i.remarks
            encodedBy = i.encodedBy
            encodedOn = i.encodedOn
        Next
    End Sub

    Sub New(ByVal ws As trans_WeightSlip, ByRef dc_ As mkdbDataContext)
        dc = dc_

        Me.weightSlip_ID = ws.weightSlip_ID
        weightSlipDate = ws.weightSlipDate
        fishWeightSlipNum = ws.fishWeightSlipNum
        sellingType = ws.sellingType
        buyer = ws.buyer
        unloadingType = ws.unloadingType
        unloadingVessel_ID = ws.unloadingVessel_ID
        unloadingForeignVessel = ws.unloadingForeignVessel
        catcherVessel_ID = ws.catcherVessel_ID
        companyChecker_ID = ws.companyChecker_ID
        buyersChecker = ws.buyersChecker
        contractNum = ws.contractNum
        remarks = ws.remarks
        encodedBy = ws.encodedBy
        encodedOn = ws.encodedOn
    End Sub
End Class
