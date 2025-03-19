Imports DevExpress.XtraTab

Module modUtils

    Function getServerDate() As Date
        Return Date.Now
    End Function

    Function AddTab(ByRef tabControl As XtraTabControl, ByVal tabTitle As String) As XtraTabPage
        Dim newTab As New XtraTabPage() With {
            .Text = tabTitle
        }
        tabControl.TabPages.Add(newTab)

        Return newTab
    End Function

End Module
