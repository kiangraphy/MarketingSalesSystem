Imports DevExpress.XtraTab
Imports DevExpress.XtraLayout
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.BandedGrid

Public Class frm_salesInvoice

    Public dt As DataTable
    Private tabControl As XtraTabControl
    Private txtNote As TextBox

    Private ctrlSales As ctrlSales

    Sub New(ByRef ctrlS As ctrlSales)

        InitializeComponent()

        ctrlSales = ctrlS

    End Sub

    Private Sub cmbUVT_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbUVT.SelectedIndexChanged
        ctrlSales.createCollectionVessel(CInt(cmbUVT.SelectedIndex) + 1)
    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioGroup1.SelectedIndexChanged
        If RadioGroup1.SelectedIndex = 1 Then
            ctrlSales.changeBuyerInput()
        Else
            ctrlSales.changeBuyerCombo()
        End If
    End Sub


    Private Sub GridControl1_Load(sender As Object, e As EventArgs) Handles GridControl1.Load

        Dim bandClass = AddBand("Class", BandedGridView1)
        Dim bandSize = AddBand("Size", BandedGridView1)
        Dim bandPrice = AddBand("Price", BandedGridView1)
        Dim bandAU = AddBand("Actual Unloading", BandedGridView1)
        Dim bandSpoilage = AddBand("Spoilage", BandedGridView1)
        Dim bandNet = AddBand("Net", BandedGridView1)

        Dim bandAUKilos = AddBand("Kilos", bandAU)
        Dim bandAUAmount = AddBand("Amount", bandAU)
        Dim bandAUKCatcher1 = AddBand("Catcher 1", bandAUKilos)
        Dim bandAUKCatcher2 = AddBand("Catcher 2", bandAUKilos)
        Dim bandAUKTotal = AddBand("Total", bandAUKilos)
        Dim bandAUACatcher1 = AddBand("Catcher 1", bandAUAmount)
        Dim bandAUACatcher2 = AddBand("Catcher 2", bandAUAmount)
        Dim bandAUATotal = AddBand("Total", bandAUAmount)

        Dim bandSKilos = AddBand("Kilos", bandSpoilage)
        Dim bandSAmount = AddBand("Amount", bandSpoilage)
        Dim bandSKCatcher1 = AddBand("Catcher 1", bandSKilos)
        Dim bandSKCatcher2 = AddBand("Catcher 2", bandSKilos)
        Dim bandSKTotal = AddBand("Total", bandSKilos)
        Dim bandSACatcher1 = AddBand("Catcher 1", bandSAmount)
        Dim bandSACatcher2 = AddBand("Catcher 2", bandSAmount)
        Dim bandSATotal = AddBand("Total", bandSAmount)

        Dim bandNKilos = AddBand("Kilos", bandNet)
        Dim bandNAmount = AddBand("Amount", bandNet)
        Dim bandNKCatcher1 = AddBand("Catcher 1", bandNKilos)
        Dim bandNKCatcher2 = AddBand("Catcher 2", bandNKilos)
        Dim bandNKTotal = AddBand("Total", bandNKilos)
        Dim bandNACatcher1 = AddBand("Catcher 1", bandNAmount)
        Dim bandNACatcher2 = AddBand("Catcher 2", bandNAmount)
        Dim bandNATotal = AddBand("Total", bandNAmount)

        BandedGridView1.PopulateColumns()

        BandedGridView1.Columns("Class").OwnerBand = bandClass
        BandedGridView1.Columns("Size").OwnerBand = bandSize
        BandedGridView1.Columns("Price").OwnerBand = bandPrice
        BandedGridView1.Columns("AUK_Catcher1").OwnerBand = bandAUKCatcher1
        BandedGridView1.Columns("AUK_Catcher2").OwnerBand = bandAUKCatcher2
        BandedGridView1.Columns("AUK_Total").OwnerBand = bandAUKTotal
        BandedGridView1.Columns("AUA_Catcher1").OwnerBand = bandAUACatcher1
        BandedGridView1.Columns("AUA_Catcher2").OwnerBand = bandAUACatcher2
        BandedGridView1.Columns("AUA_Total").OwnerBand = bandAUATotal
        BandedGridView1.Columns("SK_Catcher1").OwnerBand = bandSKCatcher1
        BandedGridView1.Columns("SK_Catcher2").OwnerBand = bandSKCatcher2
        BandedGridView1.Columns("SK_Total").OwnerBand = bandSKTotal
        BandedGridView1.Columns("SA_Catcher1").OwnerBand = bandSACatcher1
        BandedGridView1.Columns("SA_Catcher2").OwnerBand = bandSACatcher2
        BandedGridView1.Columns("SA_Total").OwnerBand = bandSATotal
        BandedGridView1.Columns("NK_Catcher1").OwnerBand = bandNKCatcher1
        BandedGridView1.Columns("NK_Catcher2").OwnerBand = bandNKCatcher2
        BandedGridView1.Columns("NK_Total").OwnerBand = bandNKTotal
        BandedGridView1.Columns("NA_Catcher1").OwnerBand = bandNACatcher1
        BandedGridView1.Columns("NA_Catcher2").OwnerBand = bandNACatcher2
        BandedGridView1.Columns("NA_Total").OwnerBand = bandNATotal

        BandedGridView1.Columns("Class").OptionsColumn.ReadOnly = True
        BandedGridView1.Columns("Size").OptionsColumn.ReadOnly = True
        bandClass.Fixed = Columns.FixedStyle.Left
        bandSize.Fixed = Columns.FixedStyle.Left

        BandedGridView1.BestFitColumns()
        BandedGridView1.OptionsView.ColumnAutoWidth = False
        BandedGridView1.OptionsView.ShowColumnHeaders = False
    End Sub

    Function AddBand(ByVal caption As String, ByRef parent As GridBand) As GridBand
        Dim band As New GridBand() With {.Caption = caption}
        parent.Children.Add(band)
        Return band
    End Function

    Function AddBand(ByVal caption As String, ByRef gridView As BandedGridView) As GridBand
        Dim band As New GridBand() With {.Caption = caption}
        gridView.Bands.Add(band)
        Return band
    End Function
End Class