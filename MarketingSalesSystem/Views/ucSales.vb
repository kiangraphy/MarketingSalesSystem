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
                    UnloadingVesser = i.unloadingForeignVessel,
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

    End Sub

    Sub displayCatcher(ByRef grid As GridControl)
        ' Initialize GridView
        Dim gridView As New GridView(grid)
        grid.MainView = gridView
        grid.ViewCollection.Add(gridView)

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
                    UnloadingVesser = i.unloadingVessel_ID,
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
    End Sub

End Class
