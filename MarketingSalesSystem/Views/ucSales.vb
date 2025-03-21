Imports DevExpress.XtraTab
Imports DevExpress.XtraLayout
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid

Public Class ucSales
    Inherits ucBase

    Private tabControl As XtraTabControl
    Private grid As GridControl
    Private gridView As GridView

    Sub New(ByVal title As String)
        InitializeComponent()

        MyBase.title = title
        LabelControl1.Text = title

        loadData()

        AddHandler grid.Load, AddressOf gridLoaded

        dtFrom.EditValue = Nothing
        dtTo.EditValue = Nothing

    End Sub

    Sub loadData()
        grid = New GridControl() With {
            .Dock = DockStyle.Fill
        }

        gridView = New GridView()
        grid.MainView = gridView
        grid.ViewCollection.Add(gridView)
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
        Dim salesPriceSummary = (From i In dc.trans_SalesReportSummaries).ToList()
        Dim vessel = (From i In mdb.ml_Vessels Select i).ToList()

        Dim salesData = From sr In sales Join
                        v In vessel On sr.unloadingVessel_ID Equals v.ml_vID Join
                        sp In salesPrice On sr.salesReport_ID Equals sp.salesReport_ID Join
                        sps In salesPriceSummary On sr.salesReport_ID Equals sps.salesReport_ID
                        Select sr.salesReport_ID,
                        SalesNo = sr.salesNum,
                        CoveredDate = sr.salesDate,
                        Catcher = sr.catchtDeliveryNum,
                        SellingType = sr.sellingType,
                        Buyer = sr.buyer,
                        UnloadingVessel = v.vesselName,
                        ActualQty = sps.actualQtyInKilos,
                        Fishmeal = sp.fishmeal,
                        Total = "Unknown",
                        Spoilage = sps.spoilageInAmount,
                        NetQty = sps.actualQtyInAmount,
                        SalesInUSD = sps.actualQtyInAmount * sr.usdRate,
                        USDRate = sr.usdRate,
                        SalesInPHP = sps.actualQtyInAmount,
                        AveragePrice = sps.actualQtyInAmount

        gridView.GridControl.DataSource = salesData
        gridView.PopulateColumns()

        gridTransMode(gridView)
    End Sub

    Protected Overrides Function GetGridControl() As GridControl
        Return If(grid IsNot Nothing, grid, Nothing)
    End Function

    Public Overrides Sub refreshData()
        Try
            ' Initialize data context
            Dim dc As New mkdbDataContext
            Dim mdb As New tpmdbDataContext

            ' Fetch data for sales, sales price, summary, and vessels
            Dim sales = New SalesReport(dc).getRows()
            Dim salesPrice = (From i In dc.trans_SalesReportPrices).ToList()
            Dim salesPriceSummary = (From i In dc.trans_SalesReportSummaries).ToList()
            Dim vessel = (From i In mdb.ml_Vessels Select i).ToList()

            ' Construct query with filtering if dtFrom and dtTo are set
            Dim salesData = From sr In sales
                            Join v In vessel On sr.unloadingVessel_ID Equals v.ml_vID
                            Join sp In salesPrice On sr.salesReport_ID Equals sp.salesReport_ID
                            Join sps In salesPriceSummary On sr.salesReport_ID Equals sps.salesReport_ID
                            Select New With {
                                .SalesReportID = sr.salesReport_ID,
                                .SalesNo = sr.salesNum,
                                .CoveredDate = sr.salesDate,
                                .Catcher = sr.catchtDeliveryNum,
                                .SellingType = sr.sellingType,
                                .Buyer = sr.buyer,
                                .UnloadingVessel = v.vesselName,
                                .ActualQty = sps.actualQtyInKilos,
                                .Fishmeal = sp.fishmeal,
                                .Total = "Unknown",
                                .Spoilage = sps.spoilageInAmount,
                                .NetQty = sps.actualQtyInAmount,
                                .SalesInUSD = sps.actualQtyInAmount * sr.usdRate,
                                .USDRate = sr.usdRate,
                                .SalesInPHP = sps.actualQtyInAmount,
                                .AveragePrice = sps.actualQtyInAmount
                            }

            ' Apply date filtering only if dtFrom and dtTo have values
            If dtFrom IsNot Nothing AndAlso dtTo IsNot Nothing AndAlso dtFrom.EditValue IsNot Nothing AndAlso dtTo.EditValue IsNot Nothing Then
                Dim fromDate As Date = CDate(dtFrom.EditValue).Date
                Dim toDate As Date = CDate(dtTo.EditValue).Date.AddDays(1)
                salesData = salesData.Where(Function(i) i.CoveredDate >= fromDate AndAlso i.CoveredDate < toDate)
            End If

            ' Convert query to list and bind to grid
            Dim ds_db = salesData.ToList()
            grid.DataSource = ds_db ' Use the grid control in ucSales

            ' Apply grid transformations
            Dim gridView As GridView = CType(grid.MainView, GridView)
            gridTransMode(gridView)

            ' Hide SalesReportID column if it exists
            If gridView.Columns("SalesReportID") IsNot Nothing Then
                gridView.Columns("SalesReportID").Visible = False
            End If

            ' Update grid count
            gvCount(gridView)
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


End Class
