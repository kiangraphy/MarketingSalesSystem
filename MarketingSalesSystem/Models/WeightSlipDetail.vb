Public Class WeightSlipDetail
    Public weightSlipDetail_ID As Integer
    Public weightSlip_ID As Integer?
    Public weightSlipDetailDate As Date?
    Public weigslipFormNum As String
    Public batchNum As String
    Public lotNum As String
    Public plateNum As String
    Public skipjack0_200To0_299 As Decimal?
    Public skipjack0_300To0_499 As Decimal?
    Public skipjack0_500To0_999 As Decimal?
    Public skipjack1_0To1_39 As Decimal?
    Public skipjack1_4To1_79 As Decimal?
    Public skipjack1_8To2_49 As Decimal?
    Public skipjack2_5To3_49 As Decimal?
    Public skipjack3_5AndUP As Decimal?
    Public yelllowfin0_300To0_499 As Decimal?
    Public yellowfin0_500To0_999 As Decimal?
    Public yellowfin1_0To1_49 As Decimal?
    Public yellowfin1_5To2_49 As Decimal?
    Public yellowfin2_5To3_49 As Decimal?
    Public yellowfin3_5To4_99 As Decimal?
    Public yellowfin5_0To9_99 As Decimal?
    Public yellowfin10AndUP As Decimal?
    Public bigeye0_300To0_499 As Decimal?
    Public bigeye0_500To0_999 As Decimal?
    Public bigeye1_0To1_49 As Decimal?
    Public bigeye1_5To2_49 As Decimal?
    Public bigeye2_5To3_49 As Decimal?
    Public bigeye3_5To4_99 As Decimal?
    Public bigeye5_0To9_99 As Decimal?
    Public bigeye10AndUP As Decimal?
    Public bonito0_300To0_499 As Decimal?
    Public bonito0_500AndUP As Decimal?
    Public fishmeal As Decimal?

    Public ws As WeightSlip

    Private dc As mkdbDataContext

    Sub New(ByRef dc_ As mkdbDataContext)
        dc = dc_
    End Sub

    Sub New(ByVal weightSlipDetailID As Integer, ByRef dc_ As mkdbDataContext)
        dc = dc_

        Dim e = From i In dc.trans_WeightSlipDetails Where i.weightSlipDetail_ID = weightSlipDetailID Select i

        For Each i In e
            Me.weightSlipDetail_ID = i.weightSlipDetail_ID
            weightSlip_ID = i.weightSlip_ID
            weightSlipDetailDate = i.weightSlipDetailDate
            weigslipFormNum = i.weigslipFormNum
            batchNum = i.batchNum
            lotNum = i.lotNum
            plateNum = i.plateNum
            skipjack0_200To0_299 = i.skipjack0_200To0_299
            skipjack0_300To0_499 = i.skipjack0_300To0_499
            skipjack0_500To0_999 = i.skipjack0_500To0_999
            skipjack1_0To1_39 = i.skipjack1_0To1_39
            skipjack1_4To1_79 = i.skipjack1_4To1_79
            skipjack1_8To2_49 = i.skipjack1_8To2_49
            skipjack2_5To3_49 = i.skipjack2_5To3_49
            skipjack3_5AndUP = i.skipjack3_5AndUP
            yelllowfin0_300To0_499 = i.yelllowfin0_300To0_499
            yellowfin0_500To0_999 = i.yellowfin0_500To0_999
            yellowfin1_0To1_49 = i.yellowfin1_0To1_49
            yellowfin1_5To2_49 = i.yellowfin1_5To2_49
            yellowfin2_5To3_49 = i.yellowfin2_5To3_49
            yellowfin3_5To4_99 = i.yellowfin3_5To4_99
            yellowfin5_0To9_99 = i.yellowfin5_0To9_99
            yellowfin10AndUP = i.yellowfin10AndUP
            bigeye0_300To0_499 = i.bigeye0_300To0_499
            bigeye0_500To0_999 = i.bigeye0_500To0_999
            bigeye1_0To1_49 = i.bigeye1_0To1_49
            bigeye1_5To2_49 = i.bigeye1_5To2_49
            bigeye2_5To3_49 = i.bigeye2_5To3_49
            bigeye3_5To4_99 = i.bigeye3_5To4_99
            bigeye5_0To9_99 = i.bigeye5_0To9_99
            bigeye10AndUP = i.bigeye10AndUP
            bonito0_300To0_499 = i.bonito0_300To0_499
            bonito0_500AndUP = i.bonito0_500AndUP
            fishmeal = i.fishmeal
        Next
    End Sub

    Sub setWS(ByRef ws_ As WeightSlip)
        Me.ws = ws_
    End Sub

    Sub setWS(ByRef tws As trans_WeightSlip)
        ws = New WeightSlip(tws, dc)
    End Sub

End Class
