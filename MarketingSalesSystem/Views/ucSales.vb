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

        dtFrom.EditValue = Nothing
        dtTo.EditValue = Nothing

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

        AddHandler gridView.DoubleClick, AddressOf HandleGridDoubleClick

        grid.MainView = gridView
        grid.ViewCollection.Add(gridView)

        Dim dc = New mkdbDataContext()
        Dim mdb = New tpmdbDataContext()

        ' Fetch all sales reports in one query
        Dim sales = New SalesReport(dc).getRows().ToList()

        ' Fetch only necessary columns for sales price and vessel
        Dim salesPriceDict = dc.trans_SalesReportPrices.Where(Function(sp) sales.Select(Function(s) s.salesReport_ID).Contains(sp.salesReport_ID)).ToList()
        Dim vesselDict = mdb.ml_Vessels.ToDictionary(Function(v) v.ml_vID, Function(v) v.vesselName)

        ' Process sales data efficiently
        Dim salesData = sales.Select(Function(sr)
                                         Dim spList = salesPriceDict.Where(Function(sp) sp.salesReport_ID = sr.salesReport_ID).ToList()

                                         If spList.Count < 1 Then
                                             Return Nothing
                                         End If

                                         Dim price = spList(0)
                                         Dim actualCatcher1 = spList(1)
                                         Dim actualCatcher2 = spList(2)
                                         Dim spoilageCatcher1 = spList(3)
                                         Dim spoilageCatcher2 = spList(4)

                                         ' Compute all values before assigning them to avoid repeated calculations
                                         Dim actualQty = sumFields(actualCatcher1) + sumFields(actualCatcher2)
                                         Dim spoilage = sumFields(spoilageCatcher1) + sumFields(spoilageCatcher2)
                                         Dim netQty = actualQty - spoilage
                                         Dim totalAmount = calculateTotalAmount(actualCatcher1, spoilageCatcher1, price) + calculateTotalAmount(actualCatcher2, spoilageCatcher2, price)
                                         Dim usdRate = sr.usdRate
                                         Dim salesInUSD = If(usdRate > 0, Math.Round(totalAmount / usdRate, 2), 0)

                                         Return New With {
                                             .salesReport_ID = sr.salesReport_ID,
                                             .SalesNo = sr.salesNum,
                                             .CoveredDate = sr.salesDate,
                                             .Catcher = sr.catchtDeliveryNum,
                                             .SellingType = sr.sellingType,
                                             .Buyer = sr.buyer,
                                             .UnloadingVessel = If(vesselDict.ContainsKey(sr.unloadingVessel_ID.GetValueOrDefault()), vesselDict(sr.unloadingVessel_ID.GetValueOrDefault()), "Unknown"),
                                             .ActualQty = actualQty,
                                             .Fishmeal = actualCatcher1.fishmeal + actualCatcher2.fishmeal,
                                             .Spoilage = spoilage,
                                             .NetQty = netQty,
                                             .SalesInUSD = salesInUSD,
                                             .USDRate = usdRate,
                                             .SalesInPHP = totalAmount,
                                             .AveragePrice = totalAmount
                                         }
                                     End Function).Where(Function(x) x IsNot Nothing).ToList()

        gridView.GridControl.DataSource = salesData
        gridView.PopulateColumns()

        gridTransMode(gridView)
    End Sub
    Function sumFields(record As trans_SalesReportPrice) As Decimal
        Return record.skipjack0_300To0_499 + record.skipjack0_500To0_999 +
               record.skipjack1_0To1_39 + record.skipjack1_4To1_79 +
               record.skipjack1_8To2_49 + record.skipjack2_5To3_49 +
               record.skipjack3_5AndUP + record.yellowfin0_300To0_499 +
               record.yellowfin0_500To0_999 + record.yellowfin1_0To1_49 +
               record.yellowfin1_5To2_49 + record.yellowfin2_5To3_49 +
               record.yellowfin3_5To4_99 + record.yellowfin5_0To9_99 +
               record.yellowfin10AndUpGood + record.yellowfin10AndUpDeformed +
               record.bigeye0_500To0_999 + record.bigeye1_0To1_49 +
               record.bigeye1_5To2_49 + record.bigeye2_5To3_49 +
               record.bigeye3_5To4_99 + record.bigeye5_0To9_99 +
               record.bigeye10AndUP + record.bonito + record.fishmeal
    End Function

    Function calculateTotalAmount(actual As trans_SalesReportPrice, spoilage As trans_SalesReportPrice, price As trans_SalesReportPrice) As Decimal
        Dim total As Decimal = 0

        total += (actual.skipjack0_300To0_499 - spoilage.skipjack0_300To0_499) * price.skipjack0_300To0_499
        total += (actual.skipjack0_500To0_999 - spoilage.skipjack0_500To0_999) * price.skipjack0_500To0_999
        total += (actual.skipjack1_0To1_39 - spoilage.skipjack1_0To1_39) * price.skipjack1_0To1_39
        total += (actual.skipjack1_4To1_79 - spoilage.skipjack1_4To1_79) * price.skipjack1_4To1_79
        total += (actual.skipjack1_8To2_49 - spoilage.skipjack1_8To2_49) * price.skipjack1_8To2_49
        total += (actual.skipjack2_5To3_49 - spoilage.skipjack2_5To3_49) * price.skipjack2_5To3_49
        total += (actual.skipjack3_5AndUP - spoilage.skipjack3_5AndUP) * price.skipjack3_5AndUP
        total += (actual.yellowfin0_300To0_499 - spoilage.yellowfin0_300To0_499) * price.yellowfin0_300To0_499
        total += (actual.yellowfin0_500To0_999 - spoilage.yellowfin0_500To0_999) * price.yellowfin0_500To0_999
        total += (actual.yellowfin1_0To1_49 - spoilage.yellowfin1_0To1_49) * price.yellowfin1_0To1_49
        total += (actual.yellowfin1_5To2_49 - spoilage.yellowfin1_5To2_49) * price.yellowfin1_5To2_49
        total += (actual.yellowfin2_5To3_49 - spoilage.yellowfin2_5To3_49) * price.yellowfin2_5To3_49
        total += (actual.yellowfin3_5To4_99 - spoilage.yellowfin3_5To4_99) * price.yellowfin3_5To4_99
        total += (actual.yellowfin5_0To9_99 - spoilage.yellowfin5_0To9_99) * price.yellowfin5_0To9_99
        total += (actual.yellowfin10AndUpGood - spoilage.yellowfin10AndUpGood) * price.yellowfin10AndUpGood
        total += (actual.yellowfin10AndUpDeformed - spoilage.yellowfin10AndUpDeformed) * price.yellowfin10AndUpDeformed
        total += (actual.bigeye0_500To0_999 - spoilage.bigeye0_500To0_999) * price.bigeye0_500To0_999
        total += (actual.bigeye1_0To1_49 - spoilage.bigeye1_0To1_49) * price.bigeye1_0To1_49
        total += (actual.bigeye1_5To2_49 - spoilage.bigeye1_5To2_49) * price.bigeye1_5To2_49
        total += (actual.bigeye2_5To3_49 - spoilage.bigeye2_5To3_49) * price.bigeye2_5To3_49
        total += (actual.bigeye3_5To4_99 - spoilage.bigeye3_5To4_99) * price.bigeye3_5To4_99
        total += (actual.bigeye5_0To9_99 - spoilage.bigeye5_0To9_99) * price.bigeye5_0To9_99
        total += (actual.bigeye10AndUP - spoilage.bigeye10AndUP) * price.bigeye10AndUP
        total += (actual.bonito - spoilage.bonito) * price.bonito
        total += (actual.fishmeal - spoilage.fishmeal) * price.fishmeal

        Return Math.Round(total, 2)
    End Function

    Private Sub HandleGridDoubleClick(sender As Object, e As EventArgs)
        Dim gridView As DevExpress.XtraGrid.Views.Grid.GridView = TryCast(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        Dim value = gridView.GetRowCellValue(gridView.FocusedRowHandle, "salesReport_ID")
        Dim ctrlSI = New ctrlSales(CInt(value))
    End Sub


    Protected Overrides Function GetGridControl() As GridControl
        Return If(grid IsNot Nothing, grid, Nothing)
    End Function

    Public Overrides Sub refreshData()
        Dim gridView = New GridView()
        gridView.GridControl = grid

        AddHandler gridView.DoubleClick, AddressOf HandleGridDoubleClick

        grid.MainView = gridView
        grid.ViewCollection.Add(gridView)

        Dim dc = New mkdbDataContext()
        Dim mdb = New tpmdbDataContext()

        ' Fetch all sales reports in one query
        Dim sales = New SalesReport(dc).getByDate(CDate(dtFrom.EditValue), CDate(dtTo.EditValue)).ToList()

        ' Fetch only necessary columns for sales price and vessel
        Dim salesPriceDict = dc.trans_SalesReportPrices.Where(Function(sp) sales.Select(Function(s) s.salesReport_ID).Contains(sp.salesReport_ID)).ToList()
        Dim vesselDict = mdb.ml_Vessels.ToDictionary(Function(v) v.ml_vID, Function(v) v.vesselName)

        ' Process sales data efficiently
        Dim salesData = sales.Select(Function(sr)
                                         Dim spList = salesPriceDict.Where(Function(sp) sp.salesReport_ID = sr.salesReport_ID).ToList()

                                         If spList.Count < 1 Then
                                             Return Nothing
                                         End If

                                         Dim price = spList(0)
                                         Dim actualCatcher1 = spList(1)
                                         Dim actualCatcher2 = spList(2)
                                         Dim spoilageCatcher1 = spList(3)
                                         Dim spoilageCatcher2 = spList(4)

                                         ' Compute all values before assigning them to avoid repeated calculations
                                         Dim actualQty = sumFields(actualCatcher1) + sumFields(actualCatcher2)
                                         Dim spoilage = sumFields(spoilageCatcher1) + sumFields(spoilageCatcher2)
                                         Dim netQty = actualQty - spoilage
                                         Dim totalAmount = calculateTotalAmount(actualCatcher1, spoilageCatcher1, price) + calculateTotalAmount(actualCatcher2, spoilageCatcher2, price)
                                         Dim usdRate = sr.usdRate
                                         Dim salesInUSD = If(usdRate > 0, Math.Round(totalAmount / usdRate, 2), 0)

                                         Return New With {
                                             .salesReport_ID = sr.salesReport_ID,
                                             .SalesNo = sr.salesNum,
                                             .CoveredDate = sr.salesDate,
                                             .Catcher = sr.catchtDeliveryNum,
                                             .SellingType = sr.sellingType,
                                             .Buyer = sr.buyer,
                                             .UnloadingVessel = If(vesselDict.ContainsKey(sr.unloadingVessel_ID.GetValueOrDefault()), vesselDict(sr.unloadingVessel_ID.GetValueOrDefault()), "Unknown"),
                                             .ActualQty = actualQty,
                                             .Fishmeal = actualCatcher1.fishmeal + actualCatcher2.fishmeal,
                                             .Spoilage = spoilage,
                                             .NetQty = netQty,
                                             .SalesInUSD = salesInUSD,
                                             .USDRate = usdRate,
                                             .SalesInPHP = totalAmount,
                                             .AveragePrice = totalAmount
                                         }
                                     End Function).Where(Function(x) x IsNot Nothing).ToList()

        gridView.GridControl.DataSource = salesData
        gridView.PopulateColumns()

        gridTransMode(gridView)
    End Sub


End Class
