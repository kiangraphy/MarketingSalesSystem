Public Class CatchMethod
    Public catchMethod_ID As Integer
    Public catchMethod As String
    Public active As Boolean?
    Public modifiedDate As Date?
    Public modifiedBy As Date?

    Private dc As mkdbDataContext

    Sub New(ByRef dc_ As mkdbDataContext)
        dc = dc_
    End Sub

    Sub New(ByVal catchMethodID As Integer, ByRef dc_ As mkdbDataContext)
        dc = dc_

        Dim e = From i In dc.trans_CatchMethods Where i.catchMethod_ID = catchMethodID Select i

        For Each i In e
            Me.catchMethod_ID = catchMethodID
            catchMethod = i.catchMethod
            active = i.active
            modifiedDate = i.modifiedDate
            modifiedBy = i.modifiedBy
        Next
    End Sub
End Class
