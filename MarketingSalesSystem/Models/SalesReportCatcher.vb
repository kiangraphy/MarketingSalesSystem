Public Class SalesReportCatcher
    Public salesReportCatcher_ID As Integer
    Public salesReport_ID As Integer
    Public catchActivityDetail_ID As Integer
    Public skipjack0_300To0_499 As Decimal
    Public skipjack0_500To0_999 As Decimal
    Public skipjack1_0To1_39 As Decimal
    Public skipjack1_4To1_79 As Decimal
    Public skipjack1_8To2_49 As Decimal
    Public skipjack2_5To3_49 As Decimal
    Public skipjack3_5AndUP As Decimal
    Public yellowfin0_300To0_499 As Decimal
    Public yellowfin0_500To0_999 As Decimal
    Public yellowfin1_0To1_49 As Decimal
    Public yellowfin1_5To2_49 As Decimal
    Public yellowfin2_5To3_49 As Decimal
    Public yellowfin3_5To4_99 As Decimal
    Public yellowfin5_0To9_99 As Decimal
    Public yellowfin10AndUpGood As Decimal
    Public yellowfin10AndUpDeformed As Decimal
    Public bigeye0_500To0_999 As Decimal
    Public bigeye1_0To1_49 As Decimal
    Public bigeye1_5To2_49 As Decimal
    Public bigeye2_5To3_49 As Decimal
    Public bigeye3_5To4_99 As Decimal
    Public bigeye5_0To9_99 As Decimal
    Public bigeye10AndUP As Decimal
    Public bonito As Decimal
    Public fishmeal As Decimal

    Private dc As mkdbDataContext

    Sub New(ByRef dc_ As mkdbDataContext)
        dc = dc_
    End Sub

    Sub New(ByVal salesReportCatcherID As Integer, ByRef dc_ As mkdbDataContext)
        dc = dc_

        Dim e = From i In dc.trans_SalesReportCatchers Where i.salesReportCatcher_ID = salesReportCatcherID Select i

        For Each i In e
            Me.salesReportCatcher_ID = salesReportCatcherID
            salesReport_ID = i.salesReport_ID
            catchActivityDetail_ID = i.catchActivityDetail_ID
            skipjack0_300To0_499 = i.skipjack0_300To0_499
            skipjack0_500To0_999 = i.skipjack0_500To0_999
            skipjack1_0To1_39 = i.skipjack1_0To1_39
            skipjack1_4To1_79 = i.skipjack1_4To1_79
            skipjack1_8To2_49 = i.skipjack1_8To2_49
            skipjack2_5To3_49 = i.skipjack2_5To3_49
            skipjack3_5AndUP = i.skipjack3_5AndUP
            yellowfin0_300To0_499 = i.yellowfin0_300To0_499
            yellowfin0_500To0_999 = i.yellowfin0_500To0_999
            yellowfin1_0To1_49 = i.yellowfin1_0To1_49
            yellowfin1_5To2_49 = i.yellowfin1_5To2_49
            yellowfin2_5To3_49 = i.yellowfin2_5To3_49
            yellowfin3_5To4_99 = i.yellowfin3_5To4_99
            yellowfin5_0To9_99 = i.yellowfin5_0To9_99
            yellowfin10AndUpGood = i.yellowfin10AndUpGood
            yellowfin10AndUpDeformed = i.yellowfin10AndUpDeformed
            bigeye0_500To0_999 = i.bigeye0_500To0_999
            bigeye1_0To1_49 = i.bigeye1_0To1_49
            bigeye1_5To2_49 = i.bigeye1_5To2_49
            bigeye2_5To3_49 = i.bigeye2_5To3_49
            bigeye3_5To4_99 = i.bigeye3_5To4_99
            bigeye5_0To9_99 = i.bigeye5_0To9_99
            bigeye10AndUP = i.bigeye10AndUP
            bonito = i.bonito
            fishmeal = i.fishmeal
        Next
    End Sub
End Class
