Imports DevExpress.XtraTab
Imports DevExpress.XtraGrid
Imports DevExpress.XtraLayout
Imports DevExpress.XtraGrid.Views.BandedGrid

Public Class ucWeightSlip
    Inherits ucBase

    Private tabControl As XtraTabControl

    Sub New(ByVal title As String)
        InitializeComponent()

        MyBase.title = title
        LabelControl1.Text = title


        Dim grid = New GridControl() With {
            .Dock = DockStyle.Fill
        }

        LayoutControl2.Controls.Add(grid)

        Dim layoutItem = LayoutControl2.AddItem("", grid)

        Dim bandView = New BandedGridView(grid)

        grid.MainView = bandView
        grid.ViewCollection.Add(bandView)

        Dim bandSJ = createBand("Skip Jack", bandView)
        Dim bandYF = createBand("Yellow Fin", bandView)
        Dim bandBE = createBand("Big Eye", bandView)
        Dim bandBon = createBand("Bonito", bandView)

    End Sub

    Function createBand(ByVal caption As String, ByRef bandView As BandedGridView) As GridBand
        Dim band As New GridBand() With {
            .Caption = caption
        }
        bandView.Bands.Add(band)
        Return band
    End Function

    'Sub addBandToView(ByVal items() As Object, ByRef band As GridBand, ByRef bandView As BandedGridView)
    '    For Each item In items
    '        Dim colName As String = item(0)
    '        Dim colCaption As String = item(1)
    '        Dim colBand As BandedGridColumn = bandView.Columns.AddVisible(colName, colCaption)
    '        colBand.OwnerBand = band
    '    Next
    'End Sub

End Class
