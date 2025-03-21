Imports DevExpress.XtraTab
Imports DevExpress.XtraLayout
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid

Public Class ucSales
    Inherits ucBase

    Private tabControl As XtraTabControl
    Private grid As GridControl

    Sub New(ByVal title As String)
        InitializeComponent()

        MyBase.title = title
        LabelControl1.Text = title

        loadData()

        AddHandler grid.Load, AddressOf gridLoaded

    End Sub

    Sub loadData()
        grid = New GridControl() With {
            .Dock = DockStyle.Fill
        }

        LayoutControl2.Controls.Add(grid)

        Dim layoutItem As LayoutControlItem = LayoutControl2.AddItem("", grid)
        layoutItem.TextVisible = False
    End Sub

    Private Sub gridLoaded(sender As Object, e As EventArgs)
        Dim gridView = New GridView()
        gridView.GridControl = grid

        grid.MainView = gridView
        grid.ViewCollection.Add(gridView)

        Dim dc = New mkdbDataContext
        Dim mdb = New tpmdbDataContext

        Dim sales = New SalesReport(dc).getRows()
        Dim salesPrice = (From i In dc.trans_SalesReportPrices).ToList()
        Dim vessel = (From i In mdb.ml_Vessels Select i).ToList()

        Dim salesData = From sr In sales Join
                        v In vessel On sr.unloadingVessel_ID Equals v.ml_vID Join
                        sp In salesPrice On sr.salesReport_ID Equals sp.salesReport_ID
                        Select sr.salesReport_ID,
                        SalesNo = sr.salesNum,
                        CoveredDate = sr.salesDate,
                        Catcher = sr.catchtDeliveryNum,
                        SellingType = sr.sellingType,
                        Buyer = sr.buyer,
                        UnloadingVessel = v.vesselName,
                        ActualQty = "null",
                        Fishmeal = sp.fishmeal,
                        Total = "Unknown",
                        Spoilage = "Nan",
                        NetQty = "Nan",
                        SalesInUSD = "Nan",
                        USDRate = sr.usdRate,
                        SalesInPHP = "Nan",
                        AveragePrice = "Nan"

        gridView.GridControl.DataSource = salesData
        gridView.PopulateColumns()

        gridTransMode(gridView)
    End Sub

End Class
