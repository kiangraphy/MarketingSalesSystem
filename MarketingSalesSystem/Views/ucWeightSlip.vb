Imports DevExpress.XtraTab
Imports DevExpress.XtraGrid
Imports DevExpress.XtraLayout
Imports DevExpress.XtraGrid.Views.BandedGrid

Public Class ucWeightSlip
    Inherits ucBase

    Private tabControl As XtraTabControl
    Private grid As GridControl

    Sub New(ByVal title As String)
        InitializeComponent()

        MyBase.title = title
        LabelControl1.Text = title

        createGrid()
        loadData()
    End Sub

    Sub loadData()
        Dim dc = New mkdbDataContext
        Dim ws = New WeightSlip(dc).getRows()
        Dim wsd = (From i In dc.trans_WeightSlipDetails Select i).ToList()

        Dim wsList = From i In ws
                     Join s In wsd On i.weightSlip_ID Equals s.weightSlip_ID
                     Select
                     i.weightSlip_ID,
                     dates = s.weightSlipDetailDate,
                     wsno = s.weigslipFormNum,
                     batchno = s.batchNum,
                     lotno = s.lotNum,
                     plateno = s.plateNum,
                     sjkilo1 = CDec(s.skipjack0_200To0_299),
                     sjkilo2 = CDec(s.skipjack0_300To0_499),
                     sjkilo3 = CDec(s.skipjack0_500To0_999),
                     sjkilo4 = CDec(s.skipjack1_0To1_39),
                     sjkilo5 = CDec(s.skipjack1_4To1_79),
                     sjkilo6 = CDec(s.skipjack1_8To2_49),
                     sjkilo7 = CDec(s.skipjack2_5To3_49),
                     sjkilo8 = CDec(s.skipjack3_5AndUP),
                     yfkilo1 = CDec(s.yelllowfin0_300To0_499),
                     yfkilo2 = CDec(s.yellowfin0_500To0_999),
                     yfkilo3 = CDec(s.yellowfin1_0To1_49),
                     yfkilo4 = CDec(s.yellowfin1_5To2_49),
                     yfkilo5 = CDec(s.yellowfin2_5To3_49),
                     yfkilo6 = CDec(s.yellowfin3_5To4_99),
                     yfkilo7 = CDec(s.yellowfin5_0To9_99),
                     yfkilo8 = CDec(s.yellowfin10AndUP),
                     bekilo1 = CDec(s.bigeye0_300To0_499),
                     bekilo2 = CDec(s.bigeye0_500To0_999),
                     bekilo3 = CDec(s.bigeye1_0To1_49),
                     bekilo4 = CDec(s.bigeye1_5To2_49),
                     bekilo5 = CDec(s.bigeye2_5To3_49),
                     bekilo6 = CDec(s.bigeye3_5To4_99),
                     bekilo7 = CDec(s.bigeye5_0To9_99),
                     bekilo8 = CDec(s.bigeye10AndUP),
                     bonkilo1 = CDec(s.bonito0_300To0_499),
                     bonkilo2 = CDec(s.bonito0_500AndUP),
                     fm = s.fishmeal


        grid.DataSource = wsList
    End Sub

    Sub createGrid()
        grid = New GridControl() With {
            .Dock = DockStyle.Fill
        }

        LayoutControl2.Controls.Add(grid)

        Dim layoutItem = LayoutControl2.AddItem("", grid)

        Dim bandView = New BandedGridView(grid) With {
            .ScrollStyle = Views.Grid.ScrollStyleFlags.LiveHorzScroll,
            .HorzScrollVisibility = Views.Base.ScrollVisibility.Always}

        grid.MainView = bandView
        grid.ViewCollection.Add(bandView)

        Dim bandDate = createBand("Date", bandView)
        addColumn("dates", "", bandDate, bandView)

        Dim bandWSNo = createBand("WS No.", bandView)
        addColumn("wsno", "", bandWSNo, bandView)

        Dim bandBatchNo = createBand("Batch No.", bandView)
        addColumn("batchno", "", bandBatchNo, bandView)

        Dim bandLotNo = createBand("Lot No.", bandView)
        addColumn("lotno", "", bandLotNo, bandView)

        Dim bandPlateNo = createBand("Plate No.", bandView)
        addColumn("plateno", "", bandPlateNo, bandView)

        Dim bandSJ = createBand("Skip Jack", bandView)
        createSubBand(New String() {
                    "0.200 - 0.299", "0.300 - 0.499", "0.500 - 0.999",
                    "1.0 - 1.39", "1.4 - 1.79", "1.8 - 2.49", "2.5 - 3.49", "3.5 - UP"}, "sj", bandSJ, bandView)

        Dim bandYF = createBand("Yellow Fin", bandView)
        createSubBand(New String() {
                    "0.300 - 0.499", "0.500 - 0.999", "1.0 - 1.49",
                    "1.5 - 2.49", "2.5 - 3.49", "3.5 - 4.99", "5.0 - 9.99", "10 - UP"}, "yf", bandYF, bandView)

        Dim bandBE = createBand("Big Eye", bandView)
        createSubBand(New String() {
                    "0.300 - 0.499", "0.500 - 0.999", "1.0 - 1.49",
                    "1.5 - 2.49", "2.5 - 3.49", "3.5 - 4.99", "5.0 - 9.99", "10 - UP"}, "be", bandBE, bandView)

        Dim bandBon = createBand("Bonito", bandView)
        createSubBand(New String() {"0.300 - 0.499", "0.500 - UP"}, "bon", bandBon, bandView)

        Dim bandFM = createBand("FM", bandView)
        addColumn("fm", "", bandFM, bandView)

        bandView.OptionsView.ShowColumnHeaders = False
        bandView.OptionsView.ColumnAutoWidth = False
        For Each col As BandedGridColumn In bandView.Columns
            col.BestFit()
            col.Width = Math.Max(col.Width, 100)
        Next
    End Sub

    Function createBand(ByVal caption As String, ByRef bandView As BandedGridView) As GridBand
        Dim band As New GridBand() With {
            .Caption = caption
        }
        bandView.Bands.Add(band)
        Return band
    End Function

    Sub createSubBand(ByVal names As Array, ByVal colName As String, ByRef parentBand As GridBand, ByRef bandView As BandedGridView)
        Dim count = 1
        For Each item In names
            Dim band = createBand(item.ToString, bandView)
            parentBand.Children.Add(band)
            addColumn(colName & "kilo" & count, item.ToString, band, bandView)
            count += 1
        Next
    End Sub

    Sub addColumn(ByVal colName As String, ByVal colCaption As String, ByRef band As GridBand, ByRef bandView As BandedGridView)
        Debug.WriteLine(colName)
        Dim colBand As BandedGridColumn = TryCast(bandView.Columns.AddVisible(colName, colCaption), BandedGridColumn)
        colBand.OwnerBand = band
    End Sub


End Class
