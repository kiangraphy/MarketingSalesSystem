Public Class CatchActivityDetail
    Public catchActivityDetail_ID As Integer
    Public catcherActivity_ID As Integer
    Public vessel_ID As Integer

    Private dc As mkdbDataContext

    Sub New(ByRef dc_ As mkdbDataContext)
        dc = dc_
    End Sub

    Sub New(ByVal catchActivityDetailID As Integer, ByRef dc_ As mkdbDataContext)
        dc = dc_

        Dim e = From i In dc.trans_CatchActivityDetails Where i.catchActivityDetail_ID = catchActivityDetailID Select i

        For Each i In e
            Me.catchActivityDetail_ID = catchActivityDetailID
            catcherActivity_ID = i.catcherActivity_ID
            vessel_ID = i.vessel_ID
        Next
    End Sub
End Class
