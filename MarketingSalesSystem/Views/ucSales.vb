Imports DevExpress.XtraTab
Imports DevExpress.XtraLayout
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid

Public Class ucSales
    Inherits ucBase

    Private tabControl As XtraTabControl

    Sub New(ByVal title As String)
        InitializeComponent()

        MyBase.title = title
        LabelControl1.Text = title

        tabControl = New XtraTabControl() With {
            .Dock = DockStyle.Fill
        }

        LayoutControl2.Controls.Add(tabControl)

        Dim layoutItem As LayoutControlItem = LayoutControl2.AddItem("", tabControl)
        layoutItem.TextVisible = False ' Hide label

        Dim buyerGrid = setTab(AddTab(tabControl, "View By Buyer"))
        Dim catcherGrid = setTab(AddTab(tabControl, "View By Catcher"))
        displayBuyer(buyerGrid)
        displayCatcher(catcherGrid)
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

    Sub displayBuyer(ByRef grid As GridControl)
        ' Initialize GridView
        Dim gridView As New GridView(grid)
        grid.MainView = gridView
        grid.ViewCollection.Add(gridView)

        ' Enable footer and merge all columns into one
        gridView.OptionsView.ShowFooter = True
        gridView.OptionsView.AllowCellMerge = True

        ' Force GridControl to initialize before setting the data source
        grid.ForceInitialize()

        ' Retrieve Data
        Dim dc = New mkdbDataContext
        Dim sr = New SalesReport(dc).getRows()

        Dim data = From i In sr
                    Select i.salesReport_ID,
                    SalesNo = i.salesNum,
                    CoveredDate = i.salesDate,
                    SellingType = i.sellingType,
                    Buyer = i.buyer,
                    UnloadingVessel = i.unloadingForeignVessel,
                    VesselType = i.unloadingType,
                    ActualQty = "",
                    FishMeal = "",
                    TotalQty = "",
                    Spoilage = "",
                    NetQty = "",
                    SalesInUSD = "",
                    USDRate = i.usdRate,
                    SalesInPHP = ""

        ' Bind the data source
        grid.DataSource = data

        AddHandler gridView.CustomDrawFooter, AddressOf GridView_CustomDrawFooter

    End Sub

    Sub displayCatcher(ByRef grid As GridControl)
        ' Initialize GridView
        Dim gridView As New GridView(grid)
        grid.MainView = gridView
        grid.ViewCollection.Add(gridView)

        ' Enable footer and merge all columns into one
        gridView.OptionsView.ShowFooter = True
        gridView.OptionsView.AllowCellMerge = True

        ' Force GridControl to initialize before setting the data source
        grid.ForceInitialize()

        ' Retrieve Data
        Dim dc = New mkdbDataContext
        Dim sr = New SalesReport(dc).getRows()

        Dim data = From i In sr
                    Select i.salesReport_ID,
                    SalesNo = i.salesNum,
                    CoveredDate = i.salesDate,
                    Catcher = "Human",
                    SellingType = i.sellingType,
                    Buyer = i.buyer,
                    UnloadingVessel = i.unloadingVessel_ID,
                    VesselType = i.unloadingType,
                    ActualQty = "",
                    FishMeal = "",
                    TotalQty = "",
                    Spoilage = "",
                    NetQty = "",
                    SalesInUSD = "",
                    USDRate = i.usdRate,
                    SalesInPHP = ""

        ' Bind the data source
        grid.DataSource = data

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
