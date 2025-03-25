Public Class CatchActivity
    Public catchActivity_ID As Integer
    Public catchDate As Date
    Public method As String
    Public longitude As Decimal
    Public latitude As Decimal
    Public catchReferenceNum As String

    Private dc As mkdbDataContext

    Sub New(ByRef dc_ As mkdbDataContext)
        dc = dc_
    End Sub

    Sub New(ByVal catchActivityID As Integer, ByRef dc_ As mkdbDataContext)
        dc = dc_

        Dim e = From i In dc.trans_CatchActivities Where i.catchActivity_ID = catchActivityID Select i

        For Each i In e
            Me.catchActivity_ID = catchActivityID
            catchDate = i.catchDate
            method = i.method
            longitude = i.longitude
            latitude = i.latitude
            catchReferenceNum = i.catchReferenceNum
        Next

    End Sub
End Class
