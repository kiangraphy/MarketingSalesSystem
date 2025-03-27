Imports DevExpress.XtraGrid
Imports DevExpress.XtraLayout
Imports DevExpress.XtraGrid.Views.Grid

Public Class ucCatcher
    Inherits ucBase

    Public grid As GridControl

    Sub New(ByVal title As String)
        InitializeComponent()

        MyBase.title = title
        LabelControl1.Text = title

        loadData()

        AddHandler grid.Load, AddressOf gridLoaded

        dtFrom.EditValue = Nothing
        dtFrom.Properties.MaxValue = Date.Now
        dtTo.EditValue = Nothing
        dtTo.Properties.MaxValue = Date.Now

    End Sub

    Sub loadData()
        grid = New GridControl() With {
            .Dock = DockStyle.Fill
        }

        LayoutControl2.Controls.Add(grid)

        Dim layoutItem As LayoutControlItem = LayoutControl2.AddItem("", grid)
        layoutItem.TextVisible = False
    End Sub

    Sub gridLoaded(sender As Object, e As EventArgs)
        loadGrid()
    End Sub

    Sub loadGrid()
        Dim dc As New mkdbDataContext

        Dim ca = New CatchActivity(dc).getByDate(CDate(dtFrom.EditValue), CDate(dtTo.EditValue)).ToList()
        Dim cad = ca.Join(dc.trans_CatchMethods,
                   Function(c) c.method_ID,
                   Function(j) j.catchMethod_ID,
                   Function(c, j) New With {
                        .catchActivity_ID = c.catchActivity_ID,
                        .CatchReferenceNumber = c.catchReferenceNum,
                        .CatchDate = c.catchDate,
                        .CatchMethod = j.catchMethod,
                        .Longitude = c.longitude,
                        .Latitude = c.latitude
                    }).ToList()


        Dim gridView = New GridView()
        AddHandler gridView.DoubleClick, AddressOf HandleGridDoubleClick
        gridView.GridControl = grid

        grid.MainView = gridView
        grid.ViewCollection.Add(gridView)


        gridView.GridControl.DataSource = cad
        gridView.PopulateColumns()

        gridTransMode(gridView)
    End Sub

    Private Sub HandleGridDoubleClick(sender As Object, e As EventArgs)
        Dim gridView As DevExpress.XtraGrid.Views.Grid.GridView = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        Dim value = gridView.GetRowCellValue(gridView.FocusedRowHandle, "catchActivity_ID")
        Dim ctrlCA = New ctrlCatchers(Me, CInt(value))
    End Sub

    Protected Overrides Function GetGridControl() As GridControl
        Return If(grid IsNot Nothing, grid, Nothing)
    End Function

    Overrides Sub refreshData()
        loadGrid()
    End Sub

    Public Overrides Sub openForm()
        Dim ctrlCA = New ctrlCatchers(Me)
    End Sub

End Class
