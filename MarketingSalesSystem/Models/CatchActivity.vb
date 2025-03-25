Public Class CatchActivity
    Public catchActivity_ID As Integer
    Public catchDate As Date
    Public method_ID As Integer
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
            method_ID = i.method_ID
            longitude = i.longitude
            latitude = i.latitude
            catchReferenceNum = i.catchReferenceNum
        Next

    End Sub

    Sub Add()
        Dim ca = New trans_CatchActivity

        With ca
            .catchDate = catchDate
            .method_ID = method_ID
            .longitude = longitude
            .latitude = latitude
            .catchReferenceNum = catchReferenceNum
        End With

        dc.trans_CatchActivities.InsertOnSubmit(ca)
        dc.SubmitChanges()
        Me.catchActivity_ID = ca.catchActivity_ID
    End Sub
End Class
