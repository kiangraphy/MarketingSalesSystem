Imports DevExpress.XtraTab
Imports DevExpress.XtraLayout
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Public Class frm_salesInvoice

    Private tabControl As XtraTabControl
    Private txtNote As TextBox

    Private ctrlSales As ctrlSales

    Sub New(ByRef ctrlS As ctrlSales)

        InitializeComponent()

        ctrlSales = ctrlS

        tabControl = New XtraTabControl() With {
            .Dock = DockStyle.Fill
        }

        LayoutControl2.Controls.Add(tabControl)

        Dim layoutItem As LayoutControlItem = LayoutControl2.AddItem("", tabControl)
        layoutItem.TextVisible = False ' Hide label

        Dim wsGrid = setTab(AddTab(tabControl, "Weight Slips"))
        Dim adGrid = setTab(AddTab(tabControl, "Amount Details"))
        Dim sGrid = setTab(AddTab(tabControl, "Summary"))

        displayWeightSlips(wsGrid)
        displayAmountDetails(adGrid)
        displaySummary(sGrid)

        ' Initialize Note TextBox
        txtNote = New TextBox() With {
            .Multiline = True,
            .Height = 80,
            .Dock = DockStyle.Bottom
        }
        LayoutControl2.Controls.Add(txtNote)

        ' Add TextBox to layout
        Dim noteLayoutItem As LayoutControlItem = LayoutControl2.AddItem("Note:", txtNote)
        noteLayoutItem.TextVisible = True

    End Sub

    Function setTab(ByRef tab As XtraTabPage) As GridControl
        Dim layout = New LayoutControl With {
                        .Dock = DockStyle.Fill
                    }
        tab.Controls.Add(layout)
        Dim grid = New GridControl() With {
            .Dock = DockStyle.Fill
        }
        layout.Controls.Add(grid)
        Dim layoutItem As LayoutControlItem = layout.AddItem("", grid)
        layoutItem.TextVisible = False

        Return grid
    End Function

    Sub displayWeightSlips(ByRef grid As GridControl)
        ' Initialize GridView
        Dim gridView As New GridView(grid)
        grid.MainView = gridView
        grid.ViewCollection.Add(gridView)

        ' Enable footer and merge all columns into one
        gridView.OptionsView.ShowFooter = True
        gridView.OptionsView.AllowCellMerge = True

        ' Force GridControl to initialize before setting the data source
        grid.ForceInitialize()

        AddHandler gridView.CustomDrawFooter, AddressOf GridView_CustomDrawFooter
    End Sub

    Sub displayAmountDetails(ByRef grid As GridControl)
        ' Initialize GridView
        Dim gridView As New GridView(grid)
        grid.MainView = gridView
        grid.ViewCollection.Add(gridView)

        ' Enable footer and merge all columns into one
        gridView.OptionsView.ShowFooter = True
        gridView.OptionsView.AllowCellMerge = True

        ' Force GridControl to initialize before setting the data source
        grid.ForceInitialize()

        AddHandler gridView.CustomDrawFooter, AddressOf GridView_CustomDrawFooter
    End Sub

    Sub displaySummary(ByRef grid As GridControl)
        ' Initialize GridView
        Dim gridView As New GridView(grid)
        grid.MainView = gridView
        grid.ViewCollection.Add(gridView)

        ' Enable footer and merge all columns into one
        gridView.OptionsView.ShowFooter = True
        gridView.OptionsView.AllowCellMerge = True

        ' Force GridControl to initialize before setting the data source
        grid.ForceInitialize()

        AddHandler gridView.CustomDrawFooter, AddressOf GridView_CustomDrawFooter
    End Sub

    Private Sub GridView_CustomDrawFooter(sender As Object, e As Views.Base.RowObjectCustomDrawEventArgs)
        Dim view As GridView = TryCast(sender, GridView)
        If view Is Nothing Then Exit Sub

        e.DefaultDraw()

        Dim footerRect As Rectangle = e.Bounds

        Dim footerFont As New Font("Arial", 10, FontStyle.Bold)

        Dim drawFormat As New StringFormat()
        drawFormat.LineAlignment = StringAlignment.Center

        e.Graphics.DrawString(" Total:", footerFont, Brushes.Black, footerRect, drawFormat)

        e.Handled = True
    End Sub

End Class