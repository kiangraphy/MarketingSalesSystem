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
        dtFrom.Properties.MaxValue = Date.Now
        dtTo.EditValue = Nothing
        dtTo.Properties.MaxValue = Date.Now.AddDays(1)

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
        loadGrid()
    End Sub

    Sub loadGrid()
        Dim gridView = New GridView()
        gridView.GridControl = grid

        AddHandler gridView.DoubleClick, AddressOf HandleGridDoubleClick

        grid.MainView = gridView
        grid.ViewCollection.Add(gridView)

        Dim dc = New mkdbDataContext()
        Dim mdb = New tpmdbDataContext()

        ' Debug.WriteLine(CDate(dtFrom.EditValue))
        ' Debug.WriteLine(CDate(dtTo.EditValue))

        ' Fetch all sales reports in one query
        Dim sales = New SalesReport(dc).getByDate(CDate(dtFrom.EditValue), CDate(dtTo.EditValue)).ToList()
        Dim data = sales.Select(Function(sr) transformData(sr)).ToList

        gridView.GridControl.DataSource = data
        gridView.PopulateColumns()

        gridTransMode(gridView)
    End Sub

    Function transformData(sr As SalesReport) As Object
        Dim dc = New mkdbDataContext()
        Dim mdc = New tpmdbDataContext()

        ' Fetch price only once
        Dim price = dc.trans_SalesReportPrices.FirstOrDefault(Function(i) i.salesReport_ID = sr.salesReport_ID)

        ' Preload catchers to avoid multiple queries inside the loop
        Dim catchers = dc.trans_SalesReportCatchers.
            Where(Function(src) src.salesReport_ID = sr.salesReport_ID).
            Select(Function(src) src.catchActivityDetail_ID).
            Distinct().ToList()

        ' Preload all relevant catch data into a dictionary for quick lookup
        Dim catchDataDict = dc.trans_SalesReportCatchers.
            Where(Function(i) i.salesReport_ID = sr.salesReport_ID).
            GroupBy(Function(i) i.catchActivityDetail_ID).
            ToDictionary(Function(g) g.Key, Function(g) g.ToList())

        ' Initialize counters
        Dim actualQty As Decimal = 0
        Dim fishMeal As Decimal = 0
        Dim spoilage As Decimal = 0
        Dim net As Decimal = 0
        Dim totalAmount As Decimal = 0

        ' Process all catchers efficiently
        For Each catcher In catchers
            If catchDataDict.ContainsKey(catcher) Then
                Dim getCatch = catchDataDict(catcher)

                actualQty += sumFields(getCatch(0))
                fishMeal += getCatch(0).fishmeal
                spoilage += sumFields(getCatch(1))
                net += (actualQty - spoilage)
                totalAmount += calculateTotalAmount(getCatch(0), getCatch(1), price)
            End If
        Next

        ' Load vessel data efficiently
        Dim vesselDict = mdc.ml_Vessels.ToDictionary(Function(v) v.ml_vID, Function(v) v.vesselName)

        Dim vesselIDs = (From src In dc.trans_SalesReportCatchers
                         Join cad In dc.trans_CatchActivityDetails On cad.catchActivityDetail_ID Equals src.catchActivityDetail_ID
                         Join ca In dc.trans_CatchActivities On ca.catchActivity_ID Equals cad.catchActivity_ID
                         Where src.salesReport_ID = sr.salesReport_ID
                         Select New With {
                             cad.vessel_ID,
                             ca.catchReferenceNum
                         }).Distinct().ToList()

        Dim vessels = vesselIDs.Select(Function(id) New With {
            .VesselName = If(vesselDict.ContainsKey(id.vessel_ID), vesselDict(id.vessel_ID), "Unknown")
        }).ToList()

        ' Return the final structured data
        Return New With {
            .salesReport_ID = sr.salesReport_ID,
            .SalesNo = sr.salesNum,
            .CoveredDate = sr.salesDate,
            .Catcher = vesselIDs.First().catchReferenceNum,
            .SellingType = sr.sellingType,
            .Buyer = sr.buyer,
            .UnloadingVessels = vessels,
            .ActualQty = actualQty,
            .Fishmeal = fishMeal,
            .Spoilage = spoilage,
            .NetQty = actualQty - spoilage,
            .SalesInUSD = Math.Round(totalAmount / sr.usdRate, 2),
            .USDRate = sr.usdRate,
            .SalesInPHP = totalAmount,
            .AveragePrice = totalAmount
        }
    End Function

    Function sumFields(record As trans_SalesReportCatcher) As Decimal
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

    Function calculateTotalAmount(actual As trans_SalesReportCatcher, spoilage As trans_SalesReportCatcher, price As trans_SalesReportPrice) As Decimal
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
        Dim ctrlSI = New ctrlSales(Me, CInt(value))
    End Sub


    Protected Overrides Function GetGridControl() As GridControl
        Return If(grid IsNot Nothing, grid, Nothing)
    End Function

    Overrides Sub refreshData()
        loadGrid()
    End Sub

    Public Overrides Sub openForm()
        Dim ctrl = New ctrlSales(Me)
    End Sub


End Class
