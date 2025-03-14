Imports System.ComponentModel
Imports System.Text


Partial Public Class frmMain

    Public ucList As New List(Of ucBase)

    Shared Sub New()
        DevExpress.UserSkins.BonusSkins.Register()
    End Sub

    Sub New()
        InitializeComponent()
    End Sub

    Private Sub addTab(ByVal uc As ucBase, ByVal title As String)
        ucList.Add(uc)
        xtraTab.TabPages.Add(uc.title)
        uc.Parent = xtraTab.TabPages(xtraTab.TabPages.Count - 1)
        uc.Dock = DockStyle.Fill
        xtraTab.TabPages(ucList.IndexOf(uc)).Show()
    End Sub

    Private Function InTabs(title As String) As Integer
        If ucList.Count = 0 OrElse Not ucList.Any(Function(x) x.title = title) Then
            Return -1
        End If
        Return ucList.FindIndex(Function(x) x.title = title)
    End Function

    Private Sub btn_salesInvc_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btn_salesInvc.ItemClick
        Dim title = "Sales Invoice"
        Dim intab = InTabs(title)
        If intab = -1 Then
            Dim uc As New ucSalesInvc(title)
            addTab(uc, title)
        Else
            xtraTab.TabPages(intab).Show()
        End If
    End Sub

    Private Sub xtraTab_CloseButtonClick(sender As Object, e As EventArgs)
        Dim page As DevExpress.XtraTab.XtraTabPage = TryCast(xtraTab.SelectedTabPage, DevExpress.XtraTab.XtraTabPage)

        If page IsNot Nothing Then
            Dim ucToRemove = ucList.FirstOrDefault(Function(uc) uc.Parent Is page)

            If ucToRemove IsNot Nothing Then
                ucList.Remove(ucToRemove)
            End If

            xtraTab.TabPages.Remove(page)
        End If
    End Sub

    Private Sub btn_weightSlips_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btn_weightSlips.ItemClick
        Dim frm2 As New frmSI_weightSlips

        frm2.Show()
    End Sub

    Private Sub btn_amountDetails_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btn_amountDetails.ItemClick
        Dim frm3 As New frmSI_amountDetails

        frm3.Show()
    End Sub

    Private Sub btn_summary_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btn_summary.ItemClick
        Dim frm3 As New frmSI_summary

        frm3.Show()
    End Sub
End Class
